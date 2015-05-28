using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Item
    {
        public static int size = 24;
        public static int itemsInRow = 16;
        public static Texture2D IconSet = Game.content.Load<Texture2D>("Textures\\Icons\\IconSet1");
        public delegate void UsingFunction(Hostile source, GameObject target);
        public int iconID;
        public UsingFunction function;
        public bool wasted;
        public Picture icon;
        public int price;
        public float weight;
        public int amount;

        public Item(int iconID, UsingFunction function, int price, float weight, bool wasted)
        {
            this.iconID = iconID;
            this.function = function;
            this.price = price;
            this.weight = weight;
            this.wasted = wasted;
            amount = 1;
        }

        public Rectangle getRect()
        {
            return new Rectangle(iconID % Item.itemsInRow * Item.size, iconID / Item.itemsInRow * Item.size, Item.size, Item.size);
        }
    }
}
