using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Enemy : Hostile
    {
        private enum EnemyAI { chill, search, hunt };
        private EnemyAI enemyIA = EnemyAI.chill;
        public List<Hostile> hostilesList;
        private Hostile currentTarget;
        public int radiusSize = 5;
        private Timer reConstructWayToTargetTimer = new Timer(500f, true);

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

            switch (enemyIA)
            {
                case EnemyAI.hunt:
                    if (reConstructWayToTargetTimer.result)
                    {
                        MoveToTarget(currentTarget);
                        reConstructWayToTargetTimer.Reset();
                    }
                    if (!raduis.Intersects(currentTarget.core) || !currentTarget.alive)
                    {
                        enemyIA = EnemyAI.chill;
                        destinationsList.Clear();
                    }
                    break;

                case EnemyAI.search:

                    break;

                case EnemyAI.chill:
                    int targetsNotInRaduis = 0;
                    foreach (Hostile target in hostilesList)
                    {
                        if (raduis.Intersects(target.core) && target.alive)
                        {
                            currentTarget = target;
                            enemyIA = EnemyAI.hunt;
                        }
                        else
                        {
                            targetsNotInRaduis++;
                        }
                    }
                    if (targetsNotInRaduis == hostilesList.Count)
                    {
                        enemyIA = EnemyAI.chill;
                    }
                    autoMovement = MovementManager.Auto.random;
                    break;
            }

        }
        private void MoveToTarget(Hostile target)
        {
            MovingState = MovementManager.MovingState.walking;
            autoMovement = MovementManager.Auto.off;
            movementManager.HighlightWayTo(new Vector2(core.X, core.Y), new Vector2(target.core.X, target.core.Y), raduis);
            List<Vector2> way = movementManager.WayTo(new Vector2(core.X, core.Y), new Vector2(target.core.X, target.core.Y), raduis);
            if (way != null)
            {
                destinationsList = way;
            }
            //MovingState = MovementManager.MovingState.walking;
            //autoMovement = MovementManager.Auto.off;
            //direction = MovementManager.DirectionToGameObject(this, target);
            //movementManager.MoveActor(this, direction, (int)speed);
        }
    }
}
