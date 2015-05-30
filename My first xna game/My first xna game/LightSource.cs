using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class LightSource
    {
        private static Texture2D texture = Game.content.Load<Texture2D>("Textures\\Sprites\\lightmask");
        private GameObject source;
        private int defaultLevel;
        private float level;
        private float levelAnimationDifference;
        private bool subbedLevel = false;
        private Color color;
        private Rectangle lightRect
        {
            get
            {
                Rectangle result = new Rectangle((int)source.position.X + source.bounds.Width / 2 - (int)level / 2, (int)source.position.Y + source.bounds.Width / 2 - (int)level / 2, (int)level, (int)level);
                return result;
            }
        }

        public LightSource(GameObject source, int level, Color color)
        {
            this.source = source;
            this.level = level;
            this.color = color;

            defaultLevel = level;
            levelAnimationDifference = level - defaultLevel * 0.9f;
        }

        public void Update()
        {
            if (!subbedLevel)
            {
                if (level > defaultLevel * 0.9f)
                {
                    level -= levelAnimationDifference / 80;
                }
                else
                {
                    subbedLevel = true;
                }
            }
            else
            {
                if (level < defaultLevel)
                {
                    level += levelAnimationDifference / 80;
                }
                else
                {
                    subbedLevel = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (source.alive)
            {
                Rectangle drawingRect = lightRect;
                drawingRect.X -= offsetRect.X;
                drawingRect.Y -= offsetRect.Y;
                spriteBatch.Draw(texture, drawingRect, color);
            }
        }
    }
}
