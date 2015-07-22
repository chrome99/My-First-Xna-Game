using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Partical
    {
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

            texture = Game.content.Load<Texture2D>("Textures\\Sprites\\particle");
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

        public bool SimulateGoTo(Vector2 startingPoint, Vector2 destination, out Vector2 result)
        {
            result.X = startingPoint.X;
            result.Y = startingPoint.Y;

            if (((startingPoint.X - destination.X >= 0 && startingPoint.X - destination.X < speed) || (startingPoint.X - destination.X <= 0 && startingPoint.X - destination.X > speed * -1))
                && ((startingPoint.Y - destination.Y >= 0 && startingPoint.Y - destination.Y < speed) || (startingPoint.Y - destination.Y <= 0 && startingPoint.Y - destination.Y > speed * -1)))
            {
                return false;
            }

            if (startingPoint.X < destination.X)
            {
                result.X += speed;
            }
            if (startingPoint.X > destination.X)
            {
                result.X -= speed;
            }

            if (startingPoint.Y < destination.Y)
            {
                result.Y += speed;
            }
            if (startingPoint.Y > destination.Y)
            {
                result.Y -= speed;
            }

            return true;
        }

        private bool GoTo(Vector2 destination)
        {
            Vector2 wayResult;
            bool functionResult = SimulateGoTo(new Vector2(rect.X, rect.Y), destination, out wayResult);
            rect.X = (int)wayResult.X;
            rect.Y = (int)wayResult.Y;
            return functionResult;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, null, color * opacity);
        }
    }
}
