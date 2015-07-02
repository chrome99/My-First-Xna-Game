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
        private Textbox textBox;
        Player player;
        private List<string> commandsHistory = new List<string>();
        private int toggleCommandsHistory = -1;
        private int maxHistory = 5;

        private bool enterKeyReleased = false;
        private bool backKeyReleased = false;
        private bool upKeyReleased = false;
        private bool downKeyReleased = false;

        public CommandLine(Player player)
        {
            this.player = player;
            textBox = new Textbox(null, player, new Vector2(300, 0), new Vector2(350, 50));
        }

        public void Update(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (!alive) { return; }
            textBox.UpdateTextbox(gameTime, newState, oldState);
            textBox.text.UpdateTextChangeColor();

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            //command line
            if (newState.IsKeyDown(Keys.Enter) && enterKeyReleased)
            {
                commandsHistory.Add(textBox.input);
                Lua state = new Lua();

                try
                {
                    state["player"] = player;
                    state.DoString(textBox.input);
                    player = (Player)state["player"];

                    textBox.Reset();
                }
                catch (NLua.Exceptions.LuaException e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                    textBox.text.ChangeColorEffect(Color.Red, 1000f);
                }

                enterKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Enter))
            {
                enterKeyReleased = true;
            }

            if (newState.IsKeyDown(Keys.Back) && backKeyReleased)
            {
                if (textBox.input.Length != 0)
                {
                    textBox.input = textBox.input.Remove(textBox.input.Length - 1);
                }
                backKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Back))
            {
                backKeyReleased = true;
            }

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
                        textBox.input = commandsHistory[commandsHistory.Count - 1 - toggleCommandsHistory];
                    }
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
                        textBox.input = commandsHistory[0];
                        toggleCommandsHistory = commandsHistory.Count - 1;
                    }
                    else
                    {
                        textBox.input = commandsHistory[commandsHistory.Count - 1 - toggleCommandsHistory];
                    }
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