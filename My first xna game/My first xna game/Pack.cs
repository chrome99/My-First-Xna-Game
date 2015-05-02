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

        public void AddItem(Item item)
        {
            items.Add(new Item(item.iconID, item.function, item.price, item.weight, item.oneTime));
        }

        public bool SubItem(Item item)
        {
            return items.Remove(item);
        }
    }
}
