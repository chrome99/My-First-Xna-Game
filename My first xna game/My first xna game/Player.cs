using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    [Serializable]
    public class Player : Hostile
    {
        public PlayerKeys kbKeys;

        public int gold;
        public int maxGold = 100;

        public int maxPackWeight = 30;
        public bool maxWeightReached = false;

        [XmlIgnore]
        private Shop shop;
        [XmlIgnore]
        private Menu menu;
        [XmlIgnore]
        private Message msg;
        [XmlIgnore]
        private DebugHUD debug;
        [XmlIgnore]
        private HostileHUD hud;

        private bool playerMoving = false;
        private bool playerRunning = false;
        private int releasedKeysCount;

        private bool attackKeyReleased = false;

        private bool jumpKeyReleased = false;
        private Vector2 originalJumpingPosition;
        private bool jumping = false;
        private int maxJumpingWidth
        {
            get { return 32 + bounds.Width; }
        }
        private int maxJumpingHeight
        {
            get { return 32 + bounds.Height; }
        }
        private int jumpingCounter = 0;
        private bool maxJumpingHeightReached = false;


        private bool defendKeyReleased = false;
        [XmlIgnore]
        public Timer defendingTimer;
        private int defendingTime = 200;
        [XmlIgnore]
        public Timer defendingCooldownTimer;
        private int defendingCoolddownTime = 500;

        private bool menuKeyReleased = false;

        public Map map;
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
            public Keys jump;
            public Keys defend;
            public Keys run;
            public Keys opMenu;
            public Keys opDebug;
        }

        public bool msgAlive
        {
            get { return msg.alive; }
        }

        public Player() { }

        public Player(Texture2D texture, Vector2 position, PlayerKeys keys, Stats stats)
            : base(texture, position, MovementManager.Auto.off)
        {
            this.kbKeys = keys;
            this.stats = stats;

            pack = new Pack(this);
            debug = new DebugHUD(Game.content.Load<SpriteFont>("Fonts\\Debug1"), Color.Wheat, this, keys.opDebug);
            hud = new HostileHUD(this);
            shop = new Shop();

            defendingTimer = new Timer(defendingTime, false);
            defendingCooldownTimer = new Timer(defendingCoolddownTime, true);
        }

        public void IntializeMapParamters(Map map)
        {
            this.map = map;
            menu = new Menu(map, this);
            msg = new Message(map, this);
        }

        public void UpdateMapParameters(KeyboardState newState, KeyboardState oldState)
        {
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

        public void HandleHit(int damage)
        {
            if (shop.alive)
            {
                shop.setPlayerWindowPosition();
            }
            if (menu.alive)
            {
                menu.setPlayerWindowPosition();
            }
            if (msg.alive)
            {
                msg.setPlayerWindowPosition();
            }
            UpdatePlayerHUD(damage);
        }

        public void MessageWindow(GameObject gameObject, List<string> dialog, bool canIgnoreMsg, bool notFromShop = true, Message.ReturnFunction returnFunction = null)
        {
            msg.CreateDialog(gameObject, dialog, canIgnoreMsg, notFromShop, returnFunction);
        }

        public void MessageWindow(GameObject gameObject, string text, bool canIgnoreMsg, bool notFromShop = true, Message.ReturnFunction returnFunction = null)
        {
            msg.CreateDialog(gameObject, text, canIgnoreMsg, notFromShop, returnFunction);
        }

        public bool Shop(Actor merchant)
        {
            if (shop.alive) { return false; }
            if (PlayerManager.TwoPlayersSameShop(this, merchant.position)) { return false; }

            Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
            shop = new Shop(map, this, merchant);
            shop.Revive();
            return true;
        }

        public void UpdatePlayerHUD(int damage)
        {
            hud.UpdateHearts(damage);
        }

        protected override void UpdatePlayer(GameTime gameTime)
        {
            //TODO: is this being updated the number of players?
            if (jumping)
            {
                UpdateJump();
            }
            UpdateDefend();


            shop.Update(newState, oldState, gameTime);
            menu.Update(newState, oldState, gameTime);
            debug.Update();
            debug.UpdateInput(newState, oldState);
            msg.Update(gameTime, newState, oldState);

            UpdateInput();
        }

        private void UpdateJump()
        {
            if (maxJumpingHeightReached)
            {
                if (jumpingCounter == 0)
                {
                    canCollide = true;
                    passable = false;
                    maxJumpingHeightReached = false;
                    jumping = false;
                }
                else
                {
                    position.Y += Tile.size / 8;
                    switch (direction)
                    {
                        case MovementManager.Direction.up:
                            position.Y -= maxJumpingHeight / 16;
                            break;

                        case MovementManager.Direction.down:
                            position.Y += maxJumpingHeight / 16;
                            break;

                        case MovementManager.Direction.right:
                            position.X += maxJumpingWidth / 16;
                            break;

                        case MovementManager.Direction.left:
                            position.X -= maxJumpingWidth / 16;
                            break;
                    }
                    jumpingCounter--;
                }
            }
            else
            {
                if (jumpingCounter == 8)
                {
                    maxJumpingHeightReached = true;
                }
                else
                {
                    position.Y -= Tile.size / 8;
                    switch (direction)
                    {
                        case MovementManager.Direction.up:
                            position.Y -= maxJumpingHeight / 16;
                            break;

                        case MovementManager.Direction.down:
                            position.Y += maxJumpingHeight / 16;
                            break;

                        case MovementManager.Direction.right:
                            position.X += maxJumpingWidth / 16;
                            break;

                        case MovementManager.Direction.left:
                            position.X -= maxJumpingWidth / 16;
                            break;
                    }
                    jumpingCounter++;
                }
            }
        }

        private void UpdateDefend()
        {
            if (defendingTimer.result)
            {
                //active cooldown
                defendingCooldownTimer.Reset();
                defendingTimer.Reset(false);
            }
        }

        protected void UpdateInput()
        {
            //cancal things, and open menu when can
            if (newState.IsKeyDown(kbKeys.opMenu) && menuKeyReleased)
            {
                if (msg.alive)
                {
                    if (!msg.HandleMenuButtonPress())
                    {
                        return;
                    }
                }
                else if (shop.alive)
                {
                    shop.HandleMenuButtonPress();
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

            if (shop.alive || msg.alive || menu.alive)
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

            //if pressed attack
            if (newState.IsKeyDown(kbKeys.attack) && attackKeyReleased)
            {
                for (int counter = 0; counter < equipmentList.Count; counter++)
                {
                    Bow weapon = equipmentList[counter] as Bow;
                    if (weapon != null)
                    {
                        weapon.Attack(map, this);
                    }
                }

                attackKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.attack))
            {
                attackKeyReleased = true;
            }

            if (jumping || (defendingTimer.Counting)) { return; }

            //jump
            if (newState.IsKeyDown(kbKeys.jump) && jumpKeyReleased)
            {
                if (!jumping)
                {
                    int VectorMoveTo = 0;
                    if (direction == MovementManager.Direction.down || direction == MovementManager.Direction.up)
                    {
                        VectorMoveTo = maxJumpingHeight;
                    }
                    if (direction == MovementManager.Direction.right || direction == MovementManager.Direction.left)
                    {
                        VectorMoveTo = maxJumpingWidth;
                    }
                    Rectangle collisionRect = core;
                    Vector2 collisionVector = MovementManager.MoveVector(position, VectorMoveTo, direction);
                    collisionRect.X = (int)collisionVector.X;
                    collisionRect.Y = (int)collisionVector.Y;
                    if (!movementManager.CollisionCheck(this, collisionRect))
                    {
                        Game.content.Load<SoundEffect>("Audio\\Waves\\fart").Play();
                        originalJumpingPosition = position;
                        canCollide = false;
                        passable = true;
                        jumping = true;
                        jumpKeyReleased = false;
                        return;
                    }
                }

            }
            else if (!oldState.IsKeyDown(kbKeys.jump))
            {
                jumpKeyReleased = true;
            }

            //defend
            if (newState.IsKeyDown(kbKeys.defend) && defendKeyReleased)
            {
                if (defendingCooldownTimer.result)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\fart").Play();
                    defendingTimer.Active();
                    defendingTimer.Reset();
                    defendKeyReleased = false;
                }
            }
            else if (!oldState.IsKeyDown(kbKeys.defend))
            {
                defendKeyReleased = true;
            }

            //movement

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
                movingState = MovementManager.MovingState.walking; //srsly
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

        public void DrawPlayerItems(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            shop.Draw(spriteBatch, offsetRect);
            menu.Draw(spriteBatch, offsetRect);
            msg.Draw(spriteBatch, offsetRect);
            hud.Draw(spriteBatch);
            debug.Draw(spriteBatch);
        }
    }
}