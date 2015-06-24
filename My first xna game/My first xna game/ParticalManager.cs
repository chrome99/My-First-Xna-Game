using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class ParticalManager
    {
        private Partical[] snakes;
        private Random random = new Random();

        public ParticalManager(int maxParticals, Rectangle rect, Vector2 particalSize, int randomParticalSize, int speed, Color color, Color randomColor, int opacity, int randomOpacity)
        {
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
                snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height)));
                snakes[i].destinationsList.Add(new Vector2(rect.X + random.Next(rect.Width), rect.Y + random.Next(rect.Height)));
                snakes[i].destinationsList.Add(startingPoint);
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
