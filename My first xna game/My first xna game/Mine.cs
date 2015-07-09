using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Mine : Sprite
    {
        public Player source;
        public int strength;

        public Mine(Player source, int strength, Sprite sprite)
            : base(sprite.texture, sprite.position, sprite.drawingCoordinates)
        {
            this.source = source;
            this.strength = strength;

            MapDepth = Game.MapDepth.below;

            collisionFunction = Mine.UpdateMineCollision;
            canCollide = false;
            position = MovementManager.GetRectNextTo(bounds, source.bounds, source.direction);
            //position = MovementManager.MoveVector(position, Tile.size * 2, source.direction);
        }

        private static void UpdateMineCollision(GameObject gameObject, GameObject colidedWith)
        {
            //mine and hostile
            Hostile hostile = colidedWith as Hostile;
            if (hostile != null)
            {
                Mine mine = gameObject as Mine;
                if (CollisionManager.GameObjectCollision(hostile, mine, false) && hostile != mine.source)
                {
                    hostile.DealDamage(mine.source, false, mine.strength);
                    mine.Kill();
                }
            }
        }
    }
}
