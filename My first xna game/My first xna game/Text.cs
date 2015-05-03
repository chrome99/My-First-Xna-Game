using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Text : WindowItem
    {
        public bool changeToRed = false;
        public string text;
        public Color color;
        private SpriteFont font;

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
            if (text != null)
            {
                this.text = text;
            }

            //change to red effect
            if (changeToRed)
            {
                if (color.R > 200 && color.G < 50 && color.B < 50)
                {
                    color = new Color(255, 0, 0);
                    changeToRed = false;
                }
                else
                {
                    if (color.R != 255)
                    {
                        color.R += 25;
                    }

                    if (color.G != 0)
                    {
                        color.G -= 25;
                    }

                    if (color.B != 0)
                    {
                        color.B -= 25;
                    }
                }
            }
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
                spriteBatch.DrawString(font, text, newPosition, color * drawingOpacity, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
            }
        }
    }
}
