using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    class ItemCollection
    {
        public static Item apple = new Item(169, useApple, 5, 0.2f, true);
        public static Item bread = new Item(170, useBread, 12, 0.2f, true);
        public static Item healthPotion = new Item(64, useHealthPotion, 12, 0.5f, true);
        public static Item manaPotion = new Item(65, useManaPotion, 12, 0.5f, true);
        public static Item strPotion = new Item(66, useStrPotion, 12, 0.5f, true);
        public static Armor ironChestArmor = new Armor(46, 20, 1, Armor.ArmorType.chest, new Hostile.Stats { defence = 5 });
        public static Armor ironSword = new Armor(2, 20, 1, Armor.ArmorType.oneHanded, new Hostile.Stats { strength = 3 });
        public static Armor occupiedArmor = new Armor(0, 0, 0, Armor.ArmorType.oneHanded, new Hostile.Stats()); 

        public static List<Item> list = new List<Item>{apple, bread, healthPotion, manaPotion, strPotion};

        public static Item RandomItem()
        {
            Random random = new Random();
            return list[random.Next(list.Count - 1)];
        }

        public static void useApple(Hostile source, GameObject target)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.health += 5;
            }
        }

        public static void useBread(Hostile source, GameObject target)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.health += 10;
            }
        }

        public static void useHealthPotion(Hostile source, GameObject target)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.health = hostile.stats.maxHealth;
            }
        }

        public static void useManaPotion(Hostile source, GameObject target)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.mana = hostile.stats.maxMana;
            }
        }

        public static void useStrPotion(Hostile source, GameObject target)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.strength += 5;
            }
        }
    }
}
