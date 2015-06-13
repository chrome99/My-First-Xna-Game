using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace My_first_xna_game
{
    public class Item
    {
        public static int size = 24;
        public static int itemsInRow = 16;
        public static Texture2D IconSet = Game.content.Load<Texture2D>("Textures\\Icons\\IconSet1");
        public int iconID;
        public bool wasted;
        [XmlIgnore]
        public Picture icon;
        public int price;
        public float weight;
        public int amount;

        public delegate void UsingFunction(Hostile source, GameObject target, Item item);

        [XmlIgnore]
        public UsingFunction function;

        public Item() { }

        public Item(int iconID, UsingFunction function, int price, float weight, bool wasted)
        {
            this.iconID = iconID;
            this.function = function;
            this.price = price;
            this.weight = weight;
            this.wasted = wasted;
            amount = 1;
        }

        public Rectangle getRect
        {
            get { return new Rectangle(iconID % Item.itemsInRow * Item.size, iconID / Item.itemsInRow * Item.size, Item.size, Item.size); }
        }

        public Sprite getSprite
        {
            get { return new Sprite(Item.IconSet, Vector2.Zero, Game.Depth.above, 2f, getRect); }
        }

        public void Spawn(Map map, GameObject gameObject)
        {
            map.AddObject(gameObject);
        }
    }
}
