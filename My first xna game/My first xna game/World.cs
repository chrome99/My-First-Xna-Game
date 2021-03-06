﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace My_first_xna_game
{
    public class World : Scene
    {
        public List<Camera> cameraList;
        private List<Map> mapsList = new List<Map>();

        public World(GraphicsDeviceManager graphicsDeviceManager, Camera camera)
            : base(graphicsDeviceManager)
        {
            cameraList.Add(camera);
            UpdateMapsList();
        }

        public World(GraphicsDeviceManager graphicsDeviceManager, List<Camera> cameraList)
            : base(graphicsDeviceManager)
        {
            this.cameraList = cameraList;
            UpdateMapsList();
        }

        public void UpdateMapsList()
        {
            foreach(Camera camera in cameraList)
            {
                if (!mapsList.Contains(camera.player.map) && camera.player.map != null)
                {
                    mapsList.Add(camera.player.map);
                }
            }
        }

        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {   
            foreach (Camera camera in cameraList)
            {
                camera.Update();
            }
            foreach (Map map in mapsList)
            {
                map.Update(newState, oldState, gameTime);
                map.UpdateCollision(cameraList);
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
                    camera.DrawWindows(spriteBatch);


                    /*
                    *** to add mini map
                    spriteBatch.End();
                    graphicsDeviceManager.GraphicsDevice.Viewport = camera.minimapViewport;
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.minimapTransform);
                    camera.DrawMinimap(spriteBatch);*/
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
