using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    class ItemCollection
    {
        public static Item mine = new Item(113, useMine, 5, 0.2f, true);

        public static Item apple = new Item(169, useApple, 5, 0.2f, true);
        public static Item bread = new Item(170, useBread, 12, 0.2f, true);
        public static Item healthPotion = new Item(64, useHealthPotion, 12, 0.5f, true);
        public static Item manaPotion = new Item(65, useManaPotion, 12, 0.5f, true);
        public static Item strPotion = new Item(66, useStrPotion, 12, 0.5f, true);

        //armor
        public static Armor occupiedArmor = new Armor(0, 0, 0, 0, Armor.ArmorType.oneHanded, new Hostile.Stats()); 

        //  head
        public static Armor hat = new Armor(34, 35, 0.5f, 15, Armor.ArmorType.head, new Hostile.Stats { maxHealth = 10 });
        public static Armor mask = new Armor(39, 25, 0.5f, 30, Armor.ArmorType.head, new Hostile.Stats { agility = 5 });
        public static Armor helmate = new Armor(32, 20, 2, 45, Armor.ArmorType.head, new Hostile.Stats { defence = 2 });

        //  body
        public static Armor shirt = new Armor(45, 20, 2, 20, Armor.ArmorType.body, new Hostile.Stats { defence = 2 });
        public static Armor copperChestArmor = new Armor(41, 30, 4, 30, Armor.ArmorType.body, new Hostile.Stats { defence = 5 });
        public static Armor ironChestArmor = new Armor(40, 50, 5, 60, Armor.ArmorType.body, new Hostile.Stats { defence = 10 });

        //  shoes
        public static Armor leatherShoes = new Armor(48, 20, 2, 15, Armor.ArmorType.shoes, new Hostile.Stats { agility = 3 });
        public static Armor ironBoots = new Armor(49, 30, 4, 40, Armor.ArmorType.shoes, new Hostile.Stats { defence = 3, agility = 1 });
        public static Armor goldBoots = new Armor(323, 50, 5, 60, Armor.ArmorType.shoes, new Hostile.Stats { defence = 7, agility = -3 });

        // one handed
        public static Armor ironSword = new Armor(2, 20, 1.5f, 10, Armor.ArmorType.oneHanded, new Hostile.Stats { strength = 3 });

        // two handed
        public static Bow woodenStaff = new Bow(20, 25, 1, 25, Armor.ArmorType.twoHanded,
            new Bow.ProjectileData {
                texture = Game.content.Load<Texture2D>("Textures\\Spritesheets\\wolf"),
                hitSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
                launchSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"),
                lit = true, lightLevel = 150, lightColor = Color.Red, lightOpacity = 100,
                pathDestination = 60, speed = 6
            }, new Hostile.Stats { strength = 3 });

        public static List<Item> list = new List<Item> { apple, healthPotion, manaPotion, strPotion };

        public static Item RandomItem()
        {
            Random random = new Random();
            return list[random.Next(list.Count - 1)];
        }

        public static Item CopyItem(Item item)
        {
            Armor armor = item as Armor;
            if (armor != null)
            {
                return new Armor(armor.iconID, armor.price, armor.weight, armor.Durability, armor.armorType, armor.changeStats);
            }
            else
            {
                return new Item(item.iconID, item.function, item.price, item.weight, item.wasted);
            }
        }

        public static void useMine(Hostile source, GameObject target, Item item)
        {
            Player player = source as Player;
            PlayerObject mine = PlayerObject.SpriteToPlayerObject(player, item.getSprite);

            Vector2 newPosition;
            newPosition.X = source.position.X + source.bounds.Width / 2 - mine.bounds.Width / 2;
            newPosition.Y = source.position.Y + source.bounds.Height / 2 - mine.bounds.Height / 2;
            mine.position = MovementManager.MoveVector(newPosition, Tile.size * 2, source.direction);

            mine.AddLight(100, Color.Green);
            mine.canCollide = false;
            mine.collisionFunction = UpdateMineCollision;
            item.Spawn(player.map, mine);
            
        }

        private static void UpdateMineCollision(GameObject gameObject)
        {
            PlayerObject mine = gameObject as PlayerObject;

            //mine and player
            foreach (GameObject gameObject2 in mine.source.map.gameObjectList)
            {
                Hostile hostile = gameObject2 as Hostile;
                if (hostile != null)
                {
                    if (CollisionManager.GameObjectCollision(hostile, mine, false) && hostile != mine.source)
                    {
                        hostile.DealDamage(mine.source, 20);
                        mine.Kill();
                    }
                }
            }
        }

        public static void useApple(Hostile source, GameObject target, Item item)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.health += 5;
            }
        }

        public static void useBread(Hostile source, GameObject target, Item item)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.health += 10;
            }
        }

        public static void useHealthPotion(Hostile source, GameObject target, Item item)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.health = hostile.stats.maxHealth;
            }
        }

        public static void useManaPotion(Hostile source, GameObject target, Item item)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.mana = hostile.stats.maxMana;
            }
        }

        public static void useStrPotion(Hostile source, GameObject target, Item item)
        {
            Hostile hostile = target as Hostile;
            if (hostile != null)
            {
                hostile.stats.strength += 5;
            }
        }
    }
}
