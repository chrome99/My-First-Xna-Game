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
         * attack (sword)
         * combos & skills
         * round raduis for enemies
         * better AI for enemies
         * 
         * visbale xp and healing
         * xp
         * stats
         * guns
         * homing spells
         * vehicles
         * 
         * minimap
         * ray casting
         * speed with accelertaion
         * 
         * enemy type / id (bee, wolf, eater of worlds)
         * 
         * bloom shader
         * shadows
         * effects
         * better knockback
         * core collision
         * options
         * gamemodes
         * trade
         * pvp menu
         * levels (in level up you can upgrade skills or stats)
         * chat
         * command line
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
         * order update anyway in sprite and in hostile
         * fix depth in dmg
         * fix height in multiple maps in
         * fix tileset drawing bug
         * fix heal health and hud
         * fix function GetID in GameObject
         * particals bug
         * fix drawing opacity
         * stuck keyboard in singleplayer and multiplayer
         * fix msgWindow intializing
         * remove depth
         * hud bug in low health
         */

        // Graphics and controls
        public static GraphicsDeviceManager graphics;
        public static bool endGame = false;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        public enum Depth { GUIFront, front, windowsSelector, windowsDataFront, windowsData, windowDataShadow, windows, above, jumping, player, projectiles, below, background }

        public static float DepthToFloat(Depth type)
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
        private static string storageName = "myStorage";
        private static string fileName = "asd.sav";

        [Serializable]
        public struct SaveData
        {
            public List<Player.PlayerData> playersData;
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
            //if (spriteBatch.)
            //spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}