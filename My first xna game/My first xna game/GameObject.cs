using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace My_first_xna_game
{
    public class GameObject
    {

        public Vector2 position;
        public Vector2 size = new Vector2(Tile.size, Tile.size);
        public List<string> tags = new List<string>();

        public bool passable;
        public bool canCollide = true;
        public CollisionManager.CollisionFunction collisionFunction;

        public MovementManager.Direction view;
        public bool alive = true;
        public MovementManager movementManager;
        public bool updated = false;

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
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
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

        public void Update(GameTime gameTime)
        {
            UpdateSprite(gameTime);

            if (collisionFunction != null)
            {
                collisionFunction(this);
            }
        }

        protected virtual void UpdateSprite(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition) { }

        public void Reset()
        {
            movementManager.MoveTo(this, new Vector2(10 * 32, 30 * 32));
        }
   }
}
