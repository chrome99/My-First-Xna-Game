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

        public bool passable = false;
        public bool canCollide = true;
        public CollisionManager.CollisionFunction collisionFunction;
        public Item[] collisionParameters = new Item[10];
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
        public Vector2 coreCollision = new Vector2(1, 1);

        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public virtual Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
        }

        public virtual Rectangle core
        {
            get
            {
                Rectangle result;
                result = bounds;
                result.Width /= (int)coreCollision.X;
                result.Height /= (int)coreCollision.Y;
                return result;
            }
        }

        public int GetID(List<GameObject> gameObjectList)
        {
            for (int counter = 0; counter < gameObjectList.Count; counter++)
            {
                if (gameObjectList[counter].Equals(this))
                {
                    return counter;
                }
            }
            throw new System.ArgumentException("Parameter wasn't found.", "original");
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

            UpdateOutSideCollision();

            if (collisionFunction != null)
            {
                collisionFunction(this);
            }
        }

        private void UpdateOutSideCollision()
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