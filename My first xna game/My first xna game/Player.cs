using System;
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
        private ChooseSkill chooseSkill;

        private Sprite HoldedSprite;

        private Vehicle vehicle;
        private VehicleSave vehicleSave;
        private struct VehicleSave
        {
            public Texture2D texture;
            public Vector2 position;
            public Vector2 size;
            public bool ShowAnimation;
        }
        public bool Riding
        {
            get { return vehicle != null; }
        }

        public List<string> impassableTilesTag { get; private set; }

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
        public Timer defendingTimer;
        private int defendingTime = 200;
        public Timer defendingCooldownTimer;
        private int defendingCoolddownTime = 500;

        private bool menuKeyReleased = false;

        public Map map;

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

        public struct PlayerData
        {
            public int gold;
            public int maxGold;
            public int maxPackWeight;
            public bool maxWeightReached;
            public bool enableRunning;

            public Vector2 position;

            public List<int> collisionsList;

            public Stats stats;

            public string mapName;
        }

        public PlayerData getSaveData()
        {
            return new PlayerData()
            {
                gold = this.gold,
                maxGold = this.maxGold,
                maxPackWeight = this.maxPackWeight,
                maxWeightReached = this.maxWeightReached,
                enableRunning = this.enableRunning,
                position = this.position,
                collisionsList = this.collisionsList,
                stats = this.stats,
                mapName = this.map.name
            };
        }

        public void LoadData(PlayerData data)
        {
            gold = data.gold;
            maxGold = data.maxGold;
            maxPackWeight = data.maxPackWeight;
            maxWeightReached = data.maxWeightReached;
            enableRunning = data.enableRunning;
            position = data.position;
            collisionsList = data.collisionsList;
            stats = data.stats;
            Map playerMap = MapCollection.mapsList.Find(x => x.name == data.mapName);
            if (map != null)
            {
                if (map != playerMap)
                {
                    this.map.RemoveObject(this);
                    playerMap.AddObject(this);
                    map = playerMap;
                }
            }
            else
            {
                map = playerMap;
                map.AddObject(this);
            }

        }

        public Player(Texture2D texture, Vector2 position, PlayerKeys keys, Stats stats)
            : base(texture, position, MovementManager.Auto.off)
        {
            this.kbKeys = keys;
            this.stats = stats;

            pack = new Pack(this);
            debug = new DebugHUD(Game.content.Load<SpriteFont>("Fonts\\Debug1"), Color.Wheat, this, keys.opDebug);
            hud = new HostileHUD(this);
            shop = new Shop();

            runningAccelerationMax = 1;

            defendingTimer = new Timer(defendingTime, false);
            defendingCooldownTimer = new Timer(defendingCoolddownTime, true);
        }

        public void IntializeMapParamters(Map map)
        {
            this.map = map;
            menu = new Menu(map, this);
            msg = new Message(map, this);
            chooseSkill = new ChooseSkill(this);
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

        public void AddImpassableTileTag(string tag)
        {
            if (impassableTilesTag == null)
            {
                impassableTilesTag = new List<string>();
            }
            impassableTilesTag.Add(tag);
        }

        public void RemoveImpassableTileTag(string tag)
        {
            impassableTilesTag.Remove(tag);
        }

        public void Ride(Vehicle vehicle)
        {
            vehicleSave = new VehicleSave
            {
                texture = this.texture,
                position = this.position,
                size = this.size,
                ShowAnimation = this.ShowAnimation
            };
            texture = vehicle.texture;
            position = vehicle.position;
            size = vehicle.size;
            ShowAnimation = vehicle.ShowAnimation;

            for (int i = 0; i < map.gameObjectList.Count; i++)
            {
                GameObject gameObject = map.gameObjectList[i];
                foreach (string vehicleTag in vehicle.passableTilesTags)
                {
                    if (gameObject.tags.Contains(vehicleTag))
                    {
                        gameObject.passable = true;
                    }
                }
            }
            foreach (string vehicleTag in vehicle.impassableTilesTags)
            {
                //create temporery object
                AddImpassableTileTag(vehicleTag);
            }

            this.vehicle = vehicle;
            vehicle.Kill();
        }

        public void Backoff()
        {
            texture = vehicleSave.texture;
            position = vehicleSave.position;
            size = vehicleSave.size;
            ShowAnimation = vehicleSave.ShowAnimation;

            for (int i = 0; i < map.gameObjectList.Count; i++)
            {
                GameObject gameObject = map.gameObjectList[i];
                foreach (string vehicleTag in vehicle.passableTilesTags)
                {
                    if (gameObject.tags.Contains(vehicleTag))
                    {
                        gameObject.passable = false;
                    }
                }
            }
            foreach (string vehicleTag in vehicle.impassableTilesTags)
            {
                //delete temporery object
                RemoveImpassableTileTag(vehicleTag);
            }

            vehicle.Revive();
            vehicle = null;
        }

        public void HoldObject(Sprite sprite)
        {
            HoldedSprite = sprite;
            map.RemoveObject(sprite);
            map.AddObject(HoldedSprite);
            HoldedSprite.canCollide = false;
            HoldedSprite.position = position;
            HoldedSprite.position.Y -= HoldedSprite.bounds.Height / 3 * 2;
        }

        public void ThrowObject()
        {
            Sprite originalSprite = HoldedSprite;

            Vector2 newPosition;
            newPosition.X = position.X + bounds.Width / 2 - bounds.Width / 2;
            newPosition.Y = position.Y + bounds.Height - originalSprite.bounds.Height;

            Rectangle originalSpriteCore = originalSprite.core;
            originalSpriteCore.X = (int)MovementManager.MoveVector(newPosition, Tile.size * 2, direction).X;
            originalSpriteCore.Y = (int)MovementManager.MoveVector(newPosition, Tile.size * 2, direction).Y;
            originalSprite.canCollide = true;
            bool passable = originalSprite.passable;
            originalSprite.passable = false;
            if (!movementManager.CollisionCheck(originalSprite, originalSpriteCore) && movementManager.InsideMap(originalSprite, originalSpriteCore))
            {
                DiscardObject();

                originalSprite.position.X = originalSpriteCore.X;
                originalSprite.position.Y = originalSpriteCore.Y;
                map.AddObject(originalSprite);
            }
            originalSprite.passable = passable;
        }

        public void DiscardObject()
        {
            map.RemoveObject(HoldedSprite);
            HoldedSprite = null;
        }

        public bool canInteract()
        {
            return HoldedSprite == null;
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

        public void UpdatePlayer(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (jumping)
            {
                UpdateJump();
            }
            UpdateDefend();

            if (HoldedSprite != null)
            {
                HoldedSprite.position.X = position.X;
                HoldedSprite.position.Y = position.Y - HoldedSprite.bounds.Height / 3 * 2;
            }

            shop.Update(newState, oldState, gameTime);
            menu.Update(newState, oldState, gameTime);
            debug.Update();
            debug.UpdateInput(newState, oldState);
            msg.Update(gameTime, newState, oldState);
            chooseSkill.Update(gameTime, newState, oldState);

            UpdateInput(newState, oldState);
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

        protected void UpdateInput(KeyboardState newState, KeyboardState oldState)
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

            if (shop.alive || msg.alive || menu.alive || forceMoving)
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
                if (HoldedSprite != null)
                {
                    ThrowObject();
                }
                else
                {
                    for (int counter = 0; counter < equipmentList.Count; counter++)
                    {
                        Bow weapon = equipmentList[counter] as Bow;
                        if (weapon != null)
                        {
                            weapon.Attack(map, this);
                        }
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
                int x = PlayerManager.playersList.IndexOf(this);
                x++;
            }

            //-Update player movement status

            if (playerMoving && playerRunning)
            {
                MovingState = MovementManager.MovingState.running;

            }
            else if (playerMoving)
            {
                MovingState = MovementManager.MovingState.walking; //srsly
            }
            else
            {
                MovingState = MovementManager.MovingState.standing;
            }
        }

        public Vector2 getMerchantPosition()
        {
            return shop.getMerchantPosition();
        }

        public override void DrawPlayer(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (HoldedSprite != null)
            {
                HoldedSprite.Draw(spriteBatch, offsetRect);
            }
        }

        public void DrawPlayerItems(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            shop.Draw(spriteBatch, offsetRect);
            menu.Draw(spriteBatch, offsetRect);
            msg.Draw(spriteBatch, offsetRect);
            hud.Draw(spriteBatch);
            debug.Draw(spriteBatch);
            chooseSkill.Draw(spriteBatch);
        }
    }
}