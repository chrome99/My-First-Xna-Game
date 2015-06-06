using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class SpawnableItem : Item
    {
        Map map;

        public SpawnableItem(Map map, int iconID, UsingFunction function, int price, float weight, bool wasted) : base(iconID, function, price, weight, wasted)
        {
            this.map = map;
        }

        public void Spawn(Vector2 position)
        {
            map.AddObject(new Sprite(Item.IconSet, position, Game.Depth.above, 2f, getRect()));
        }
    }
}
