using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Camera
    {
        public Map map;

        public Rectangle mapRect;
        public Rectangle screenRect;
        public Viewport viewport;
        public Matrix transform;
        private Vector2 scale = new Vector2(1, 1);

        public GameObject cameraLightspot;
        public Player player;

        public enum Corner { topLeft, topRight, bottomLeft, bottomRight }

        public Camera(Rectangle screenRect, Map map, GameObject cameraLightspot, Player player)
        {
            this.cameraLightspot = cameraLightspot;
            this.screenRect = screenRect;
            this.mapRect = screenRect;
            viewport = new Viewport(screenRect);
            transform = Matrix.CreateScale(new Vector3(scale.X, scale.Y, 0)) * Matrix.CreateTranslation(new Vector3(0, 0, 0));
            this.map = map;
            this.player = player;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (player.alive)
            {
                map.Draw(spriteBatch, this);
                player.DrawPlayerItems(spriteBatch, mapRect);
            }
            else
            {
                spriteBatch.Draw(Game.content.Load<Texture2D>("Textures\\Pictures\\grave"), new Rectangle(), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(Game.Depth.front));
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
            mapRect.X = (int)MathHelper.Clamp(destination.X, 0, (map.tileMap.width - screenRect.Width / Tile.size) * Tile.size);//?
            mapRect.Y = (int)MathHelper.Clamp(destination.Y, 0, (map.tileMap.height - screenRect.Height / Tile.size) * Tile.size);
        }
    }
}
