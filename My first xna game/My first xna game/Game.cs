using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;

namespace My_first_xna_game
{
    public class Game : Microsoft.Xna.Framework.Game

    {
        /*
         TODO:
         * A*
         * attack (sword)
         * round raduis for enemies
         * better AI for enemies
         * switch skill hud
         * 
         * fps
         * visable healing
         * guns
         * homing spells
         * ray casting
         * 
         * enemy type / id (bee, wolf, eater of worlds)
         * 
         * dust, smoke, fire, better light, partical shader, and realistic water shaders
         * shadows
         * effects
         * core collision
         * options
         * gamemodes
         * trade
         * pvp menu
         * chat
         * push and plate sound
         * item data(durabillity, price, weight, and more)
         * 
         * LAN
         * Best IA
         * 
         * :Fighter- bodybuilder, knight, barbarian :Roughe thief, clown: Mage
         * bodybuilder, lost hero, clown
         * special potions
         * building
         * turrets
         * redstone
         
         BUGS:
         * fix too much stat changes in skill tree at choose skill
         * fix boat bug
         * order update anyway in sprite and in hostile
         * fix depth in dmg
         * fix heal health and hud
         * stuck keyboard in multiplayer
         * hud bug in low health
         */

        // Graphics and controls
        public static GraphicsDeviceManager graphics;
        public static bool endGame = false;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        public enum WindowDepth { GUIFront, windowsSelector, windowsDataFront, windowsData, windowDataShadow, windows }
        public enum MapDepth { below, player, above }

        public static WindowDepth FloatToWindowDepth(float type)
        {
            return (WindowDepth)(int)(type * 1000);
        }

        public static MapDepth FloatToMapDepth(float type)
        {
            return (MapDepth)(int)(type * 1000);
        }

        public static float DepthToFloat(int type)
        {
            return (float)type / 1000;
        }

        // public static Rectangle worldRect;
        public static Scene scene;

        // Timer list
        public static List<Timer> timersList = new List<Timer>();

        // Class lists
        private List<GameObject> tagList = new List<GameObject>();

        // TODO: Is this needed?
        public static ContentManager content;
        public static Rectangle worldRect = new Rectangle(0, 0, 1920, 1080);

        private static StorageDevice storageDevice;
        private const string storageName = "myStorage";
        private const string fileName = "asd.sav";

        public struct SaveData
        {
            public List<Player.PlayerData> playersData;
        }

