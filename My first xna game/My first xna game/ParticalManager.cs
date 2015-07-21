using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class ParticalManager
    {
        public enum ParticalsMovement { xy, y, x }
        private Partical[] snakes;
        private Random random = new Random();
        private Rectangle rect;
        private ParticalsMovement particalsMovement;
        public List<Rectangle> holesList = new List<Rectangle>();

        public ParticalManager(ParticalsMovement particalsMovement, int maxParticals, Rectangle rect, Vector2 particalSize, int randomParticalSize, int speed, Color color, Color randomColor, int opacity, int randomOpacity)
        {
            this.rect = rect;
            this.particalsMovement = particalsMovement;

            snakes = new Partical[maxParticals];

            int randomColorR = 0;
            int randomColorG = 0;
            int randomColorB = 0;
            if (color.R > randomColor.R)
            {
                randomColorR = randomColor.R - color.R;
            }
            if (color.R < randomColor.R)
            {
                randomColorR = randomColor.R - color.R;
            }

            if (color.G > randomColor.G)
            {
                randomColorG = randomColor.G - color.G;
            }
            if (color.G < randomColor.G)
            {
                randomColorG = randomColor.G - color.G;
            }

            if (color.B > randomColor.B)
            {
                randomColorB = randomColor.B - color.B;
            }
            if (color.B < randomColor.B)
            {
                randomColorB = randomColor.B - color.B;
            }

            for (int i = 0; i < snakes.Length; i++)
            {
                int randomSize;
                if (randomParticalSize < 0)
                {
                    randomSize = random.Next(randomParticalSize * -1);
                    randomParticalSize *= -1;
                }
                else
                {
                    randomSize = random.Next(randomParticalSize);
                }
                Vector2 startingPoint = new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height));
                snakes[i] = new Partical(new Rectangle((int)startingPoint.X, (int)startingPoint.Y, (int)particalSize.X + randomSize, (int)particalSize.Y + randomSize), speed, new Color(color.R + RandomColor(randomColorR), color.G + RandomColor(randomColorG), color.B + RandomColor(randomColorB)), (opacity + random.Next(randomOpacity)) / 100f);//i, 255, i   //random.Next(70) / 100f);
                switch (particalsMovement)
                {
                    case ParticalsMovement.xy:
                        snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height)));
                        snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height)));
                        snakes[i].destinationsList.Add(startingPoint);
                        break;

                    case ParticalsMovement.y:
                        snakes[i].destinationsList.Add(new Vector2(startingPoint.X, rect.Y + random.Next(rect.Height)));
                        snakes[i].destinationsList.Add(new Vector2(startingPoint.X, rect.Y + random.Next(rect.Height)));
                        snakes[i].destinationsList.Add(startingPoint);
                        break;

                    case ParticalsMovement.x:
                        snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), startingPoint.Y));
                        snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), startingPoint.Y));
                        snakes[i].destinationsList.Add(startingPoint);
                        break;
                }
            }
        }

        public void AddHole(Rectangle holeRect)
        {
            holeRect.X += rect.X;
            holeRect.Y += rect.Y;
            holesList.Add(holeRect);
            UpdateHolesList();
        }

        public bool RemoveHole(Rectangle holeRect)
        {
            bool result = holesList.Remove(holeRect);
            if (result)
            {
                UpdateHolesList();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateHolesList()
        {
            foreach (Partical snake in snakes)
            {
                Vector2 startingPoint = new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height));
                snake.destinationsList.Clear();
                AddDestinationAndAvoidHoles(snake, startingPoint);
                AddDestinationAndAvoidHoles(snake, startingPoint);
                AddDestinationAndAvoidHoles(snake, startingPoint);
            }
        }

        private void AddDestinationAndAvoidHoles(Partical partical, Vector2 startingPoint)
        {
            Vector2 randomDestination;
            switch (particalsMovement)
            {
                case ParticalsMovement.xy:
                    randomDestination = new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height));
                    break;

                case ParticalsMovement.y:
                    randomDestination = new Vector2(startingPoint.X, rect.Y + random.Next(rect.Height));
                    break;

                case ParticalsMovement.x:
                    randomDestination = new Vector2(rect.X + random.Next(rect.Width), startingPoint.Y);
                    break;

                default:
                    randomDestination = Vector2.Zero;
                    break;
            }
            Rectangle particalRect = new Rectangle((int)randomDestination.X, (int)randomDestination.Y, partical.rect.Width, partical.rect.Height);
            bool dosentIntersect = true;
            foreach (Rectangle hole in holesList)
            {
                if (particalRect.Intersects(hole))
                {
                    dosentIntersect = false;
                    AddDestinationAndAvoidHoles(partical, startingPoint);
                }
            }
            if (dosentIntersect)
            {
                partical.destinationsList.Add(randomDestination);
            }
        }

        private int RandomColor(int seed)
        {
            if (seed < 0)
            {
                return random.Next(seed * -1) *-1;
            }
            else
            {
                return random.Next(seed);
            }
        }

        public void NewRect(Rectangle rect)
        {
            Random random = new Random();
            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i].destinationsList.Clear();
                snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height)));
                snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height)));
                snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height)));
            }
        }

        public void Update()
        {
            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i].Draw(spriteBatch);
            }
        }
    }
}
