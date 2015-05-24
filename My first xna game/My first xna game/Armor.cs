using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Armor : Item
    {
        //new private UsingFunction function;
        public enum ArmorType { head, oneHanded, twoHanded, chest, legs, shoes }
        public ArmorType armorType;
        public Player.Stats changeStats;


        public Armor(int iconID, int price, float weight, ArmorType armorType, Player.Stats changeStats)
            : base(iconID, null, price, weight, false)
        {
            this.armorType = armorType;
            this.changeStats = changeStats;
        }
    }
}
