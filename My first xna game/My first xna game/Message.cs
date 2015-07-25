using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;


namespace My_first_xna_game
{
    public class Message
    {
        public delegate void ReturnFunction();
        private ReturnFunction returnFunction;
        private Window window;
        private Text currentText;
        private List<string> dialog;
        private int lastTextIndex;
        private bool canIgnoreMsg;
        private Player player;
        private GameObject source;

        private bool notFromShop = true;
        private bool useKeyReleased = false;

        public bool alive
        {
            get
            {
                return window.alive;
            }
            set
            {
                window.alive = value;
            }
        }

        public Message(Map map, Player player)
        {
            this.player = player;

            window = new Window(map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), Vector2.Zero, 0, 0, player);
            window.thickness = new Vector2(10f, 0f);

            currentText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, null, window, new Vector2(2, 5));

            window.Kill();
        }

        public void CreateDialog(GameObject source, List<string> dialog, bool canIgnoreMsg, bool notFromShop, ReturnFunction returnFunction)
        {
            //text
            currentText.text = dialog[0];
            this.dialog = dialog;

            createDialog(source, canIgnoreMsg, notFromShop, returnFunction);
        }

        public void CreateDialog(GameObject source, string text, bool canIgnoreMsg, bool notFromShop, ReturnFunction returnFunction)
        {
            //text
            currentText.text = text;
            dialog = null;

            createDialog(source, canIgnoreMsg, notFromShop, returnFunction);
        }

        private void createDialog(GameObject source, bool canIgnoreMsg, bool notFromShop, ReturnFunction returnFunction)
        {
            //variables
            this.source = source;
            this.canIgnoreMsg = canIgnoreMsg;
            this.notFromShop = notFromShop;
            this.returnFunction = returnFunction;
            lastTextIndex = 0;

            //window
            setWindowSize();
            window.SetWindowAbove(source.bounds);
            useKeyReleased = false;
            //currentText.position += Vector2.Zero;

            window.Revive();
        }

        public void setPlayerWindowPosition()
        {
            window.SetWindowAbove(source.bounds);
        }

        private void setWindowSize()
        {
            window.width = currentText.bounds.Width + 10;
            window.height = currentText.bounds.Height + 10;
        }

        public void Update(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (!alive) { return; }
            window.Update(gameTime);

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(player.kbKeys.attack.key) && useKeyReleased)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                if (dialog == null)
                {
                    window.alive = false;
                    if (returnFunction != null)
                    {
                        returnFunction();
                    }
                    player.holdUpdateInput = true;
                }
                else
                {
                    lastTextIndex++;
                    if (dialog.Count == lastTextIndex)
                    {
                        window.alive = false;
                        if (returnFunction != null)
                        {
                            returnFunction();
                        }
                        player.holdUpdateInput = true;
                    }
                    else
                    {
                        currentText.text = dialog[lastTextIndex];
                        setWindowSize();
                    }
                }

                useKeyReleased = false;

            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack.key) && notFromShop)
            {
                useKeyReleased = true;
            }

            if (!notFromShop)
            {
                notFromShop = true;
            }
        }

        public bool HandleMenuButtonPress()
        {
            if (canIgnoreMsg)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                window.alive = false;
                if (returnFunction != null)
                {
                    returnFunction();
                }
                return true;
            }
            else { return false; }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }
            window.Draw(spriteBatch, offsetRect);
        }
    }
}
