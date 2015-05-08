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
        public Selector selector;
        public Window window;
        private bool confirmKeyReleased = false;
        private Vector2 biggestOptionSize = Vector2.Zero;
        private double optionsRoot;
        private Vector2 windowSize;
        private Vector2 spacing;
        private int newRow;

        public Choice(Rectangle sourcePosition, Player player, List<WindowItem> optionsList, Arrangement arrangement = Arrangement.square)
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
            spacing = biggestOptionSize / 4;

            //arrange options
            switch (arrangement)
            {
                case Arrangement.line:
                    //arrange window items
                    for (int counter = 0; counter < optionsList.Count; counter++)
                    {
                        optionsList[counter].position = new Vector2(counter * biggestOptionSize.X + spacing.X, 0);
                    }

                    //set newRow parameter
                    newRow = 1;

                    //set window size

                    break;

                case Arrangement.column:
                    //arrange window items
                    for (int counter = 0; counter < optionsList.Count; counter++)
                    {
                        optionsList[counter].position = new Vector2(0, counter * (biggestOptionSize.Y + spacing.Y));
                    }

                    //set newRow parameter
                    newRow = 0;

                    //set window size
                    windowSize = new Vector2(biggestOptionSize.X * optionsList.Count, (biggestOptionSize.Y + spacing.Y) * optionsList.Count);
                    break;

                case Arrangement.square:
                    //arrange window items
                    if (optionsRoot % 1 != 0)
                    {
                        for (int counter = 0; counter < optionsList.Count; counter++)
                        {
                            optionsList[counter].position = new Vector2(counter % (int)optionsRoot * (biggestOptionSize.X + spacing.X), counter / (int)optionsRoot * (biggestOptionSize.Y + spacing.Y));
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("Parameter dosen't have perfect square", "original");
                    }

                    //set newRow parameter
                    newRow = (int)optionsRoot;

                    //set window size
                    windowSize = new Vector2((biggestOptionSize.X + spacing.X) * (int)optionsRoot, (biggestOptionSize.Y + spacing.Y) * (int)optionsRoot);
                    break;

            }

            //create window and selector
            window = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, (int)windowSize.X, (int)windowSize.Y, player);
            window.SetWindowAbove(sourcePosition);
            selector = new Selector(window, optionsList, biggestOptionSize, 0, newRow);
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
            if (newState.IsKeyDown(player.kbKeys.opMenu) && confirmKeyReleased)
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
            window.Draw(spriteBatch, offsetRect, screenRect);
        }
    }
}
