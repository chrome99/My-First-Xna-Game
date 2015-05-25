using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My_first_xna_game
{
    class ChoiceInventory : Inventory
    {
        public delegate void ItemChoiceFunction(Item item);
        private ItemChoiceFunction handleItemChoice;

        public ChoiceInventory(Player player, ItemChoiceFunction handleItemChoice, Side side = Side.up, Filter filter = Filter.all)
            : base(player, true, side, filter)
        {
            this.handleItemChoice = handleItemChoice;
        }

        protected override void HandleItemChoice()
        {
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
