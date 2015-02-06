using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Item
    {
        public static int size = 24;
        public static Texture2D IconSet = Game.content.Load<Texture2D>("IconSet1");
        public delegate void UsingFunction(Hostile source, GameObject target);
        public int iconID;
        public UsingFunction function;
        public bool oneTime;
        public Picture icon;
        public int price;
        public float weight;

        public Item(int iconID, UsingFunction function, int price, float weight, bool oneTime)
        {
            this.iconID = iconID;
            this.function = function;
            this.price = price;
            this.weight = weight;
            this.oneTime = oneTime;
        }
    }
}
