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

        public Projectile(Map map, Texture2D texture, float speed, Player source, int maxLength)
            : base(texture, source.position, Game.Depth.projectiles, speed)
        {
            this.source = source;
            this.pathDestination = maxLength;
            direction = source.direction;
            map.AddObject(this);
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
