using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Player : Hostile
    {
        private bool debugAStar = false;
        private bool debugConceptArt = false;
        private Picture debugConceptArtPic = new Picture(Game.content.Load<Texture2D>("Textures\\ConceptArt\\livenglow"), Vector2.Zero, null) { drawingBoundaries = new Vector2(400, 200) };

        public Camera camera;
        public PlayerKeys kbKeys;

        private List<Combo> combosList = new List<Combo>() { ComboCollection.fireCombo };
        private List<ComboCollection.PlayerKeysIndex> comboKeysList = new List<ComboCollection.PlayerKeysIndex>();
        public Timer comboTimer = new Timer(300f, true);

        public SkillTree skillTree;
        public Skill currentSkill;

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
        private CommandLine commandLine;
        private SwitchSkillDisplay switchSkillDisplay;

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

        public bool holdUpdateInput = false;

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

        private bool useSkillKeyReleased = false;
        public Timer switchSkillMenuTimer = new Timer(200f, false);

        private bool menuKeyReleased = false;

        private bool commandKeyReleased = false;

        private bool mvRightKeyReleased = false;
        private bool mvLeftKeyReleased = false;
        private bool mvUpKeyReleased = false;
        private bool mvDownKeyReleased = false;

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
            public Keys opCommand;
            public Keys useSkill;
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

            public List<string> combosList;
            public List<string> skillsList;
            public int currentSkill;
            public List<int> equipmentList;

            public Vector2 position;

            public List<int> collisionsList;

            public Stats stats;

            public string mapName;
        }

        public PlayerData getSaveData()
        {
            List<string> combosNames = new List<string>();
            foreach(Combo combo in combosList)
            {
                combosNames.Add(combo.name);
            }

            List<string> skillsNames = new List<string>();
            foreach(Skill skill in SkillCollection.list)
            {
                skillsNames.Add(skill.name);
            }

            int currentSkillIndex = skillsList.IndexOf(currentSkill);

            List<int> armorsIDs = new List<int>();
            foreach (Item item in ItemCollection.list)
            {
                Armor armor = item as Armor;
                if (armor != null)
                {
                    armorsIDs.Add(armor.iconID);
                }
            }

            return new PlayerData()
            {
                gold = this.gold,
                maxGold = this.maxGold,
                maxPackWeight = this.maxPackWeight,
                maxWeightReached = this.maxWeightReached,
                enableRunning = this.enableRunning,
                position = this.position,
                skillsList = skillsNames,
                currentSkill = currentSkillIndex,
                combosList = combosNames,
                equipmentList = armorsIDs,
                collisionsList = this.collisionsList,
                stats = this.stats,
                mapName = this.map.name
            };
        }

        public void LoadData(PlayerData data)
        {
            List<Combo> dataCombosList = new List<Combo>();
            foreach (Combo combo in ComboCollection.list)
            {
                foreach (string comboName in data.combosList)
                {
                    if (combo.name == comboName)
                    {
                        dataCombosList.Add(combo);
                    }
                }
            }

            List<Armor> dataEquipmentList = new List<Armor>();
            foreach (Item item in ItemCollection.list)
            {
                Armor armor = item as Armor;
                if (armor != null)
                {
                    foreach (int armorID in data.equipmentList)
                    {
                        if (armor.iconID == armorID)
                        {
                            dataEquipmentList.Add(armor);
                        }
                    }
                }

            }

            List<Skill> dataSkillsList = new List<Skill>();
            foreach (Skill skill in SkillCollection.list)
            {
                foreach (string skillName in data.skillsList)
                {
                    if (skill.name == skillName)
                    {
                        dataSkillsList.Add(skill);
                    }
                }
            }
            
            Skill dataCurrentSkill;
            if (data.currentSkill == -1)
            {
                dataCurrentSkill = null;
            }
            else
            {
                dataCurrentSkill = dataSkillsList[data.currentSkill];
            }

            gold = data.gold;
            maxGold = data.maxGold;
            maxPackWeight = data.maxPackWeight;
            maxWeightReached = data.maxWeightReached;
            enableRunning = data.enableRunning;
            position = data.position;
            equipmentList = dataEquipmentList;
            combosList = dataCombosList;
            skillsList = dataSkillsList;
            currentSkill = dataCurrentSkill;
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

        public Player(Texture2D texture, Vector2 position, PlayerKeys kbKeys, Stats stats)
            : base(texture, position, MovementManager.Auto.off)
        {
            this.kbKeys = kbKeys;
            this.stats = stats;

            pack = new Pack(this);
            hud = new HostileHUD(this);
            shop = new Shop();
            debug = new DebugHUD(new Text(Game.content.Load<SpriteFont>("Fonts\\Debug1"), Vector2.Zero, Color.Wheat, ""), this);

            skillTree = new SkillTree();
            runningAccelerationMax = 1;

            defendingTimer = new Timer(defendingTime, false);
            defendingCooldownTimer = new Timer(defendingCoolddownTime, true);

            LearnSkill(SkillCollection.ballOfDestruction);
            LearnSkill(SkillCollection.wallOfFire);
            LearnSkill(SkillCollection.ballOfDestruction2);
            LearnSkill(SkillCollection.ballOfDestruction3);
            LearnSkill(SkillCollection.ballOfDestruction4);
        }

        public void IntializeMapParamters(Map map)
        {
            this.map = map;
            menu = new Menu(map, this);
            msg = new Message(map, this);
            switchSkillDisplay = new SwitchSkillDisplay(map, this);
            chooseSkill = new ChooseSkill(this);
            commandLine = new CommandLine(this);
        }

        public void AssignToCamera(Camera camera)
        {
            this.camera = camera;
        }

        public void AddExp(int exp)
        {
            stats.exp += exp;
            if (stats.exp >= 10 * (stats.level * stats.level))
            {
                Player player = this as Player;
                if (player != null)
                {
                    player.LevelUp();
                }
            }
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
            HoldedSprite.MapDepth = Game.MapDepth.above;
            HoldedSprite.position = position;
            HoldedSprite.position.Y -= HoldedSprite.bounds.Height / 3 * 2;
        }

        public void ThrowObject()
        {
            Sprite originalSprite = HoldedSprite;

            Rectangle originalSpriteCore = originalSprite.core;
            Vector2 newPosition = MovementManager.GetRectNextTo(originalSpriteCore, bounds, direction);
            originalSpriteCore.X = (int)newPosition.X;
            originalSpriteCore.Y = (int)newPosition.Y;

            newPosition = MovementManager.MoveVector(new Vector2(originalSpriteCore.X, originalSpriteCore.Y), Tile.size, direction);
            originalSpriteCore.X = (int)newPosition.X;
            originalSpriteCore.Y = (int)newPosition.Y;
            originalSprite.canCollide = true;
            HoldedSprite.MapDepth = Game.MapDepth.player;
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
            UpdatePlayerHUD();
        }

        public void HandleHeal()
        {
            UpdatePlayerHUD();
        }

        public void LevelUp()
        {
            ChooseSkillWindow();
        }

        private void ChooseSkillWindow()
        {
            chooseSkill.alive = true;
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

        public void UpdatePlayerHUD()
        {
            hud.UpdateHearts(stats.health);
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

            if (comboTimer.result)
            {
                comboKeysList.Clear();
                comboTimer.Reset();
            }

            if (switchSkillMenuTimer.result && !switchSkillDisplay.alive)
            {
                switchSkillDisplay.Revive();
                switchSkillMenuTimer.Reset(false);
            }

            shop.Update(newState, oldState, gameTime);
            menu.Update(newState, oldState, gameTime);
            debug.Update();
            debug.UpdateInput(newState, oldState);
            msg.Update(gameTime, newState, oldState);
            chooseSkill.Update(gameTime, newState, oldState);
            commandLine.Update(gameTime, newState, oldState);
            switchSkillDisplay.Update(gameTime, newState, oldState);

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
            //command line
            if (newState.IsKeyDown(kbKeys.opCommand) && commandKeyReleased)
            {
                commandLine.alive = !commandLine.alive;
                commandKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.opCommand))
            {
                commandKeyReleased = true;
            }

            if (commandLine.alive) { return; }

            //hold update input for one frame
            if (holdUpdateInput)
            {
                holdUpdateInput = false;

                attackKeyReleased = false;
                commandKeyReleased = false;
                defendKeyReleased = false;
                jumpKeyReleased = false;
                menuKeyReleased = false;
                useSkillKeyReleased = false;
                mvRightKeyReleased = false;
                mvLeftKeyReleased = false;
                mvUpKeyReleased = false;
                mvDownKeyReleased = false;
                return;
            }

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

            if (shop.alive || msg.alive || menu.alive || chooseSkill.alive || switchSkillDisplay.alive || forceMoving)
            {
                return;
            }
            Combo combo;
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
                comboTimer.Reset();
                if (comboKeysList.Count == 3)
                {
                    comboKeysList.Remove(comboKeysList[0]);
                }
                comboKeysList.Add(ComboCollection.PlayerKeysIndex.attack);

                combo = CheckForCombo();
                if (HoldedSprite != null)
                {
                    ThrowObject();
                }
                else if (combo == null)
                {
                    if (!Interact())
                    {
                        Attack();
                    }
                }
                attackKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.attack))
            {
                attackKeyReleased = true;
            }

            if (jumping || (defendingTimer.Counting)) { return; }

            UpdateMovementInput(newState, oldState);

            if (vehicle != null) { return; }

            //jump
            if (newState.IsKeyDown(kbKeys.jump) && jumpKeyReleased)
            {
                comboTimer.Reset();
                if (comboKeysList.Count == 3)
                {
                    comboKeysList.Remove(comboKeysList[0]);
                }
                comboKeysList.Add(ComboCollection.PlayerKeysIndex.jump);
                Jump();
                jumpKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.jump))
            {
                jumpKeyReleased = true;
            }

            //defend
            if (newState.IsKeyDown(kbKeys.defend) && defendKeyReleased)
            {
                comboTimer.Reset();
                if (comboKeysList.Count == 3)
                {
                    comboKeysList.Remove(comboKeysList[0]);
                }
                comboKeysList.Add(ComboCollection.PlayerKeysIndex.defend);
                Defend();
                defendKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.defend))
            {
                defendKeyReleased = true;
            }

            //use skill
            if (newState.IsKeyDown(kbKeys.useSkill) && useSkillKeyReleased)
            {

                //if the player is about to switch skill(holding running button) don't use skill
                if (playerRunning && !switchSkillMenuTimer.result)
                {
                    switchSkillMenuTimer.Reset();
                }
                else
                {
                    if (currentSkill != null)
                    {
                        currentSkill.Use(map, this);
                    }
                }

                useSkillKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(kbKeys.useSkill))
            {
                //switch skill (if running button is pressed and use skill button released)
                if (!useSkillKeyReleased)
                {
                    if (playerRunning)
                    {
                        SwitchSkill();
                    }
                    switchSkillMenuTimer.Reset(false);
                }

                useSkillKeyReleased = true;
            }


            //Do combo
            combo = CheckForCombo();
            if (combo != null)
            {
                combo.comboFunction(this);
                comboKeysList.Clear();
            }
        }

        private Combo CheckForCombo()
        {
            if (comboKeysList.Count == 3)
            {
                foreach (Combo combo in combosList)
                {
                    bool result = true;
                    for (int i = 0; i < comboKeysList.Count; i++)
                    {
                        if (comboKeysList[i] != combo.comboKeys[i])
                        {
                            result = false;
                            break;
                        }
                    }
                    if (result)
                    {
                        return combo;
                    }
                }
            }
            return null;
        }

        private void UpdateMovementInput(KeyboardState newState, KeyboardState oldState)
        {
            //movement

            releasedKeysCount = 0;

            // if pressed left
            if (newState.IsKeyDown(kbKeys.mvLeft))
            {
                if (mvLeftKeyReleased)
                {
                    if (comboKeysList.Count == 3)
                    {
                        comboKeysList.Remove(comboKeysList[0]);
                    }
                    comboKeysList.Add(ComboCollection.PlayerKeysIndex.move);
                    comboTimer.Reset();
                    mvLeftKeyReleased = false;
                }

                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.left, (int)speed);
            }
            else if (!oldState.IsKeyDown(kbKeys.mvLeft))
            {
                mvLeftKeyReleased = true;
            }
            else
            {
                releasedKeysCount++;
            }

            // if pressed right
            if (newState.IsKeyDown(kbKeys.mvRight))
            {
                if (mvRightKeyReleased)
                {
                    if (comboKeysList.Count == 3)
                    {
                        comboKeysList.Remove(comboKeysList[0]);
                    }
                    comboKeysList.Add(ComboCollection.PlayerKeysIndex.move);
                    comboTimer.Reset();
                    mvRightKeyReleased = false;
                }

                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.right, (int)speed);
            }
            else if (!oldState.IsKeyDown(kbKeys.mvRight))
            {
                mvRightKeyReleased = true;
            }
            else
            {
                releasedKeysCount++;
            }

            // if pressed up
            if (newState.IsKeyDown(kbKeys.mvUp))
            {
                if (mvUpKeyReleased)
                {
                    if (comboKeysList.Count == 3)
                    {
                        comboKeysList.Remove(comboKeysList[0]);
                    }
                    comboKeysList.Add(ComboCollection.PlayerKeysIndex.move);
                    comboTimer.Reset();
                    mvUpKeyReleased = false;
                }

                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.up, (int)speed);
            }
            else if (!oldState.IsKeyDown(kbKeys.mvUp))
            {
                mvUpKeyReleased = true;
            }
            else
            {
                releasedKeysCount++;
            }

            // if pressed down
            if (newState.IsKeyDown(kbKeys.mvDown))
            {
                if (mvDownKeyReleased)
                {
                    if (comboKeysList.Count == 3)
                    {
                        comboKeysList.Remove(comboKeysList[0]);
                    }
                    comboKeysList.Add(ComboCollection.PlayerKeysIndex.move);
                    comboTimer.Reset();
                    mvDownKeyReleased = false;
                }

                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.down, (int)speed);
            }
            else if (!oldState.IsKeyDown(kbKeys.mvDown))
            {
                mvDownKeyReleased = true;
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

        private bool Interact()
        {
            Rectangle interactRect = new Rectangle(0, 0, Tile.size, Tile.size);
            Vector2 result = MovementManager.GetRectNextTo(interactRect, bounds, direction);
            interactRect.X = (int)result.X;
            interactRect.Y = (int)result.Y;

            foreach(GameObject gameObject in map.gameObjectList)
            {
                if (camera.InCamera(gameObject))
                {
                    if (interactRect.Intersects(gameObject.core) && gameObject.interactFunction != null)
                    {
                        gameObject.interactFunction(this, gameObject);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DebugAStarToggleSetGet = false;
        private Vector2 AStarDebugLastPosition;
        private void Attack()
        {
            if (debugConceptArt)
            {
                if (debugConceptArtPic.visible == false)
                {
                    debugConceptArtPic.visible = true;
                    debugConceptArtPic.opacity = 100;
                }
                else if (debugConceptArtPic.opacity == 100)
                {
                    debugConceptArtPic.opacity = 50;
                }
                else if (debugConceptArtPic.opacity == 50)
                {
                    debugConceptArtPic.visible = false;
                }
                return;
            }
            if (debugAStar)
            {
                if (DebugAStarToggleSetGet)
                {
                    List<Vector2> way = movementManager.WayTo(new Vector2(core.X, core.Y), AStarDebugLastPosition);
                    movementManager.HighlightWayTo(new Vector2(core.X, core.Y), AStarDebugLastPosition);
                    foreach (Vector2 node in way)
                    {
                        destinationsList.Add(node);
                    }
                    DebugAStarToggleSetGet = false;
                }
                else
                {
                    AStarDebugLastPosition = new Vector2(core.X, core.Y);
                    DebugAStarToggleSetGet = true;
                }
                return;
            }
            for (int counter = 0; counter < equipmentList.Count; counter++)
            {
                Weapon weapon = equipmentList[counter] as Weapon;
                if (weapon != null)
                {
                    weapon.Attack(map, this);
                    break;
                }
            }
        }

        private void Jump()
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
                Vector2 collisionVector = MovementManager.MoveVector(position, VectorMoveTo, direction); //TODO: something is wrong here. i can smell it
                collisionRect.X = (int)collisionVector.X;
                collisionRect.Y = (int)collisionVector.Y;
                if (!movementManager.CollisionCheck(this, collisionRect))
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\fart").Play();
                    originalJumpingPosition = position;
                    canCollide = false;
                    passable = true;
                    jumping = true;
                    return;
                }
            }
        }

        private void Defend()
        {
            if (defendingCooldownTimer.result)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\fart").Play();
                defendingTimer.Active();
                defendingTimer.Reset();
            }
        }

        private void SwitchSkill()
        {
            if (skillsList.Count == 1)
            {
                if (currentSkill == null)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                    currentSkill = skillsList[0];
                }
            }
            else if (skillsList.Count > 1)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                int currentSkillIndex = skillsList.IndexOf(currentSkill);

                //if he is on the last skill on the list
                if (currentSkillIndex == skillsList.Count - 1)
                {
                    currentSkill = skillsList[0];
                }
                else
                {
                    currentSkill = skillsList[currentSkillIndex + 1];
                }
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
            if (debugConceptArt)
            {
                debugConceptArtPic.DrawWithoutSource(spriteBatch, new Rectangle());
            }
            shop.Draw(spriteBatch, offsetRect);
            menu.Draw(spriteBatch, offsetRect);
            msg.Draw(spriteBatch, offsetRect);
            switchSkillDisplay.Draw(spriteBatch, offsetRect);
            hud.Draw(spriteBatch);
            debug.Draw(spriteBatch);
            chooseSkill.Draw(spriteBatch);
            commandLine.Draw(spriteBatch);
        }
    }
}