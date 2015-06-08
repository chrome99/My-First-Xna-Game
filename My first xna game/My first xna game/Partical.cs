using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Partical
    {
        public delegate void ParticalUpdate();
        public Rectangle rect;
        public int speed;
        public Color color;
        public float opacity;

        public List<Vector2> destinationsList = new List<Vector2>();
        public int currentDestination = 0;

        private Texture2D texture;

        public Partical(Rectangle rect, int speed, Color color, float opacity = 1.0f)
        {
            this.rect = rect;
            this.speed = speed;
            this.color = color;
            this.opacity = opacity;

            texture = Game.content.Load<Texture2D>("Textures\\Sprites\\white dot");
        }

        public void Update()
        {
            if (destinationsList.Count == 0) { return; }

            if (currentDestination == destinationsList.Count)
            {
                currentDestination = 0;
            }

            for (int i = 0; i < destinationsList.Count; i++)
            {
                if (currentDestination == i)
                {
                    if (!GoTo(destinationsList[i]))
                    {
                        currentDestination++;
                    }
                }
            }
        }

        /*public void GetOffRectangle(Rectangle rect)
        {
            if (this.rect.X > rect.X && this.rect.X < rect.X + rect.Width)
            {
                if (this.rect.X > rect.X + rect.Width / 2)
                {
                    this.rect.X += speed;
                }
                else
                {
                    this.rect.X -= speed;
                }
            }

            if (this.rect.Y > rect.Y && this.rect.Y < rect.Y + rect.Height)
            {
                if (this.rect.Y > rect.Y + rect.Height / 2)
                {
                    this.rect.Y += speed;
                }
                else
                {
                    this.rect.Y -= speed;
                }
            }
        }*/

        public bool GoTo(Vector2 destination)
        {
            if (rect.X == destination.X && rect.Y == destination.Y)
            {
                return false;
            }

            if (rect.X < destination.X)
            {
                rect.X += speed;
            }
            if (rect.X > destination.X)
            {
                rect.X -= speed;
            }

            if (rect.Y < destination.Y)
            {
                rect.Y += speed;
            }
            if (rect.Y > destination.Y)
            {
                rect.Y -= speed;
            }

            return true;
        }

        public void GoTo(Rectangle rect)
        {
            if (this.rect.X < rect.X)
            {
                this.rect.X += speed;
            }
            if (this.rect.X + this.rect.Width > rect.X + rect.Width)
            {
                this.rect.X -= speed;
            }

            if (this.rect.Y < rect.Y)
            {
                this.rect.Y += speed;
            }
            if (this.rect.Y + this.rect.Height > rect.Y + rect.Height)
            {
                this.rect.Y -= speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, null, color * opacity);
        }
    }
}
