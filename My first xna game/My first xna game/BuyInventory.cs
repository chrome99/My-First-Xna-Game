using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    class BuyInventory : Inventory
    {
        private List<Text> priceTexts = new List<Text>();

        public BuyInventory(Player player, Actor merchant) :base(player, false)
        {
            //create items
            CreatePack(merchant);

            //create price texts
            for (int counter = 0; counter < pack.items.Count; counter++)
            {
                Text price = new Text(Game.content.Load<SpriteFont>("small"), Vector2.Zero, new Color(255, 215, 0), pack.items[counter].price.ToString(), window);
                price.position = new Vector2(counter % margin * (Item.size + spacing) + Item.size / 2, counter / margin * (Item.size + spacing) + Item.size / 2);
                price.depth = Game.DepthToFloat(Game.Depth.windowsDataFront);
                priceTexts.Add(price);
            }
        }

        private void SortPriceTexts()
        {
            for (int counter = 0; counter < priceTexts.Count; counter++)
            {
                priceTexts[counter].position = new Vector2(counter % margin * (Item.size + spacing) + Item.size / 2, counter / margin * (Item.size + spacing) + Item.size / 2);
            }
        }

        protected override void HandleItemChoice()
        {
            Item currentItem = pack.items[selector.currentTargetNum];
            if (player.gold - currentItem.price > 0)
            {
                pack.SubItem(currentItem);
                priceTexts[selector.currentTargetNum] = null;
                priceTexts.Remove(priceTexts[selector.currentTargetNum]);
                player.pack.AddItem(currentItem);
                player.gold -= currentItem.price;
                window.itemsList.Remove(selector.currentTarget);
                SortItems();
                SortPriceTexts();
                selector.Clamp();
            }
            else
            {
                priceTexts[selector.currentTargetNum].ChangeColorEffect(Color.Red, 1000f);
            }
        }

        protected override void UpdateBuyInventory()
        {
            foreach(Text price in priceTexts)
            {
                price.Update(null);
            }
        }

        public override void DrawBuyInventory(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            foreach(Text price in priceTexts)
            {
                price.Draw(spriteBatch, offsetRect, screenPosition);
            }
        }
    }
}
