using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLua;
using System.IO;

namespace My_first_xna_game
{
    class CommandLine
    {
        public bool alive = false;
        private Player player;
        private Textbox textBox;

        private List<string> commandsHistory = new List<string>();
        private int toggleCommandsHistory = -1;
        private int maxHistory = 5;

        private bool upKeyReleased = false;
        private bool downKeyReleased = false;

        public CommandLine(Player player)
        {
            this.player = player;
            textBox = new Textbox(null, player, new Vector2(300, 0), new Vector2(350, 50), HandleText);
        }

        private void HandleText(string input)
        {
            commandsHistory.Add(textBox.InputString);
            Lua state = new Lua();

            try
            {
                state["player"] = player;
                state.DoString(textBox.InputString);
                player = (Player)state["player"];

                textBox.Reset();
            }
            catch (NLua.Exceptions.LuaException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                textBox.input.ChangeColorEffect(Color.Red, 1000f);
            }
        }

        public void Update(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (!alive) { return; }
            textBox.UpdateTextbox(gameTime, newState, oldState);
            textBox.input.UpdateTextChangeColor();

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {

            if (newState.IsKeyDown(Keys.Up) && upKeyReleased)
            {
                if (commandsHistory.Count == 0)
                {
                    //todo play sound
                }
                else
                {
                    toggleCommandsHistory++;
                    if (commandsHistory.Count <= toggleCommandsHistory)
                    {
                        textBox.Reset();
                        toggleCommandsHistory = -1;
                    }
                    else
                    {
                        textBox.InputString = commandsHistory[commandsHistory.Count - 1 - toggleCommandsHistory];
                    }
                    textBox.ResetCursorPosition();
                }
                upKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Up))
            {
                upKeyReleased = true;
            }

            if (newState.IsKeyDown(Keys.Down) && downKeyReleased)
            {
                if (commandsHistory.Count == 0)
                {
                    //todo play sound
                }
                else
                {
                    toggleCommandsHistory--;
                    if (toggleCommandsHistory == -1)
                    {
                        textBox.Reset();
                        toggleCommandsHistory = -1;
                    }
                    else if (toggleCommandsHistory == -2)
                    {
                        textBox.InputString = commandsHistory[0];
                        toggleCommandsHistory = commandsHistory.Count - 1;
                    }
                    else
                    {
                        textBox.InputString = commandsHistory[commandsHistory.Count - 1 - toggleCommandsHistory];
                    }
                    textBox.ResetCursorPosition();
                }
                downKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Down))
            {
                downKeyReleased = true;
            }


            if (commandsHistory.Count > maxHistory)
            {
                commandsHistory.Remove(commandsHistory[0]);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!alive) { return; }
            textBox.DrawWithoutSource(spriteBatch, new Rectangle());
        }
    }
}