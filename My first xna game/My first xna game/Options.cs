using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Options
    {
        public bool alive = false;
        private Window window;
        private Selector selector;
        private Player player;
        private SpriteFont font;

        private Textbox resolutionTextBoxWidth;
        private Textbox resolutionTextBoxHeight;
        private int resolutionWidthSave;

        public Options(Map map, Player player)
        {
            this.player = player;

            font = Game.content.Load<SpriteFont>("Fonts\\medival1");
            window = new Window(map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), new Vector2(0, 0), 700, 700);

            int textLayout = 50;

            Text resolutionText = new Text(font, new Vector2(textLayout, 100), Color.Black, "Resolution", window);
            resolutionTextBoxWidth = new Textbox(window, player, new Vector2(500, 100), font.MeasureString("8888"), HandleResolutionWidthInput);
            resolutionTextBoxWidth.Active = false;
            resolutionTextBoxHeight = new Textbox(window, player, new Vector2(700, 100), font.MeasureString("8888"), HandleResolutionHeightInput);
            resolutionTextBoxHeight.Active = false;


            Vector2 biggestItemSize = new Vector2(window.itemsList[0].bounds.Width, window.itemsList[0].bounds.Height);
            foreach(WindowItem item in window.itemsList)
            {
                if (item.bounds.Width > biggestItemSize.X)
                {
                    biggestItemSize.X = item.bounds.Width;
                }
                if (item.bounds.Height > biggestItemSize.Y)
                {
                    biggestItemSize.Y = item.bounds.Height;
                }
            }

            selector = new Selector(player, new List<WindowItem>() { resolutionText }, HandleItemChoice, biggestItemSize, 0, 1, window);
        }

        public void Kill()
        {
            alive = false;
        }

        public void Revive()
        {
            selector.confirmKeyReleased = false;
            selector.currentTargetNum = 0;
            alive = true;
        }

        private void HandleItemChoice()
        {
            switch (selector.currentTargetNum)
            {
                case 0: //resolution
                    selector.active = false;
                    resolutionTextBoxWidth.Active = true;
                    break;
            }
        }

        private void HandleResolutionWidthInput(string input)
        {
            if (Int32.TryParse(input, out resolutionWidthSave))
            {
                resolutionTextBoxWidth.Active = false;
                resolutionTextBoxHeight.Active = true;
            }
            else
            {
                resolutionTextBoxWidth.input.ChangeColorEffect(Color.Red, 1000f);
            }
        }

        private void HandleResolutionHeightInput(string input)
        {
            int resolutionHeight;
            if (Int32.TryParse(input, out resolutionHeight))
            {
                Game.graphics.PreferredBackBufferWidth = resolutionWidthSave;
                Game.graphics.PreferredBackBufferHeight = resolutionHeight;
                Game.graphics.ApplyChanges();
                resolutionTextBoxHeight.Active = false;
                selector.active = true;
            }
            else
            {
                resolutionTextBoxHeight.input.ChangeColorEffect(Color.Red, 1000f);
            }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }

            window.Update(gameTime);
            window.UpdateSelectorAndTextBox(newState, oldState, gameTime);
            resolutionTextBoxWidth.input.UpdateTextChangeColor();
            resolutionTextBoxHeight.input.UpdateTextChangeColor();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!alive) { return; }

            window.Draw(spriteBatch, new Rectangle());
        }
    }
}
