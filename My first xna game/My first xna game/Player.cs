using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Player : Hostile
    {
        public PlayerKeys kbKeys;

        public int gold;
        public int maxGold = 100;

        public int maxPackWeight = 30;
        public bool maxWeightReached = false;

        private Shop shop;
        private Menu menu;
        private Message msg;
        private DebugHUD debug;
        private HostileHUD hud;

        private bool playerMoving = false;
        private bool playerRunning = false;
        private int releasedKeysCount;

        private bool fireballkeyReleased = false;
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

        public bool msgAlive
        {
            get { return msg.alive; }
        }

        public Player(Map map, Texture2D texture, Vector2 position, PlayerKeys keys, Stats stats)
            : base(texture, position, MovementManager.Auto.off)
        {
            this.kbKeys = keys;
            this.stats = stats;

            pack = new Pack(this);
            debug = new DebugHUD(Game.content.Load<SpriteFont>("Fonts\\Debug1"), Color.Wheat, this, keys.opDebug);
            menu = new Menu(map, this);
            msg = new Message(map, this);
            hud = new HostileHUD(this);

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
            shop.Update(newState, oldState, gameTime);
            menu.Update(newState, oldState, gameTime);
            debug.Update();
            debug.UpdateInput(newState, oldState);
            msg.Update(gameTime, newState, oldState);

            UpdateInput();
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
            if (newState.IsKeyDown(kbKeys.attack) && fireballkeyReleased)
            {
                for (int counter = 0; counter < equipmentList.Count; counter++)
                {
                    Armor armor = equipmentList[counter];
                    if (armor.armorType == Armor.ArmorType.oneHanded || armor.armorType == Armor.ArmorType.twoHanded)
                    {
                        int asd = armor.Durability;
                        armor.Durability--;
                    }
                }
                Projectile projectile = new Projectile(map, Game.content.Load<Texture2D>("Textures\\Spritesheets\\wolf"),
                    6, this, 60, Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
                    Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"));
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
            debug.Draw(spriteBatch);
            menu.Draw(spriteBatch, offsetRect);
            msg.Draw(spriteBatch, offsetRect);
            hud.Draw(spriteBatch);
        }
    }
}
