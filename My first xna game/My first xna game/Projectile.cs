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
            public int strength;
            public bool exploading;
            public bool sourceDamage;
        }

        public Player source;
        public int strength;
        private bool exploading = false;
        private bool sourceDamage = true;
        private int pathDestination = 50;
        private int pathTravelled = 0;
        private MovementManager.Direction direction;
        private SoundEffect launchSound;
        private SoundEffect hitSound;

        public Projectile(Texture2D texture, float speed, Player source, int pathDestination, int strength, SoundEffect launchSound, SoundEffect hitSound)
            : base(texture, new Vector2())
        {
            this.source = source;
            this.pathDestination = pathDestination;
            this.launchSound = launchSound;
            this.hitSound = hitSound;
            this.speed = speed;
            this.strength = strength;
            this.direction = source.direction;

            Vector2 result = MovementManager.GetRectNextTo(bounds, source.bounds, source.direction);
            position = result;

            if (launchSound != null)
            {
                launchSound.Play();
            }
        }

        public void Colide(Hostile target)
        {
            if (hitSound != null)
            {
                hitSound.Play();
            }
            target.DealDamage(source, sourceDamage, strength);
            Kill();
        }

        protected override void UpdateProjectile()
        {
            //-update movement
            movementManager.MoveToDirection(this, direction, (int)this.speed);
            StartAnimation(direction);
            pathTravelled += (int)speed;
            if (pathTravelled - pathDestination >= 0 && pathTravelled - pathDestination < speed)
            {
                Fade();
                //texture.Dispose();
                // TODO: Destroy instance
            }
        }

        public static void LaunchProjectile(ProjectileData projectileData, Map map, Player source)
        {
            Projectile projectile = new Projectile(projectileData.texture, projectileData.speed, source,
                projectileData.pathDestination, projectileData.strength, projectileData.launchSound, projectileData.hitSound);
            projectile.exploading = projectileData.exploading;
            projectile.sourceDamage = projectileData.sourceDamage;

            if (projectileData.lit)
            {
                projectile.AddLight(new LightSource(projectileData.lightLevel, projectileData.lightColor, projectileData.lightOpacity));
            }

            projectile.passable = true;

            map.AddObject(projectile);
        }
    }
}
