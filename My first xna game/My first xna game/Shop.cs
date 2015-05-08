using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    class Shop// : Inventory
    {
        public bool alive;
        public BuyInventory buyInventory;
        public SellInventory sellInventory;
        public Choice choice;
        private Text buyText;
        private Text sellText;
        private Text tallkText;
        private Text exitText;
        private Player player;
        private Actor merchant;
        private bool confirmKeyReleased = false;

        public Shop() { }

        public Shop(Player player, Actor merchant)
        {
            this.player = player;
            this.merchant = merchant;

            //intialize Buy Inventory
            buyInventory = new BuyInventory(player, merchant);
            buyInventory.alive = false;
            buyInventory.window.SetWindowAbove(merchant.bounds);

            //intialize Sell Inventory
            sellInventory = new SellInventory(player, merchant);
            sellInventory.alive = false;
            sellInventory.window.SetWindowAbove(player.bounds);

            //intialize Choice
            buyText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Buy");
            sellText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Sell");
            exitText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Talk");
            tallkText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Exit");
            choice = new Choice(merchant.bounds, player, new List<WindowItem> { buyText, sellText, tallkText, exitText }, Choice.Arrangement.square);
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
            if (newState.IsKeyDown(player.kbKeys.attack) && confirmKeyReleased)
            {
                switch(choice.selector.currentTargetNum)
                {
                    case 0: //Buy
                        choice.alive = false;
                        buyInventory.alive = true;
                        break;

                    case 1: //Sel
                        choice.alive = false;
                        sellInventory.alive = true;
                        break;

                    case 2: //Exit
                        alive = false;
                        break;

                    case 3: //Talk

                        break;
                }
                confirmKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack))
            {
                confirmKeyReleased = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            if (!alive) { return; }
            choice.Draw(spriteBatch, offsetRect, screenPosition);
            buyInventory.Draw(spriteBatch, offsetRect, screenPosition);
            sellInventory.Draw(spriteBatch, offsetRect, screenPosition);
        }

    }
}
