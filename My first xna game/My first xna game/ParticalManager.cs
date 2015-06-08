using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class ParticalManager
    {
        private Partical[] snakes;
        private Rectangle rect;

        private Random random;

        public ParticalManager(int maxParticals, Rectangle rect)
        {
            this.rect = rect;

            random = new Random();
            snakes = new Partical[maxParticals];

            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i] = new Partical(new Rectangle(random.Next(Game.worldRect.Width), random.Next(Game.worldRect.Height), 4, 4), 1, new Color(255, 150 + random.Next(50), 50 - random.Next(50)));//i, 255, i   //random.Next(70) / 100f);
                Vector2 x = new Vector2(random.Next(Game.worldRect.Width), random.Next(Game.worldRect.Height));
                snakes[i].destinationsList.Add(x);
                snakes[i].destinationsList.Add(new Vector2(random.Next(Game.worldRect.Width), random.Next(Game.worldRect.Height)));
                snakes[i].destinationsList.Add(new Vector2(random.Next(Game.worldRect.Width), random.Next(Game.worldRect.Height)));
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

            /*foreach (Partical partical in particalsArray)
            {
                if (partical.rect.Intersects(rect))
                {
                    partical.Draw(spriteBatch);
                }
            }*/
        }
    }
}
