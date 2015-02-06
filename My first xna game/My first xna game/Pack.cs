﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    public class Pack
    {
        public List<Item> items = new List<Item>();
        private ItemInstance itemInstance = new ItemInstance();


        public Pack()
        {
            AddItem(ItemInstance.apple);
            AddItem(ItemInstance.bread);
            AddItem(ItemInstance.healthPotion);
            AddItem(ItemInstance.strPotion);
            AddItem(ItemInstance.manaPotion);
            AddItem(ItemInstance.apple);
            AddItem(ItemInstance.bread);
            AddItem(ItemInstance.healthPotion);
            AddItem(ItemInstance.strPotion);
            AddItem(ItemInstance.manaPotion);
            AddItem(ItemInstance.apple);
            AddItem(ItemInstance.bread);
            AddItem(ItemInstance.healthPotion);
            AddItem(ItemInstance.strPotion);
            AddItem(ItemInstance.manaPotion);
        }

        public void CreateItems(Inventory inventory)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].icon = new Picture(Item.IconSet, new Vector2(i % inventory.margin * (Item.size + inventory.spacing), i / inventory.margin * (Item.size + inventory.spacing)), inventory.window);
                items[i].icon.drawingRect = new Rectangle(items[i].iconID % 16 * Item.size, items[i].iconID / 16 * Item.size, Item.size, Item.size);
                inventory.window.itemsList.Add(items[i].icon);
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