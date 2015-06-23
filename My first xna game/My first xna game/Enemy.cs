using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Enemy : Hostile
    {
        public List<Hostile> hostilesList;
        private Hostile currentTarget;
        private bool huntMode = false;
        public int radiusSize = 5;

        public Rectangle raduis
        {
            get { return new Rectangle((int)position.X - radiusSize * Tile.size, (int)position.Y - radiusSize * Tile.size, bounds.Width + radiusSize * Tile.size * 2, bounds.Height + radiusSize * Tile.size * 2); }
        }

        public Enemy(Texture2D texture, Vector2 position)
            : base(texture, position, MovementManager.Auto.off)
        {
            MovingState = MovementManager.MovingState.walking;
        }

        protected override void UpdateEnemy()
        {
            //update movement
            /*
             * the enemy has 3 modes:
             * chill mode: the enemy is ba chill.
             * search mode: the enemy is searching for a target.
             * hunt mode: the enemy hunts his pray until his pray dies or runs away.
            */

            if (huntMode)
            {
                MoveToTarget(currentTarget);
                if (!raduis.Intersects(currentTarget.core) || !currentTarget.alive)
                {
                    huntMode = false;
                }
            }
            else
            {
                int targetsNotInRaduis = 0;
                foreach (Hostile target in hostilesList)
                {
                    if (raduis.Intersects(target.core) && target.alive)
                    {
                        currentTarget = target;
                        huntMode = true;
                    }
                    else
                    {
                        targetsNotInRaduis++;
                    }
                }
                if (targetsNotInRaduis == hostilesList.Count)
                {
                    huntMode = false;
                }
                autoMovement = MovementManager.Auto.random;
            }

        }
        private void MoveToTarget(Hostile target)
        {
            MovingState = MovementManager.MovingState.walking;
            autoMovement = MovementManager.Auto.off;
            direction = MovementManager.DirectionToGameObject(this, target);
            movementManager.MoveActor(this, direction, (int)speed);
        }
    }
}
