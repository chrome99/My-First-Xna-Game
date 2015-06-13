using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace My_first_xna_game
{
    public class World : Scene
    {
        public static bool save = false;
        public static bool load = false;

        private StorageDevice storageDevice;
        private string storageName = "myStorage";
        private string fileName = "asd.sav";

        [Serializable]
        public struct SaveData
        {
            public Map.MapData map;
        }

        private List<Camera> cameraList;
        private List<Map> mapsList = new List<Map>();

        public World(GraphicsDeviceManager graphicsDeviceManager, Camera camera)
            : base(graphicsDeviceManager)
        {
            cameraList.Add(camera);
            IntializeMapsList();
        }

        public World(GraphicsDeviceManager graphicsDeviceManager, List<Camera> cameraList)
            : base(graphicsDeviceManager)
        {
            this.cameraList = cameraList;
            IntializeMapsList();
        }

        private void IntializeMapsList()
        {
            foreach(Camera camera in cameraList)
            {
                if (!mapsList.Contains(camera.player.map))
                {
                    mapsList.Add(camera.player.map);
                }
            }
        }

        public void InitiateSave()
        {
            if (true)//!Guide.IsVisible) //TODO: xbox compability
            {
                storageDevice = null;
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.SaveToDevice, null);
            }
        }

        private void SaveToDevice(IAsyncResult result)
        {
            storageDevice = StorageDevice.EndShowSelector(result);
            if (storageDevice != null && storageDevice.IsConnected)
            {
                SaveData SaveData = new SaveData() { map = mapsList[0].getSaveData()};
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

        private void InitiateLoad()
        {
            if (true)//!Guide.IsVisible) //TODO: xbox compability
            {
                storageDevice = null;
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.LoadFromDevice, null);
            }
        }

        void LoadFromDevice(IAsyncResult result)
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
                mapsList[0].LoadData(SaveData.map);
            }
        }

        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (save)
            {
                save = false;
                InitiateSave();
            }
            if (load)
            {
                load = false;
                InitiateLoad();
            }
            foreach (Camera camera in cameraList)
            {
                camera.Update();                
            }
            foreach (Map map in mapsList)
            {
                map.Update(newState, oldState, gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw camera
            foreach (Camera camera in cameraList)
            {
                camera.CatchDraw(spriteBatch);
            }
            foreach(Camera camera in cameraList)
            {
                //draw camera and lighting
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
                graphicsDeviceManager.GraphicsDevice.Viewport = camera.viewport;
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                if (camera.renderTarget != null)
                {
                    camera.effect.Parameters["lightMask"].SetValue(camera.lightsTarget);

                    camera.effect.Parameters["active"].SetValue(true);
                    camera.effect.CurrentTechnique.Passes[0].Apply();
                    spriteBatch.Draw(camera.renderTarget, Vector2.Zero, Color.White);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                    camera.effect.Parameters["active"].SetValue(false);
                    camera.effect.CurrentTechnique.Passes[0].Apply();
                    camera.DrawWindows(spriteBatch);
                }
                else
                {
                    camera.DrawGrave(spriteBatch);
                }
                spriteBatch.End();
            }


            
            //split
            graphicsDeviceManager.GraphicsDevice.Viewport = new Viewport(Game.worldRect);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //vertical line
            spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Sprites\\white dot"),
                new Rectangle(Game.worldRect.Width / 2 - 1, 0, 2, Game.worldRect.Height),
                null, Color.Black);

            //horizontal line
            spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Sprites\\white dot"),
                new Rectangle(0, Game.worldRect.Height / 2 - 1, Game.worldRect.Width, 3),
                null, Color.Black);

            spriteBatch.End();
        }
    }
}
