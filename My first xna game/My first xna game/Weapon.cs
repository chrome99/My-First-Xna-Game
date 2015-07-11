using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My_first_xna_game
{
    public class Weapon : Armor
    {
        public Weapon(int iconID, int price, float weight, int durability, ArmorType armorType, Player.Stats changeStats)
            : base(iconID, price, weight, durability, armorType, changeStats)
        {

        }
        public virtual void Attack(Map map, Player player)
        {
            Durability--;
        }
    }
}
