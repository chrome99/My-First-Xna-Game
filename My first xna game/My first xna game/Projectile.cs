using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Projectile : Spritesheet
    {
        public struct ProjectileData
        {
            public Texture2D texture;
            public SoundEffect launchSound;
            public SoundEffect hitSound;
            public bool lit;
            public int lightLevel;
            public Color lightColor;
            public int lightOpacity;
            public int speed;
            public int pathDestination;
        }

        public Player source;
        private int pathDestination = 50;
        private int pathTravelled = 0;
        private MovementManager.Direction direction;
        private SoundEffect launchSound;
        private SoundEffect hitSound;

        public Projectile(Texture2D texture, float speed, Player source, int pathDestination, SoundEffect launchSound, SoundEffect hitSound)
            : base(texture, new Vector2(), Game.Depth.projectiles)
        {
            this.source = source;
            this.pathDestination = pathDestination;
            this.launchSound = launchSound;
            this.hitSound = hitSound;
            this.speed = speed;

            direction = source.direction;
            position.X = source.position.X + source.bounds.Width / 2 - bounds.Width / 2;
            position.Y = source.position.Y + source.bounds.Height / 2 - bounds.Height / 2;
            position = MovementManager.MoveVector(position, Tile.size, direction);

            if (launchSound != null)
            {
                launchSound.Play();
            }
        }

        public void AddToGameObjectList(Map map)
        {
            map.AddObject(this);
        }

        public void PlayHitSound()
        {
            if (hitSound != null)
            {
                hitSound.Play();
            }
        }

        protected override void UpdateProjectile()
        {
            //-update movement
            movementManager.MoveToDirection(this, direction, (int)this.speed);
            StartAnimation(direction);
            pathTravelled++;
            if (pathTravelled == pathDestination)
            {
                Fade();
                //texture.Dispose();
                // TODO: Destroy instance
            }
        }

        public static void LaunchProjectile(ProjectileData projectileData, Map map, Player source)
        {
            Projectile projectile = new Projectile(projectileData.texture, projectileData.speed, source,
                projectileData.pathDestination, projectileData.launchSound, projectileData.hitSound);

            if (projectileData.lit)
            {
                projectile.AddLight(projectileData.lightLevel, projectileData.lightColor, projectileData.lightOpacity);
            }

            projectile.AddToGameObjectList(map);

            projectile.passable = true;
        }
    }
}
