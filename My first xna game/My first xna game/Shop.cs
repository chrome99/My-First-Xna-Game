using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    public class Shop// : Inventory todo: private
    {
        public bool alive { get; private set; }
        private ShopInventory buyInventory;
        private ShopInventory sellInventory;
        public Choice choice;
        private Text buyText;
        private Text sellText;
        private Text tallkText;
        private Text exitText;
        private Player player;
        public Actor merchant; // todo: private
        private bool confirmKeyReleased = false;

        public Shop() { }

        public Shop(Map map, Player player, Actor merchant)
        {
            this.player = player;
            this.merchant = merchant;

            //intialize Buy Inventory
            buyInventory = new ShopInventory(map, player, merchant, false);
            buyInventory.Kill();
            buyInventory.SetWindowPosition(Inventory.Side.up);

            //intialize Sell Inventory
            sellInventory = new ShopInventory(map, player, merchant, true);
            sellInventory.Kill();
            sellInventory.SetWindowPosition(Inventory.Side.up);

            //intialize Choice
            buyText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Buy", null, new Vector2(2, 5));
            sellText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Sell", null, new Vector2(2, 5));
            exitText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Talk", null, new Vector2(2, 5));
            tallkText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Exit", null, new Vector2(2, 5));
            choice = new Choice(map, merchant.bounds, player, new List<WindowItem> { buyText, sellText, tallkText, exitText }, Choice.Arrangement.square, true);
        }

        public void setPlayerWindowPosition()
        {
            Kill();
        }

        public void Revive()
        {
            alive = true;
        }

        public void Kill()
        {
            PlayerManager.playersOnShop.Remove(player);
            alive = false;
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }
            choice.Update(newState, oldState, gameTime);
            buyInventory.Update(newState, oldState, gameTime);
            sellInventory.Update(newState, oldState, gameTime);

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (!choice.alive) { return; }
            if (newState.IsKeyDown(player.kbKeys.attack) && confirmKeyReleased)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                switch (choice.currentTargetNum)
                {
                    case 0: //Buy
                        choice.alive = false;
                        buyInventory.Revive();
                        break;

                    case 1: //Sel
                        choice.alive = false;
                        sellInventory.Revive();
                        break;

                    case 2: //Exit

                        Kill();
                        player.holdUpdateInput = true;
                        break;

                    case 3: //Talk
                        choice.alive = false;
                        player.MessageWindow(merchant, "I've got some fine merchandise today.", true, false, ReturnToMenu);
                        break;
                }
                confirmKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack))
            {
                confirmKeyReleased = true;
            }
        }
        public void ReturnToMenu()
        {
            choice.alive = true;
        }

        /*public void UpdateShopItems(int itemID, Player.PauseState sellOrBuy)
        {
            if (buyInventory.alive)
            {
                if (sellOrBuy == Player.PauseState.buyInventory)
                {
                    //do the same when both buying
                    merchant.pack.SubItem(merchant.pack.items[itemID], false);
                }
                if (sellOrBuy == Player.PauseState.sellInventory)
                {
                    //when i am buying and he is selling do the oppsite
                    merchant.pack.AddItem(merchant.pack.items[itemID]);//TODO: ADDING WONT WORK UNTIL YOU SET ANIMATION IN THE FUNCTION ADDITEM
                }
            }

        }*/

        public void HandleMenuButtonPress()
        {
            if (player.msgAlive)
            {
                return;
            }
            Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
            if (buyInventory.alive)
            {
                buyInventory.Kill();
                choice.alive = true;
                return;
            }
            if (sellInventory.alive)
            {
                sellInventory.Kill();
                choice.alive = true;
                return;
            }
            Kill();
        }

        public Vector2 getMerchantPosition()
        {
            return merchant.position;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }
            choice.Draw(spriteBatch, offsetRect);
            buyInventory.Draw(spriteBatch, offsetRect);
            sellInventory.Draw(spriteBatch, offsetRect);
        }

    }
}
