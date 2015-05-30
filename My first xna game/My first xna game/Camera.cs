﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Camera
    {
        private GraphicsDeviceManager graphicsDeviceManager;

        public Rectangle mapRect;
        public Rectangle screenRect;
        public Viewport viewport;

        public Matrix transform;
        public RenderTarget2D renderTarget;
        public Effect effect;
        public RenderTarget2D lightsTarget;

        public GameObject cameraLightspot;
        public Player player;

        public enum Corner { topLeft, topRight, bottomLeft, bottomRight }

        public Camera(GraphicsDeviceManager graphicsDeviceManager, Rectangle screenRect, GameObject cameraLightspot, Player player)
        {
            this.cameraLightspot = cameraLightspot;
            this.screenRect = screenRect;
            this.mapRect = screenRect;
            this.player = player;
            this.graphicsDeviceManager = graphicsDeviceManager;

            viewport = new Viewport(screenRect);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(0, 0, 0));
            renderTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, Game.worldRect.Width, Game.worldRect.Height);
            effect = Game.content.Load<Effect>("Effects\\FirstOne");
            lightsTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, Game.worldRect.Width, Game.worldRect.Height);
        }

        public Rectangle view
        {
            get { return new Rectangle((int)mapRect.X, (int)mapRect.Y, mapRect.X + screenRect.Width, mapRect.Y + screenRect.Height); }
        }

        private Vector2 cellNumber(GameObject player)
        {
            Vector2 result;
            result.X = screenRect.Width / 2 - player.bounds.Width / 2;
            result.Y = screenRect.Height / 2 - player.bounds.Height / 2;
            return result;
        }

        public void Update()
        {
            Move(cameraLightspot.position - cellNumber(cameraLightspot));
        }

        private void draw(SpriteBatch spriteBatch)
        {
            if (player.alive)
            {
                player.map.Draw(spriteBatch, this);
                player.DrawPlayerItems(spriteBatch, mapRect);
            }
        }

        public void DrawGrave(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Pictures\\grave"), new Rectangle(0, 0, screenRect.Width, screenRect.Height), Color.White);
        }

        public void CatchDraw(SpriteBatch spriteBatch)
        {
            if (player.alive)
            {
                //draw lights
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(lightsTarget);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Rectangle lightSize;
                lightSize.Width = 500;
                lightSize.Height = 500;
                lightSize.X = (int)player.position.X + player.bounds.Width / 2 - lightSize.Width / 2 - mapRect.X;
                lightSize.Y = (int)player.position.Y + player.bounds.Height / 2 - lightSize.Height / 2 - mapRect.Y;

                player.map.DrawLights(spriteBatch, this);
                spriteBatch.End();

                //catch camera drawings
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(renderTarget);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                draw(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                renderTarget = null;
            }
        }


        public Rectangle setCenter(Rectangle rect)
        {
            Rectangle result;
            result.X = view.Width / 2 - rect.Width / 2;
            result.Y = view.Height / 2 - rect.Height / 2;
            result.Width = rect.Width;
            result.Height = rect.Height;
            return result;
        }

        public Rectangle setSide(Rectangle rect, Corner corner)
        {
            switch (corner)
            {
                case Corner.topLeft:
                    return new Rectangle((int)mapRect.X, (int)mapRect.Y, rect.Width, rect.Height);

                case Corner.topRight:
                    return new Rectangle(view.Width - rect.Width, (int)mapRect.Y, rect.Width, rect.Height);

                case Corner.bottomLeft:
                    return new Rectangle((int)mapRect.X, view.Height - rect.Height, rect.Width, rect.Height);

                case Corner.bottomRight:
                    return new Rectangle(view.Width - rect.Width, view.Height - rect.Height, rect.Width, rect.Height);
            }
            return new Rectangle();
        }

        public bool InCamera(GameObject gameObject)
        {
            return gameObject.bounds.Intersects(mapRect);
        }

        public void Move(Vector2 destination)
        {
            mapRect.X = (int)MathHelper.Clamp(destination.X, 0, (player.map.tileMap.width - screenRect.Width / Tile.size) * Tile.size);//?
            mapRect.Y = (int)MathHelper.Clamp(destination.Y, 0, (player.map.tileMap.height - screenRect.Height / Tile.size) * Tile.size);
        }
    }
}
