using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Mine : Sprite
    {
        public Player source;
        public int strength;

        public Mine(Player source, int strength, Sprite sprite)
            : base(sprite.texture, sprite.position, sprite.depth, sprite.drawingCoordinates)
        {
            this.source = source;
            this.strength = strength;

            collisionFunction = Mine.UpdateMineCollision;
            canCollide = false;
            MovementManager.PositionNextTo(this, source, Tile.size * 2);
        }

        private static void UpdateMineCollision(GameObject gameObject)
        {
            Mine mine = gameObject as Mine;

            //mine and player
            foreach (GameObject gameObject2 in mine.source.map.gameObjectList)
            {
                Hostile hostile = gameObject2 as Hostile;
                if (hostile != null)
                {
                    if (CollisionManager.GameObjectCollision(hostile, mine, false) && hostile != mine.source)
                    {
                        hostile.DealDamage(mine.source, mine.strength);
                        mine.Kill();
                    }
                }
            }
        }
    }
}
