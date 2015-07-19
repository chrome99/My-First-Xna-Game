using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class LightSource : Light
    {
        private int defaultRaduis;
        private float raduis;
        private float raduisAnimationDifference;
        private bool subbedLevel = false;
        private Color color;
        public Vector2 position;
        private int opacity;
        private Rectangle lightRect
        {
            get
            {
                return new Rectangle((int)source.position.X + (int)position.X + source.bounds.Width / 2 - (int)raduis / 2, (int)source.position.Y + (int)position.Y + source.bounds.Width / 2 - (int)raduis / 2, (int)raduis, (int)raduis); ;
            }
        }

        public LightSource(int raduis, Color color, int opacity = 100)
        {
            this.raduis = raduis;
            this.opacity = opacity;
            this.color = color;

            position = Vector2.Zero;

            defaultRaduis = raduis;
            raduisAnimationDifference = raduis - defaultRaduis * 0.9f;
        }

        public override void Update()
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

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (source.alive)
            {
                Rectangle drawingRect = lightRect;
                drawingRect.X -= offsetRect.X;
                drawingRect.Y -= offsetRect.Y;
                spriteBatch.Draw(Light.texture, drawingRect, color * (opacity / 100f));
            }
        }
    }
}
