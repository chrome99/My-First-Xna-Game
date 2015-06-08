using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class ParticalManager
    {
        private Partical[] particalsArray;
        private Rectangle rect;

        private Snake snake;

        public ParticalManager(int maxParticals, Rectangle rect)
        {
            this.rect = rect;

            particalsArray = new Partical[maxParticals];
            snake = new Snake(100, new Rectangle(500, 500, 1, 1));
            snake.destinationsList.Add(new Vector2(200, 200));
            snake.destinationsList.Add(new Vector2(300, 100));
            snake.destinationsList.Add(new Vector2(500, 500));

            for (int i = 0; i < particalsArray.Length; i++)
            {
                particalsArray[i] = new Partical(new Rectangle(i * 5, i * 5, 5, 5), 1, new Color(0, 0, 255));
            }
        }

        public void Update()
        {
            snake.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            snake.Draw(spriteBatch);
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
