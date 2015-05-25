using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace My_first_xna_game
{
    public class Player : Hostile
    {
        public PlayerKeys kbKeys;

        public int gold;
        public int maxGold = 100;

        private Shop shop;

        private Menu menu;

        private DebugHUD debug;

        private Window msgWindow;
        private Text msgWindowCurrentText;
        private List<string> msgWindowDialog = new List<string>();
        private int lastTextIndex;
        private bool canIgnoreMsg;

        private bool playerMoving = false;
        private bool playerRunning = false;
        private int releasedKeysCount;
        private bool fireballkeyReleased = false;
        private bool useKeyReleased = false;
        private bool menuKeyReleased = false;
        private Map map;
        private KeyboardState newState;
        private KeyboardState oldState;
        public List<int> collisionsList = new List<int>();

        public struct PlayerKeys
        {
            public Keys mvUp;
            public Keys mvDown;
            public Keys mvLeft;
            public Keys mvRight;
            public Keys attack;
            public Keys run;
            public Keys opMenu;
            public Keys opDebug;
        }

        /*public enum PauseState { game, inventory, shop, buyInventory, sellInventory, msgWindow }
        public PauseState pauseState
        {
            get
            {
                if (inventory.alive)
                {
                    return PauseState.inventory;
                }
                if (shop.alive)
                {
                    return PauseState.shop;
                }
                if (msgWindow.alive)
                {
                    return PauseState.msgWindow;
                }
                else
                {
                    return PauseState.game;
                }
            }
        }*/

        public Player(Texture2D texture, Vector2 position, PlayerKeys keys)
            : base(texture, position, MovementManager.Auto.off)
        {
            this.kbKeys = keys;

            pack = new Pack(this);
            debug = new DebugHUD(Game.content.Load<SpriteFont>("Debug1"), Color.Wheat, this, keys.opDebug);
            menu = new Menu(this);
            msgWindow = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, 0, 0, null);
            msgWindowCurrentText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, null);
            msgWindow.Kill();

            shop = new Shop();
        }

        public void UpdateMapParameters(Map map, KeyboardState newState, KeyboardState oldState)
        {
            this.map = map;
            this.newState = newState;
            this.oldState = oldState;
        }

        public void FlipRunning()
        {
            if (enableRunning)
            {
                enableRunning = false;
            }
            else
            {
                enableRunning = true;
            }
        }

        public void MessageWindow(Rectangle position, List<string> dialog, bool canIgnoreMsg)
        {
            this.canIgnoreMsg = canIgnoreMsg;
            msgWindowDialog = dialog;
            msgWindowCurrentText.text = dialog[0];
            lastTextIndex = 0;
            Vector2 windowSize = new Vector2(msgWindowCurrentText.bounds.Width + 10, msgWindowCurrentText.bounds.Height + 10);

            msgWindow = new Window(msgWindow.texture, Vector2.Zero, (int)windowSize.X, (int)windowSize.Y, this);
            msgWindow.SetWindowAbove(bounds);
            msgWindowCurrentText.position += Vector2.Zero;
            msgWindow.thickness = new Vector2(10f, 0f);
            msgWindow.AddItem(msgWindowCurrentText);

            msgWindow.Revive();
        }

        public void Shop(Actor merchant)
        {
            if (shop.alive) { return; }
            shop = new Shop(this, merchant);
            shop.alive = true;
        }

        protected override void UpdatePlayer(GameTime gameTime)
        {
            //TODO: is this being updated the number of players?
            shop.Update(newState, oldState, gameTime);
            menu.Update(newState, oldState, gameTime);
            debug.Update();
            debug.UpdateInput(newState, oldState);
            msgWindow.Update(gameTime);

            UpdateInput();
        }

        protected void UpdateInput()
        {
            //cancal things, and open menu when can
            if (newState.IsKeyDown(kbKeys.opMenu) && menuKeyReleased)
            {
                if (shop.alive)
                {
                    shop.HandleMenuButtonPress();
                }
                else if (msgWindow.alive )
                {
                    if (canIgnoreMsg)
                    {
                        msgWindow.alive = false;
                    }
                    else { return; }
                }
                else
                {
                    menu.HandleMenuButtonPress();
                }

                menuKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.opMenu))
            {
                menuKeyReleased = true;
            }

            if (msgWindow.alive)
            {
                if (newState.IsKeyDown(kbKeys.attack) && useKeyReleased)
                {
                    lastTextIndex++;
                    if (msgWindowDialog.Count == lastTextIndex)
                    {
                        msgWindow.alive = false;
                    }
                    else
                    {
                        Vector2 windowSize = new Vector2(msgWindowCurrentText.bounds.Width + 10, msgWindowCurrentText.bounds.Height + 10);
                        msgWindow.width = (int)windowSize.X;
                        msgWindow.height = (int)windowSize.Y;
                        msgWindowCurrentText.text = msgWindowDialog[lastTextIndex];
                    }

                    useKeyReleased = false;
                }
                else if (!oldState.IsKeyDown(kbKeys.attack))
                {
                    useKeyReleased = true;
                }
            }

            if (shop.alive || msgWindow.alive || menu.alive)
            {
                return;
            }

            // -Update player speed state
            if (newState.IsKeyDown(kbKeys.run) && enableRunning)
            {
                playerRunning = true;
            }
            else // reset player speed speed state
            {
                playerRunning = false;
            }

            //if perssed attack
            if (newState.IsKeyDown(kbKeys.attack) && fireballkeyReleased)
            {
                Projectile projectile = new Projectile(map, Game.content.Load<Texture2D>("wolf"), 6, this, 60);
                projectile.passable = true;

                fireballkeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.attack))
            {
                fireballkeyReleased = true;
            }

            releasedKeysCount = 0;

            // if pressed left
            if (newState.IsKeyDown(kbKeys.mvLeft))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.left, (int)speed);
                
            }
            else
            {
                releasedKeysCount++;
            }

            // if pressed right
            if (newState.IsKeyDown(kbKeys.mvRight))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.right, (int)speed);
            }
            else
            {
                releasedKeysCount++;
            }

            // if pressed up
            if (newState.IsKeyDown(kbKeys.mvUp))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.up, (int)speed);
            }
            else
            {
                releasedKeysCount++;
            }

            // if pressed down
            if (newState.IsKeyDown(kbKeys.mvDown))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.down, (int)speed);
            }
            else
            {
                releasedKeysCount++;
            }

            //if all four arrows relesed
            if (releasedKeysCount >= 4)
            {
                playerMoving = false;
            }

            //-Update player movement status

            if (playerMoving && playerRunning)
            {
                movingState = MovementManager.MovingState.running;

            }
            else if (playerMoving)
            {
                movingState = MovementManager.MovingState.walking;
            }
            else
            {
                movingState = MovementManager.MovingState.standing;
            }
        }

        public Vector2 getMerchantPosition()
        {
            return shop.getMerchantPosition();
        }

        public void DrawPlayerItems(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            shop.Draw(spriteBatch, offsetRect, screenPosition);
            debug.Draw(spriteBatch, screenPosition);
            menu.Draw(spriteBatch, offsetRect, screenPosition);
            msgWindow.Draw(spriteBatch, offsetRect, screenPosition);
        }
    }
}
