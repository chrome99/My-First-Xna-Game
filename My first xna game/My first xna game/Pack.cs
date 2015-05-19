using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    public class Pack
    {
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
                items[i].icon.drawingRect = new Rectangle(items[i].iconID % 16 * Item.size, items[i].iconID / 16 * Item.size, Item.size, Item.size);
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
            Item sameItem = FindIdenticalItem(item);
            if (sameItem != null)
            {
                sameItem.amount++;
                return;
            }

            items.Add(new Item(item.iconID, item.function, item.price, item.weight, item.wasted));
        }

        public void SubItem(Item item)
        {
            if (item.amount == 1)
            {
                items.Remove(item);
            }
            else
            {
                item.amount--;
            }
        }
    }
}
