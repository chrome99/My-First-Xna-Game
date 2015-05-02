using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    class Shop : Inventory
    {
        private Actor merchant;
        private Text buyText;
        private Text sellText;
        private Text tallkText;
        private Text exitText;
        public Choice choice;
        public static string blabla;

        public Shop() : base(null, false) { alive = false; }
        public Shop(Player player, Actor merchant)
            : base(player, true)
        {
            this.merchant = merchant;

            window.visible = false;
            selector.visible = false;
            window.position = merchant.position;
            buyText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Buy");
            sellText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Sell");
            exitText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Talk");
            tallkText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, "Exit");
            choice = new Choice(player, new List<WindowItem> { buyText, sellText, tallkText, exitText }, Choice.Arrangement.square);
            //chouice should return some value to home function when the player chooses something
        }
        protected override void UpdateBuyInventory(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            choice.Update(newState, oldState, gameTime);
        }

        protected override void ChosenItemFunction()
        {
            //choice.alive = true;
        }

        protected override void DrawShop(SpriteBatch spriteBatch, Rectangle screenPosition)
        {
            choice.Draw(spriteBatch, new Rectangle(), screenPosition);
        }

    }
}
