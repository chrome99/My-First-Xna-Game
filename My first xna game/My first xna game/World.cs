using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class World : Scene
    {
        public List<Camera> cameraList;

        public World(GraphicsDeviceManager graphicsDeviceManager, Camera camera)
            : base(graphicsDeviceManager)
        {
            cameraList.Add(camera);
        }

        public World(GraphicsDeviceManager graphicsDeviceManager, List<Camera> cameraList)
            : base(graphicsDeviceManager)
        {
            this.cameraList = cameraList;
        }

        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            foreach (Camera camera in cameraList)
            {
                camera.Update();                
            }
            cameraList[0].map.Update(newState, oldState, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            foreach (Camera camera in cameraList)
            {
                graphicsDeviceManager.GraphicsDevice.Viewport = camera.viewport;
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                camera.Draw(spriteBatch);
                spriteBatch.End();
            }
            graphicsDeviceManager.GraphicsDevice.Viewport = new Viewport(Game.worldRect);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //vertical line
            spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Windows\\black dot"),
                new Rectangle(Game.worldRect.Width / 2 - 1, 0, 2, Game.worldRect.Height),
                null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(Game.Depth.GUIFront));

            //horizontal line
            spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Windows\\black dot"),
                new Rectangle(0, Game.worldRect.Height / 2 - 1, Game.worldRect.Width, 3),
                null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(Game.Depth.GUIFront));
            spriteBatch.End();
        }

        public void DrawBAS(SpriteBatch spriteBatch, int asd = 123)
        {
            foreach (Camera camera in cameraList)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                RenderTarget2D renderTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, camera.screenRect.Width, camera.screenRect.Height);
                //graphicsDeviceManager.GraphicsDevice.Viewport = camera.viewport;
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(renderTarget);
                graphicsDeviceManager.GraphicsDevice.Clear(Color.Black);
                
                spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Sprites\\brick1"), new Vector2(100f, 100f), Color.White);
                camera.Draw(spriteBatch);
                spriteBatch.End();
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw((Texture2D)renderTarget,
                new Vector2(0, 0),          // x,y position
                Game.worldRect,   // just one grid
                Color.White                    // no color scaling
                );
                spriteBatch.End();
            }
            //graphicsDeviceManager.GraphicsDevice.Viewport = Game.defaultViewPort;
            //graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        }

        public void DrawABS(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (Camera camera in cameraList)
            {
                camera.Draw(spriteBatch);

            }
            spriteBatch.End();
        }
    }
}
