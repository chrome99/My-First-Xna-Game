using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    public class Pack
    {
        public Inventory representation;
        public List<Item> items = new List<Item>();
        private ItemCollection itemInstance = new ItemCollection();

        public Pack()
        {

        }

        public void CreateItems(Inventory inventory)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].icon = new Picture(Item.IconSet, new Vector2(i % inventory.margin * (Item.size + inventory.spacing), i / inventory.margin * (Item.size + inventory.spacing)), inventory.window);
                items[i].icon.drawingRect = items[i].getRect();
                inventory.window.AddItem(items[i].icon);
            }

        }

        public Item FindIdenticalItem(Item item)
        {
            //return items.Contains(item);
            //if item already exists
            foreach(Item identicalItem in items)
            {
                if (ReferenceEquals(identicalItem, item))//identicalItem.Equals((object)item))
                {
                    return identicalItem;
                }
            }
            return null;
             /*
            Item identicalItem = items.Find(sameItem => sameItem.Equals(item));
            return identicalItem;
            */
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
            /*Item sameItem = FindIdenticalItem(item);
            if (sameItem != null)
            {
                sameItem.amount++;
                return;
            }*/

            items.Add(new Item(item.iconID, item.function, item.price, item.weight, item.wasted));
        }

        public void SubItem(Item item, bool checkOtherPlayers = true)
        {
            int result = -1;
            if (checkOtherPlayers)
            {
                for (int counter = 0; counter < items.Count; counter++)
                {
                    if (items[counter].Equals(item))
                    {
                        result = counter;
                        break;
                    }
                }
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

            
            //remove item
            items.Remove(item);
            /*
            if (checkOtherPlayers)
            {
                if (PlayerManager.TwoPlayersSameShop(result))
                {

                }
                else
                {
                    items.Remove(item);
                }
            }
            else
            {
                items.Remove(item);
            }*/
            /*if (item.amount == 1)
            {
                items.Remove(item);
            }
            else
            {
                item.amount--;
            }*/
        }
    }
}
