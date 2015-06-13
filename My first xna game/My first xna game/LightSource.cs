using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class LightSource
    {
        private static Texture2D texture = Game.content.Load<Texture2D>("Textures\\Sprites\\lightmask");
        private GameObject source;
        private int defaultRaduis;
        private float raduis;
        private float raduisAnimationDifference;
        private bool subbedLevel = false;
        private Color color;
        private int opacity;
        private Rectangle lightRect
        {
            get
            {
                Rectangle result = new Rectangle((int)source.position.X + source.bounds.Width / 2 - (int)raduis / 2, (int)source.position.Y + source.bounds.Width / 2 - (int)raduis / 2, (int)raduis, (int)raduis);
                return result;
            }
        }

        public struct LightData
        {
            public int raduis;
            public int opacity;
            public Color color;
        }

        public LightData getSaveData()
        {
            return new LightData()
            {
                raduis = (int)this.raduis,
                opacity = this.opacity,
                color = this.color
            };
        }

        public void LoadData(LightData data)
        {
            raduis = data.raduis;
            opacity = data.opacity;
            color = data.color;
        }

        public LightSource(GameObject source, int raduis, int opacity, Color color)
        {
            this.source = source;
            this.raduis = raduis;
            this.opacity = opacity;
            this.color = color;

            defaultRaduis = raduis;
            raduisAnimationDifference = raduis - defaultRaduis * 0.9f;
        }

        public void SetSource(GameObject source)
        {
            this.source = source;
        }

        public void Update()
        {
            if (!subbedLevel)
            {
                if (raduis > defaultRaduis * 0.9f)
                {
                    raduis -= raduisAnimationDifference / 80;
                }
                else
                {
                    subbedLevel = true;
                }
            }
            else
            {
                if (raduis < defaultRaduis)
                {
                    raduis += raduisAnimationDifference / 80;
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
                spriteBatch.Draw(texture, drawingRect, color * (opacity / 100f));
            }
        }
    }
}
