using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Text : WindowItem
    {
        private SpriteFont font;
        public string text;
        public Color color;

        public Text(SpriteFont font, Vector2 position, Color color, string text, Window source = null)
            : base(source)
        {
            this.position = position;
            this.font = font;
            this.color = color;
            this.text = text;
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y); }
        }

        public void Update(string text)
        {
            this.text = text;
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenRect)
        {
            if (visible)
            {
                Vector2 newPosition;
                if (source == null)
                {
                    newPosition = position;
                }
                else
                {
                    newPosition = position + source.position + source.thickness;
                }
                newPosition.X = newPosition.X + screenRect.X - offsetRect.X;
                newPosition.Y = newPosition.Y + screenRect.Y - offsetRect.Y;
                spriteBatch.DrawString(font, text, newPosition, color * getOpacity, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
            }
        }
    }
}
