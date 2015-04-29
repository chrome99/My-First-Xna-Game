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
    public class Enemy : Hostile
    {
        public List<Hostile> targetsList;
        private Hostile currentTarget;
        private bool huntMode = false;
        public int raduisSize = 5;
        public Rectangle raduis
        {
            get { return new Rectangle((int)position.X - raduisSize * Tile.size, (int)position.Y - raduisSize * Tile.size, bounds.Width + raduisSize * Tile.size * 2, bounds.Height + raduisSize * Tile.size * 2); }
        }

        public Enemy(Texture2D texture, Vector2 position)
            : base(texture, position, MovementManager.Auto.off)
        {
            this.targetsList = Map.defultTargetsList;
            movingState = MovementManager.MovingState.walking;
        }

        protected override void UpdateEnemy()
        {
            //update movement
            /*
             * the enemy has two modes:
             * search mode: the enemy is searching for a target.
             * hunt mode: the enemy hunts his pray until, his pray dies or gets away.
            */
            //search mode

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
                foreach (Hostile target in targetsList)
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
                if (targetsNotInRaduis == targetsList.Count)
                {
                    huntMode = false;
                }
                autoMovement = MovementManager.Auto.random;
            }

        }
        private void MoveToTarget(Hostile target)
        {
            autoMovement = MovementManager.Auto.off;
            direction = MovementManager.DirectionToGameObject(this, target);
            movementManager.MoveActor(this, direction, (int)speed);
        }
        private void MoveToTarget(Player target)
        {
            autoMovement = MovementManager.Auto.off;
            direction = MovementManager.DirectionToGameObject(this, target);
            movementManager.MoveActor(this, direction, (int)speed);
        }
    }
}
