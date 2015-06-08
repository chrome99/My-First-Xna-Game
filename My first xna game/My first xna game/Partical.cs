using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Partical
    {
        public delegate void ParticalUpdate();

        public ParticalUpdate UpdateMovement;
        public Rectangle rect;
        public int speed;
        public Color color;

        private Texture2D texture;

        public Partical(Rectangle rect, int speed, Color color)
        {
            this.rect = rect;
            this.speed = speed;
            this.color = color;

            texture = Game.content.Load<Texture2D>("Textures\\Sprites\\white dot");
        }

        public void Update()
        {
            if (UpdateMovement != null)
            {
                UpdateMovement();
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
            if (rect.X == destination.X - rect.Width && rect.Y == destination.Y - rect.Height)
            {
                return false;
            }

            if (rect.X < destination.X)
            {
                rect.X += speed;
            }
            if (rect.X + rect.Width > destination.X)
            {
                rect.X -= speed;
            }

            if (rect.Y < destination.Y)
            {
                rect.Y += speed;
            }
            if (rect.Y + rect.Height > destination.Y)
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
            spriteBatch.Draw(texture, rect, null, color);
        }
    }
}
