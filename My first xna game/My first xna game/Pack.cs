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

        public float getWeight
        {
            get
            {
                float result = 0;
                foreach (Item item in items)
                {
                    result += item.weight * item.amount;
                }
                return result;
            }
        }

        public Pack(Actor source)
        {
            this.source = source;
        }

        public void CreateItems(Inventory inventory, Window InventoryWindow)
        {
            Armor armor;
            for (int i = 0; i < items.Count; i++)
            {
                switch (inventory.filter)
                {
                    case Inventory.Filter.all:
                        CreateItem(items[i], i, inventory, InventoryWindow);
                        break;

                    case Inventory.Filter.armor:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            CreateItem(items[i], i, inventory, InventoryWindow);
                        }
                        break;

                    case Inventory.Filter.head:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.head)
                            {
                                CreateItem(items[i], i, inventory, InventoryWindow);
                            }
                        }
                        break;

                    case Inventory.Filter.body:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.body)
                            {
                                CreateItem(items[i], i, inventory, InventoryWindow);
                            }
                        }
                        break;

                    case Inventory.Filter.shoes:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.shoes)
                            {
                                CreateItem(items[i], i, inventory, InventoryWindow);
                            }
                        }
                        break;

                    case Inventory.Filter.oneHanded:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.oneHanded)
                            {
                                CreateItem(items[i], i, inventory, InventoryWindow);
                            }
                        }
                        break;

                    case Inventory.Filter.twoHanded:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.twoHanded)
                            {
                                CreateItem(items[i], i, inventory, InventoryWindow);
                            }
                        }
                        break;

                    case Inventory.Filter.weapon:
                        armor = items[i] as Armor;
                        if (armor != null)
                        {
                            if (armor.armorType == Armor.ArmorType.oneHanded || armor.armorType == Armor.ArmorType.twoHanded)
                            {
                                CreateItem(items[i], i, inventory, InventoryWindow);
                            }
                        }
                        break;
                }
            }

        }

        private void CreateItem(Item item, int i, Inventory inventory, Window InventoryWindow)
        {
            item.icon = new Picture(Item.IconSet, new Vector2(i % inventory.margin * (Item.size + inventory.spacing), i / inventory.margin * (Item.size + inventory.spacing)), InventoryWindow);
            item.icon.fileDrawingRect = item.getRect;
            if (item.amount > 1)
            {
                Text amount = new Text(Game.content.Load<SpriteFont>("Fonts\\small"), Vector2.Zero, new Color(255, 255, 255), items[i].amount + "X", InventoryWindow, new Vector2(1, 2), true);
                amount.position = new Vector2(i % inventory.margin * (Item.size + inventory.spacing), i / inventory.margin * (Item.size + inventory.spacing));
                amount.WindowDepth = Game.WindowDepth.windowsDataFront;
                inventory.amountTexts.Add(amount);
            }
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
            UpdatePlayerWeight(true);
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
                        representation.RemoveWindowItem(result, true, true);
                        representation.amountTexts.Remove(result);
                    }
                    else
                    {
                        result.text = item.amount - 1 + "X";
                    }
                }
                item.amount--;
                UpdatePlayerWeight(false);
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
                representation.RemoveWindowItem(item.icon, true);
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
            UpdatePlayerWeight(false);
        }
        private void UpdatePlayerWeight(bool add)
        {
            //update player running option by weight
            Player player = source as Player;
            if (player != null)
            {
                if (add) //add item
                {
                    if (!player.maxWeightReached)
                    {
                        if (getWeight >= player.maxPackWeight)
                        {
                            player.enableRunning = false;
                            player.maxWeightReached = true;
                        }
                    }
                }
                else //sub item
                {
                    if (player.maxWeightReached)
                    {
                        if (getWeight < player.maxPackWeight)
                        {
                            player.enableRunning = true;
                            player.maxWeightReached = false;
                        }
                    }
                }
            }
        }
    }
}
