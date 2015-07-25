using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Choice//todo: private
    {
        public enum Arrangement { line, column, square }
        public Selector.HandleItemChoice handleChoice;
        private List<WindowItem> optionsList;
        public bool alive = true;
        private Player player;
        private Selector selector;
        public Window window;
        private bool confirmKeyReleased = false;
        private Vector2 biggestOptionSize = Vector2.Zero;
        private double optionsRoot;
        private Vector2 windowSize;
        private Vector2 spacing;
        private int newRow;

        public bool Active
        {
            get { return selector.active; }
            set { selector.active = value; }
        }

        public bool SelectorVisible
        {
            get { return selector.visible; }
            set { selector.visible = value; }
        }

        public int currentTargetNum
        {
            get { return selector.currentTargetNum; }
            set { selector.currentTargetNum = value; }
        }

        public Choice(Map map, Rectangle sourcePosition, Player player, List<WindowItem> optionsList, Selector.HandleItemChoice handleChoice, Arrangement arrangement = Arrangement.square, bool playerCollision = false)
        {
            //intialize variables
            this.handleChoice = handleChoice;
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
                    newRow = optionsList.Count;

                    //set window size
                    windowSize = new Vector2((biggestOptionSize.X + spacing.X) * optionsList.Count, biggestOptionSize.Y + spacing.Y);
                    break;

                case Arrangement.column:
                    //arrange window items
                    for (int counter = 0; counter < optionsList.Count; counter++)
                    {
                        optionsList[counter].position = new Vector2(0, counter * (biggestOptionSize.Y + spacing.Y));
                    }

                    //set newRow parameter
                    newRow = 1;

                    //set window size
                    windowSize = new Vector2(biggestOptionSize.X + spacing.X, (biggestOptionSize.Y + spacing.Y) * optionsList.Count);
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
            Player windowPlayer;
            if (playerCollision)
            {
                windowPlayer = player;
            }
            else
            {
                windowPlayer = null;
            }
            window = new Window(map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), Vector2.Zero, (int)windowSize.X, (int)windowSize.Y, windowPlayer);
            window.SetWindowAbove(sourcePosition);

            selector = new Selector(player, optionsList, handleChoice, biggestOptionSize, 0, newRow, window);

            foreach (WindowItem option in this.optionsList)
            {
                window.AddItem(option);
            }
        }

        public void ResetSelector()
        {
            selector.Reset();
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }
            window.Update(gameTime);
            window.UpdateSelectorAndTextBox(newState, oldState, gameTime);
            

            if (selector.active)
            {
                UpdateInput(newState, oldState);
            }
        }

        protected void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(player.kbKeys.opMenu.key) && confirmKeyReleased)
            {
                alive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }

            window.Draw(spriteBatch, offsetRect);
        }
    }
}
