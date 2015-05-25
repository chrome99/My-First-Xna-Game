using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Pack
    {
        public Inventory representation;
        public List<Item> items = new List<Item>();
        private Actor source;

        public Pack(Actor source)
        {
            this.source = source;
        }

        public void CreateItems(Inventory inventory)
        {
            Armor armor;
            for (int i = 0; i < items.Count; i++)
            {
                switch (inventory.filter)
                {
                    case Inventory.Filter.all:
                        CreateItem(items[i], i, inventory);
                        break;

                    case Inventory.Filter.armor:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            CreateItem(items[i], i, inventory);
                        }
                        break;

                    case Inventory.Filter.head:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.head)
                            {
                                CreateItem(items[i], i, inventory);
                            }
                        }
                        break;

                    case Inventory.Filter.body:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.body)
                            {
                                CreateItem(items[i], i, inventory);
                            }
                        }
                        break;

                    case Inventory.Filter.shoes:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.shoes)
                            {
                                CreateItem(items[i], i, inventory);
                            }
                        }
                        break;

                    case Inventory.Filter.oneHanded:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.oneHanded)
                            {
                                CreateItem(items[i], i, inventory);
                            }
                        }
                        break;

                    case Inventory.Filter.twoHanded:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.twoHanded)
                            {
                                CreateItem(items[i], i, inventory);
                            }
                        }
                        break;

                    case Inventory.Filter.weapon:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.oneHanded || armor.armorType == Armor.ArmorType.twoHanded)
                            {
                                CreateItem(items[i], i, inventory);
                            }
                        }
                        break;
                }
            }

        }

        private void CreateItem(Item item, int i, Inventory inventory)
        {
            item.icon = new Picture(Item.IconSet, new Vector2(i % inventory.margin * (Item.size + inventory.spacing), i / inventory.margin * (Item.size + inventory.spacing)), inventory.window);
            item.icon.drawingRect = item.getRect();
            if (item.amount > 1)
            {
                Text amount = new Text(Game.content.Load<SpriteFont>("small"), Vector2.Zero, new Color(255, 255, 255), items[i].amount + "X", inventory.window);
                amount.position = new Vector2(i % inventory.margin * (Item.size + inventory.spacing), i / inventory.margin * (Item.size + inventory.spacing));
                amount.depth = Game.DepthToFloat(Game.Depth.windowsDataFront);
                inventory.amountTexts.Add(amount);
            }
            inventory.window.AddItem(item.icon);
        }

        public void AddItem(List<Item> itemsList)
        {
            foreach(Item item in itemsList)
            {
                AddItem(item);
            }
        }

        public void AddItem(Item item)
        {
            // if there is already a same item
            Item sameItem = null;
            foreach (Item identicalItem in items)
            {
                if (identicalItem.iconID == item.iconID)
                {
                    sameItem = identicalItem;
                }
            }
            if (sameItem != null)
            {
                sameItem.amount++;
                return;
            }

            items.Add(item);
        }

        public void SubItem(Item item)
        {
            if (item.amount > 1)
            {
                Text result = null;
                foreach(Item item2 in items)
                {
                    foreach (Text amount in representation.amountTexts)
                    {
                        if (item.icon.position == amount.position)
                        {
                            result = amount;
                        }
                    }
                }
                if (result != null)
                {
                    if (item.amount == 2)
                    {
                        representation.amountTexts.Remove(result);
                        result = null;
                    }
                    else
                    {
                        result.text = item.amount - 1 + "X";
                    }
                }
                item.amount--;
                return;
            }

            if (representation.alive)
            {
                //remove price
                ShopInventory shopInventory = representation as ShopInventory;
                if (shopInventory != null)
                {
                    shopInventory.SubPrice(item.icon);
                }

                //remove window item
                representation.RemoveWindowItem(item.icon);
            }

            //check if the item is equiped
            Hostile hostile = source as Hostile;
            if (hostile != null)
            {
                Armor armor = item as Armor;
                if (armor != null)
                {
                    List<Armor> identicalArmorsList = new List<Armor>();
                    foreach (Armor armor2 in hostile.equipmentList)
                    {
                        if (armor.iconID == armor2.iconID)
                        {
                            identicalArmorsList.Add(armor2);
                        }
                    }
                    if (identicalArmorsList.Count == 1)
                    {
                        hostile.UnEquip(identicalArmorsList[0]);
                    }
                }
            }
            
            //remove item
            items.Remove(item);
        }
    }
}
