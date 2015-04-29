using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Choice
    {
        public List<WindowItem> optionsList;
        public bool alive = true;
        public Player player;
        private Selector selector;
        public Window window;
        private bool confirmKeyReleased;

        public Choice(Player player, List<WindowItem> optionsList)
        {
            this.optionsList = optionsList;

            window = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, 200, 200, player);
            selector = new Selector(window, optionsList, new Vector2(32f, 32f), 0, 0);
            selector.player = player;

            foreach (WindowItem option in this.optionsList)
            {
                window.AddItem(option);
            }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }
            window.Update(gameTime);
            selector.Update(newState, oldState, gameTime);

            if (selector.visible)
            {
                UpdateInput(newState, oldState);
            }
        }

        protected void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(player.keys.attack) && confirmKeyReleased)
            {
                
                confirmKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.keys.attack))
            {
                confirmKeyReleased = true;
            }

            if (newState.IsKeyDown(player.keys.menu) && confirmKeyReleased)
            {
                alive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenRect)
        {
            if (!alive) { return; }
            foreach(WindowItem option in optionsList)
            {
                option.Draw(spriteBatch, offsetRect, screenRect);
            }
            selector.Draw(spriteBatch, offsetRect, screenRect);
        }
    }
}
