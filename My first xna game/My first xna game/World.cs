using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class World : Scene
    {
        //effects
        Effect effect;
        Texture2D light;
        RenderTarget2D lightsTarget;

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

            effect = Game.content.Load<Effect>("Effects\\FirstOne");
            light = Game.content.Load<Texture2D>("Textures\\Sprites\\lightmask");
            lightsTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, Game.worldRect.Width, Game.worldRect.Height);
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

            //draw lights
            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(lightsTarget);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            spriteBatch.Draw(light, new Rectangle(cameraList[0].screenRect.Width / 2 - 500 / 2, cameraList[0].screenRect.Height / 2 - 500 / 2, 500, 500), Color.White);
            spriteBatch.End();

            //draw camera
            foreach (Camera camera in cameraList)
            {
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(camera.renderTarget);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                camera.Draw(spriteBatch);
                spriteBatch.End();
            }

            
            //map
            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
            foreach (Camera camera in cameraList)
            {
                graphicsDeviceManager.GraphicsDevice.Viewport = camera.viewport;
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.transform);

                effect.Parameters["lightMask"].SetValue(lightsTarget);
                effect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(camera.renderTarget, Vector2.Zero, Color.White);

                spriteBatch.End();
            }


            //split
            graphicsDeviceManager.GraphicsDevice.Viewport = new Viewport(Game.worldRect);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //vertical line
            spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Windows\\black dot"),
                new Rectangle(Game.worldRect.Width / 2 - 1, 0, 2, Game.worldRect.Height),
                null, Color.White);

            //horizontal line
            spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Windows\\black dot"),
                new Rectangle(0, Game.worldRect.Height / 2 - 1, Game.worldRect.Width, 3),
                null, Color.White);

            spriteBatch.End();
        }
    }
}
