using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Camera
    {
        private GraphicsDeviceManager graphicsDeviceManager;

        public Rectangle mapRect;
        public Rectangle screenRect;
        public Viewport viewport;
        public Viewport minimapViewport;

        public Matrix minimapTransform;
        public RenderTarget2D renderTarget;
        public Effect effect;
        public RenderTarget2D lightsTarget;
        public Vector2 scale = new Vector2(1, 1);
        private Vector2 minimapScale = new Vector2(0.2f, 0.2f);

        public GameObject cameraLightspot;
        public Player player;

        public enum Corner { topLeft, topRight, bottomLeft, bottomRight }

        public Camera(GraphicsDeviceManager graphicsDeviceManager, Rectangle screenRect, GameObject cameraLightspot, Player player)
        {
            this.cameraLightspot = cameraLightspot;
            this.screenRect = screenRect;
            this.mapRect = screenRect;
            this.graphicsDeviceManager = graphicsDeviceManager;
            this.player = player;
            player.AssignToCamera(this);

            viewport = new Viewport(screenRect);

            float minimapRectWidth = screenRect.Width * minimapScale.X;
            float minimapRectHeight = screenRect.Height * minimapScale.Y;
            minimapViewport = new Viewport(new Rectangle(screenRect.X, screenRect.Y, (int)minimapRectWidth, (int)minimapRectHeight));
            minimapTransform = Matrix.CreateScale(new Vector3(minimapScale.X, minimapScale.Y, 0)) * Matrix.CreateTranslation(new Vector3(0, 0, 0));

            renderTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, Game.worldRect.Width, Game.worldRect.Height);
            lightsTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, Game.worldRect.Width, Game.worldRect.Height);

            effect = Game.content.Load<Effect>("Effects\\FirstOne");
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
            result.X /= scale.X;
            result.Y /= scale.Y;
            return result;
        }

        public void Update()
        {
            //Move(cameraLightspot.position - cellNumber(cameraLightspot));

            int speed = 2;
            Sprite sprite = cameraLightspot as Sprite;
            if (sprite != null)
            {
                speed = (int)sprite.speed - 1;
            }

            Vector2 cameraLightspotPosition = new Vector2((int)cameraLightspot.position.X - (int)cellNumber(cameraLightspot).X, (int)cameraLightspot.position.Y - (int)cellNumber(cameraLightspot).Y);
            Vector2 mapPosition = new Vector2((int)mapRect.X, (int)mapRect.Y);

            if (!(cameraLightspotPosition.X - mapPosition.X >= 0 && cameraLightspotPosition.X - mapPosition.X <= speed))
            {
                if (MathHelper.Clamp(cameraLightspot.position.X - cellNumber(cameraLightspot).X, 0, cameraLightspot.position.X) > mapRect.X)
                {
                    Move(new Vector2(mapRect.X + speed, mapRect.Y));
                }
                else if (MathHelper.Clamp(cameraLightspot.position.X - cellNumber(cameraLightspot).X, 0, cameraLightspot.position.X) < mapRect.X)
                {
                    Move(new Vector2(mapRect.X - speed, mapRect.Y));
                }
            }

            if (!(cameraLightspotPosition.Y - mapPosition.Y >= 0 && cameraLightspotPosition.Y - mapPosition.Y <= speed))
            {
                if (MathHelper.Clamp(cameraLightspot.position.Y - cellNumber(cameraLightspot).Y, 0, cameraLightspot.position.Y) > mapRect.Y)
                {
                    Move(new Vector2(mapRect.X, mapRect.Y + speed));
                }
                else if (MathHelper.Clamp(cameraLightspot.position.Y - cellNumber(cameraLightspot).Y, 0, cameraLightspot.position.Y) < mapRect.Y)
                {
                    Move(new Vector2(mapRect.X, mapRect.Y - speed));
                }
            }
        }

        private void DrawLow(SpriteBatch spriteBatch)
        {
            if (player.alive)
            {
                player.map.Draw(spriteBatch, this, true);
            }
        }

        public void DrawHigh(SpriteBatch spriteBatch)
        {
            if (player.alive)
            {
                player.map.Draw(spriteBatch, this, false);
            }
        }

        public void DrawWindows(SpriteBatch spriteBatch)
        {
            if (player.alive)
            {
                player.DrawPlayerItems(spriteBatch, mapRect);
                foreach (GameObject gameObject in player.map.gameObjectList)
                {
                    Hostile hostile = gameObject as Hostile;
                    if (hostile != null)
                    {
                        hostile.DrawDmg(spriteBatch, mapRect);
                    }
                }
            }
        }

        public void DrawMinimap(SpriteBatch spriteBatch)
        {
            DrawLow(spriteBatch);
            DrawHigh(spriteBatch);
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
                player.map.DrawLights(spriteBatch, this);
                spriteBatch.End();

                //catch camera drawings
                graphicsDeviceManager.GraphicsDevice.SetRenderTarget(renderTarget);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                DrawLow(spriteBatch);
                DrawHigh(spriteBatch);
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
            mapRect.X = (int)MathHelper.Clamp(destination.X, 0, (player.map.width - screenRect.Width / scale.X / Tile.size) * Tile.size);
            mapRect.Y = (int)MathHelper.Clamp(destination.Y, 0, (player.map.height - screenRect.Height / scale.Y / Tile.size) * Tile.size);
        }
    }
}
