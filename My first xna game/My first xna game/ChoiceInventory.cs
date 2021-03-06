﻿using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    class ChoiceInventory : Inventory
    {
        public delegate void ItemChoiceFunction(Item item);
        private ItemChoiceFunction handleItemChoice;

        public ChoiceInventory(Map map, Player player, ItemChoiceFunction handleItemChoice, Side side = Side.up, Filter filter = Filter.all)
            : base(map, player, true, side, filter)
        {
            this.handleItemChoice = handleItemChoice;
        }

        protected override void HandleItemChoice()
        {
            Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
            if (filter == Filter.all)
            {
                handleItemChoice(pack.items[selector.currentTargetNum]);
            }
            else
            {
                Item result = null;
                foreach (Item item in pack.items)
                {
                    if (item.icon == selector.currentTarget)
                    {
                        result = item;
                        break;
                    }
                }
                handleItemChoice(result);
            }
        }
    }
}
