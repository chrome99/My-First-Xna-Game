using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Choice
    {
        public enum Arrangement { line, column, square }
        private List<WindowItem> optionsList;
        public bool alive = true;
        private Player player;
        private Selector selector;
        public Window window;
        private bool confirmKeyReleased = false;
        private Vector2 biggestOptionSize = Vector2.Zero;
        private double optionsRoot;
        private Vector2 windowSize;
        private Vector2 layout;

        public Choice(Player player, List<WindowItem> optionsList, Arrangement arrangement = Arrangement.square)
        {
            //intialize variables
            this.optionsList = optionsList;
            this.player = player;
            optionsRoot = Math.Sqrt(optionsList.Count + 1);
            foreach(WindowItem option in optionsList) 
            {
                //if biggestOptionSize.X is smaller than option.X - make it the size option.X
                if (biggestOptionSize.X < option.bounds.Width)
                {
                    biggestOptionSize.X = option.bounds.Width;
                }

                //if biggestOptionSize.Y is smaller than option.Y - make it the size option.Y
                if (biggestOptionSize.Y < option.bounds.Height)
                {
                    biggestOptionSize.Y = option.bounds.Height;
                }
            }
            layout = biggestOptionSize / 4;
            biggestOptionSize += layout;

            //arrange options
            switch (arrangement)
            {
                case Arrangement.line:
                    //arrange window items
                    for (int counter = 0; counter < optionsList.Count; counter++)
                    {
                        optionsList[counter].position = new Vector2(counter * biggestOptionSize.X, 0);
                    }

                    //set window size

                    break;

                case Arrangement.column:
                    //arrange window items
                    for (int counter = 0; counter < optionsList.Count; counter++)
                    {
                        optionsList[counter].position = new Vector2(0, counter * biggestOptionSize.Y);
                    }

                    //set window size
                    windowSize = new Vector2(biggestOptionSize.X * optionsList.Count, biggestOptionSize.Y * optionsList.Count);
                    break;

                case Arrangement.square:
                    //arrange window items
                    if (optionsRoot % 1 != 0)
                    {
                        for (int counter = 0; counter < optionsList.Count; counter++)
                        {
                            optionsList[counter].position = new Vector2(counter % (int)optionsRoot * biggestOptionSize.X, counter / (int)optionsRoot * biggestOptionSize.Y);
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("Parameter dosen't have perfect square", "original");
                    }

                    //set window size
                    windowSize = new Vector2(biggestOptionSize.X * (int)optionsRoot, biggestOptionSize.Y * (int)optionsRoot);
                    break;

            }

            //create window and selector
            window = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, (int)windowSize.X, (int)windowSize.Y, player);
            selector = new Selector(window, optionsList, biggestOptionSize, 0);
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
                option.Draw(spriteBatch, new Rectangle(), screenRect);
            }
            selector.Draw(spriteBatch, new Rectangle(), screenRect);
            window.Draw(spriteBatch, new Rectangle(), screenRect);
        }
    }
}
