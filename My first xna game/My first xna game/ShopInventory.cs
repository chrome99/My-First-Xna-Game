using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    public class ShopInventory : Inventory//todo: private
    {
        private Actor merchant;
        private List<Text> priceTexts = new List<Text>();
        private bool sellInventory;

        //buy inventory
        public ShopInventory(Map map, Player player, Actor merchant, bool sellInventory)
            : base(map, player, false)
        {
            //intialize shop state
            this.sellInventory = sellInventory;

            //create items
            if (sellInventory)
            {
                this.merchant = merchant;
                sourcePack = player;
            }
            else
            {
                sourcePack = merchant;
            }
            CreateWindowItems();
            CreatePriceTexts();
        }

        private int getPrice(int price)
        {
            if (sellInventory)
            {
                return price / 2;
            }
            else
            {
                return price;
            }
        }
        
        public void CreatePriceTexts()
        {
            for (int counter = 0; counter < pack.items.Count; counter++)
            {
                Text price = new Text(Game.content.Load<SpriteFont>("Fonts\\small"), Vector2.Zero, new Color(255, 215, 0), getPrice(pack.items[counter].price).ToString(), window, new Vector2(1, 2));
                price.position = new Vector2(counter % margin * (Item.size + spacing) + Item.size / 2, counter / margin * (Item.size + spacing) + Item.size / 2);
                price.depth = Game.DepthToFloat(Game.Depth.windowsDataFront);
                priceTexts.Add(price);
            }
        }

        public void DeletePriceTexts()
        {
            for (int counter = 0; counter < priceTexts.Count;)
            {
                SubPrice(priceTexts[counter]);
            }
        }
        
        private void SortPriceTexts()
        {
            for (int counter = 0; counter < priceTexts.Count; counter++)
            {
                priceTexts[counter].position = new Vector2(counter % margin * (Item.size + spacing) + Item.size / 2, counter / margin * (Item.size + spacing) + Item.size / 2);
            }
        }

        public void SubPrice(Text text)
        {
            priceTexts.Remove(text);
            text = null;
        }

        public void SubPrice(WindowItem windowItem)
        {
            int priceID = -1;
                for (int counter = 0; counter < window.itemsList.Count; counter++)
                {
                    if (window.itemsList[counter].Equals(windowItem))
                    {
                        priceID = counter;
                        break;
                    }
                }
                SubPrice(priceTexts[priceID]);
        }

        protected override void HandleItemChoice()
        {
            Item currentItem = pack.items[selector.currentTargetNum];
            if (sellInventory)
            {
                if (player.gold + currentItem.price / 2 <= player.maxGold)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                    pack.SubItem(currentItem);
                    merchant.pack.AddItem(ItemCollection.CopyItem(currentItem));
                    player.gold += getPrice(currentItem.price);
                    SortPriceTexts();
                }
                else
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                    priceTexts[selector.currentTargetNum].ChangeColorEffect(Color.Red, 1000f);
                }
            }
            else
            {
                if (player.gold - currentItem.price > 0)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                    pack.SubItem(currentItem);
                    player.pack.AddItem(ItemCollection.CopyItem(currentItem));
                    player.gold -= currentItem.price;
                    SortPriceTexts();
                }
                else
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                    priceTexts[selector.currentTargetNum].ChangeColorEffect(Color.Red, 1000f);
                }
            }

        }

        protected override void UpdateShopInventory()
        {
            foreach(Text price in priceTexts)
            {
                price.Update(null);
            }
        }

        protected override void DrawShopInventory(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            foreach(Text price in priceTexts)
            {
                price.Draw(spriteBatch, offsetRect);
            }
        }
    }
}
