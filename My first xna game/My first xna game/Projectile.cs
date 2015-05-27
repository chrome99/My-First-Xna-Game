using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace My_first_xna_game
{
    public class Projectile : Spritesheet
    {
        public Player source;
        private int pathDestination = 50;
        private int pathTravelled = 0;
        private MovementManager.Direction direction;
        private SoundEffect launchSound;
        private SoundEffect hitSound;

        public Projectile(Map map, Texture2D texture, float speed, Player source, int pathDestination, SoundEffect launchSound, SoundEffect hitSound)
            : base(texture, source.position, Game.Depth.projectiles, speed)
        {
            this.source = source;
            this.pathDestination = pathDestination;
            this.launchSound = launchSound;
            this.hitSound = hitSound;

            direction = source.direction;
            map.AddObject(this);

            if (launchSound != null)
            {
                launchSound.Play();
            }
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
            movementManager.MoveToDirection(this, this.speed, direction);
            StartAnimation(direction);
            pathTravelled++;
            if (pathTravelled == pathDestination)
            {
                fade = true;
                //texture.Dispose();
                // TODO: Destroy instance
            }
        }
    }
}