        public static bool TryConvertKeyboardInput(KeyboardState keyboard, KeyboardState oldKeyboard, out char key)
        {
            Keys[] keys = keyboard.GetPressedKeys();
            bool shift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

            if (keys.Length > 0 && !oldKeyboard.IsKeyDown(keys[0]))
            {
                switch (keys[0])
                {
                    //Alphabet keys
                    case Keys.A: key = shift ? 'A' : 'a'; return true;
                    case Keys.B: key = shift ? 'B' : 'b'; return true;
                    case Keys.C: key = shift ? 'C' : 'c'; return true;
                    case Keys.D: key = shift ? 'D' : 'd'; return true;
                    case Keys.E: key = shift ? 'E' : 'e'; return true;
                    case Keys.F: key = shift ? 'F' : 'f'; return true;
                    case Keys.G: key = shift ? 'G' : 'g'; return true;
                    case Keys.H: key = shift ? 'H' : 'h'; return true;
                    case Keys.I: key = shift ? 'I' : 'i'; return true;
                    case Keys.J: key = shift ? 'J' : 'j'; return true;
                    case Keys.K: key = shift ? 'K' : 'k'; return true;
                    case Keys.L: key = shift ? 'L' : 'l'; return true;
                    case Keys.M: key = shift ? 'M' : 'm'; return true;
                    case Keys.N: key = shift ? 'N' : 'n'; return true;
                    case Keys.O: key = shift ? 'O' : 'o'; return true;
                    case Keys.P: key = shift ? 'P' : 'p'; return true;
                    case Keys.Q: key = shift ? 'Q' : 'q'; return true;
                    case Keys.R: key = shift ? 'R' : 'r'; return true;
                    case Keys.S: key = shift ? 'S' : 's'; return true;
                    case Keys.T: key = shift ? 'T' : 't'; return true;
                    case Keys.U: key = shift ? 'U' : 'u'; return true;
                    case Keys.V: key = shift ? 'V' : 'v'; return true;
                    case Keys.W: key = shift ? 'W' : 'w'; return true;
                    case Keys.X: key = shift ? 'X' : 'x'; return true;
                    case Keys.Y: key = shift ? 'Y' : 'y'; return true;
                    case Keys.Z: key = shift ? 'Z' : 'z'; return true;

                    //Decimal keys
                    case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } return true;
                    case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } return true;
                    case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } return true;
                    case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } return true;
                    case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } return true;
                    case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } return true;
                    case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } return true;
                    case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } return true;
                    case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } return true;
                    case Keys.D9: if (shift) { key = '('; } else { key = '9'; } return true;

                    //Decimal numpad keys
                    case Keys.NumPad0: key = '0'; return true;
                    case Keys.NumPad1: key = '1'; return true;
                    case Keys.NumPad2: key = '2'; return true;
                    case Keys.NumPad3: key = '3'; return true;
                    case Keys.NumPad4: key = '4'; return true;
                    case Keys.NumPad5: key = '5'; return true;
                    case Keys.NumPad6: key = '6'; return true;
                    case Keys.NumPad7: key = '7'; return true;
                    case Keys.NumPad8: key = '8'; return true;
                    case Keys.NumPad9: key = '9'; return true;

                    //Special keys
                    case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } return true;
                    case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ':'; } return true;
                    case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } return true;
                    case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } return true;
                    case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } return true;
                    case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } return true;
                    case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } return true;
                    case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } return true;
                    case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } return true;
                    case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } return true;
                    case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } return true;
                    case Keys.Space: key = ' '; return true;
                }
            }

            key = (char)0;
            return false;
        }

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            content = Content;
            scene = new Title(graphics);

            // Initialize keyboard (old) state
            oldState = Keyboard.GetState();

            base.Initialize(); 
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //if exit
            if (endGame)
            {
                MediaPlayer.Stop();
                Exit();
            }

            // Update newState
            KeyboardState newState = Keyboard.GetState();

            // Update
            foreach (Timer timer in timersList)
            {
                timer.Update(gameTime);
            }
            if (scene != null)
            {
                scene.Update(newState, oldState, gameTime);
            }


            // Update oldState
            oldState = newState;

            base.Update(gameTime);
        }

        public static void InitiateSave()
        {
            if (true)//!Guide.IsVisible) //TODO: xbox compability
            {
                storageDevice = null;
                StorageDevice.BeginShowSelector(PlayerIndex.One, SaveToDevice, null);
            }
        }

        private static void SaveToDevice(IAsyncResult result)
        {
            storageDevice = StorageDevice.EndShowSelector(result);
            if (storageDevice != null && storageDevice.IsConnected)
            {
                List<Player.PlayerData> playersDataToSave = new List<Player.PlayerData>();
                foreach (Player player in PlayerManager.playersList)
                {
                    playersDataToSave.Add(player.getSaveData());
                }
                SaveData SaveData = new SaveData()
                {
                    playersData = playersDataToSave
                };
                IAsyncResult r = storageDevice.BeginOpenContainer(storageName, null, null);
                result.AsyncWaitHandle.WaitOne();
                StorageContainer container = storageDevice.EndOpenContainer(r);
                if (container.FileExists(fileName))
                    container.DeleteFile(fileName);
                Stream stream = container.CreateFile(fileName);
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, SaveData);
                stream.Close();
                container.Dispose();
                result.AsyncWaitHandle.Close();
            }
        }

        public static void InitiateLoad()
        {
            if (true)//!Guide.IsVisible) //TODO: xbox compability
            {
                storageDevice = null;
                StorageDevice.BeginShowSelector(PlayerIndex.One, LoadFromDevice, null);
            }
        }

        private static void LoadFromDevice(IAsyncResult result)
        {
            storageDevice = StorageDevice.EndShowSelector(result);
            IAsyncResult r = storageDevice.BeginOpenContainer(storageName, null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = storageDevice.EndOpenContainer(r);
            result.AsyncWaitHandle.Close();
            if (container.FileExists(fileName))
            {
                Stream stream = container.OpenFile(fileName, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                SaveData SaveData = (SaveData)serializer.Deserialize(stream);
                stream.Close();
                container.Dispose();
                //Update the game based on the save game file
                for (int i = 0; i < PlayerManager.playersList.Count; i++)
                {
                    PlayerManager.playersList[i].LoadData(SaveData.playersData[i]);
                }

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aqua);
            if (scene != null)
            {
                scene.Draw(spriteBatch);
            }
            base.Draw(gameTime);
        }
    }
}