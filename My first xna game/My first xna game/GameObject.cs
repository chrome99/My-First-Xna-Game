using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class GameObject
    {

        public Vector2 position;
        public Vector2 size = new Vector2(Tile.size, Tile.size);
        public List<string> tags = new List<string>();

        public CollisionManager.CollisionFunction collisionFunction;
        public CollisionManager.InteractionFunction interactFunction;
        public bool passable = false;
        public bool canCollide = true;

        public MovementManager.Direction view;
        public bool alive = true;

        private LightSource lightSource;
        public LightSource getLightSource
        {
            get { return lightSource; }
        }

        public MovementManager movementManager;
        public Rectangle mapRect;

        public bool updated = false;

        public GameObject() { }
        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public virtual Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
        }

        private Rectangle? realCore = null;
        public virtual Rectangle core
        {
            get
            {
                if (realCore == null)
                {
                    return bounds;
                }
                Rectangle result = (Rectangle)realCore;
                result.X += (int)position.X;
                result.Y += (int)position.Y;
                return result;
            }
            set
            {
                realCore = value;
            }
        }

        public virtual void Kill()
        {
            canCollide = false;
            alive = false;
        }

        public virtual void Revive()
        {
            canCollide = true;
            alive = true;
        }

        public void AddLight(int level, Color color, int opacity = 100)
        {
            lightSource = new LightSource(this, level, opacity, color);
        }

        public void Update(GameTime gameTime)
        {
            UpdateSprite(gameTime);
        }

        public virtual void FixOutsideCollision()
        {
            if (mapRect == new Rectangle()) { return; }
            if (position.X < mapRect.X)
            {
                position.X = 0;
            }
            if (position.X > mapRect.Width)
            {
                position.X = mapRect.Width;
            }
            if (position.Y < mapRect.Y)
            {
                position.Y = 0;
            }
            if (position.Y > mapRect.Height)
            {
                position.Y = mapRect.Width;
            }
        }

        protected virtual void UpdateSprite(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle offsetRect) { }
    }
}