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
        private Inventory inventory;
        private Shop shop;
        private DebugHUD debug;
        private Window msgWindow;
        private Text msgWindowText;
        private bool playerMoving = false;
        private bool playerRunning = false;
        private int releasedKeysCount;
        private bool fireballkeyReleased = false;
        private bool menuKeyReleased = false;
        private Map map;
        private KeyboardState newState;
        private KeyboardState oldState;

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

        public Player(Texture2D texture, Vector2 position, PlayerKeys keys)
            : base(texture, position, MovementManager.Auto.off)
        {
            this.kbKeys = keys;

            pack = new Pack();
            inventory = new Inventory(this, false);
            debug = new DebugHUD(Game.content.Load<SpriteFont>("Debug1"), Color.Wheat, this, keys.opDebug);

            msgWindow = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, 0, 0, null);
            msgWindowText = new Text(Game.content.Load<SpriteFont>("medival1"), Vector2.Zero, Color.White, null);
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

        public void MessageWindow(Rectangle position, string text)
        {
            msgWindowText.text = text;
            Vector2 windowSize = new Vector2(msgWindowText.bounds.Width + 10, msgWindowText.bounds.Height + 10);

            //intialize new position
            Vector2 newPosition;

            //set center
            newPosition.X = position.X + position.Width / 2 - windowSize.X / 2;
            newPosition.Y = position.Y + position.Height / 2 - windowSize.Y / 2;

            //set above
            newPosition.Y = newPosition.Y - position.Height / 2 - windowSize.Y / 2 - 10;

            msgWindow = new Window(msgWindow.texture, newPosition, (int)windowSize.X, (int)windowSize.Y, null);
            msgWindow.opacity = 75;
            msgWindowText.position += Vector2.Zero;
            msgWindow.thickness = new Vector2(10f, 0f);
            msgWindow.AddItem(msgWindowText);

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
            inventory.Update(newState, oldState, gameTime);
            shop.Update(newState, oldState, gameTime);
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
                    if (shop.buyInventory.alive)
                    {
                        shop.buyInventory.alive = false;
                        shop.choice.alive = true;
                        menuKeyReleased = false;
                        return;
                    }
                    if (shop.sellInventory.alive)
                    {
                        shop.sellInventory.alive = false;
                        shop.choice.alive = true;
                        menuKeyReleased = false;
                        return;
                    }
                    shop.alive = false;
                }
                else if (msgWindow.alive)
                {
                    msgWindow.alive = false;
                }
                else if (inventory.alive)
                {
                    inventory.Kill();
                }
                else
                {
                    inventory.Revive();
                }

                menuKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.opMenu))
            {
                menuKeyReleased = true;
            }
            if (inventory.alive || shop.alive || msgWindow.alive)
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

            // -Update player through debug button
            if (newState.IsKeyDown(Keys.LeftControl))
            {
                passable = true;
            }
            else // reset player speed speed state
            {
                passable = false;
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

        public void DrawPlayerItems(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            inventory.Draw(spriteBatch, offsetRect, screenPosition);
            shop.Draw(spriteBatch, offsetRect, screenPosition);
            debug.Draw(spriteBatch, screenPosition);
            msgWindow.Draw(spriteBatch, offsetRect, screenPosition);
        }
    }
}
