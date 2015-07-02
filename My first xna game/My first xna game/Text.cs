using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Text : WindowItem
    {
        public string text;
        public float scale = 1.0f;
        public Color color;
        private SpriteFont font;
        private TextShadow shadow;

        private bool changeColor;
        private bool returnToOriginalColor;
        private Color originalColorState;
        private Color newColor;
        private Timer changeColorTimer;

        public Text(SpriteFont font, Vector2 position, Color color, string text, Window source = null, Vector2 shadowPosition = new Vector2(), bool hidden = false)
            : base(source, hidden)
        {
            this.position = position;
            this.font = font;
            this.color = color;
            this.text = text;

            if (shadowPosition != new Vector2())
            {
                shadow = new TextShadow(this, shadowPosition);
            }
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y); }
        }

        public void UpdateTextChangeColor()
        {
            //change to red effect
            if (changeColor)
            {
                if (ChangeColor(newColor))
                {
                    changeColorTimer.Active();
                    changeColor = false;
                }

            }

            if (changeColorTimer != null)
            {
                if (changeColorTimer.result)
                {
                    returnToOriginalColor = true;
                    changeColorTimer = null;
                }
            }

            if (returnToOriginalColor)
            {
                if (ChangeColor(originalColorState))
                {
                    returnToOriginalColor = false;
                }
            }
        }

        public void UpdateTextString(string text = null)
        {
            if (text != null)
            {
                this.text = text;
            }
        }

        private bool ChangeColor(Color newColor)
        {
            if ((color.R > newColor.R - 25 && color.R < newColor.R + 25) &&
                (color.G > newColor.G - 25 && color.G < newColor.G + 25) &&
                (color.B > newColor.B - 25 && color.B < newColor.B + 25))
            {
                color = newColor;
                return true;
            }
            else
            {
                if (color.R > newColor.R)
                {
                    color.R -= 25;
                }
                else if (color.R < newColor.R)
                {
                    color.R += 25;
                }

                if (color.G > newColor.G)
                {
                    color.G -= 25;
                }
                else if (color.G < newColor.G)
                {
                    color.G += 25;
                }

                if (color.B > newColor.B)
                {
                    color.B -= 25;
                }
                else if (color.B < newColor.B)
                {
                    color.B += 25;
                }
                return false;
            }
        }

        public void ChangeColorEffect(Color newColor, float time)
        {
            if (changeColor || changeColorTimer != null || returnToOriginalColor) { return; }
            changeColorTimer = new Timer(time, false);
            originalColorState = color;
            this.newColor = newColor;
            changeColor = true;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 windowPosition)
        {
            if (visible)
            {
                Vector2 drawingPosition = GetDrawingPosition(windowPosition);
                if (shadow != null)
                {
                    shadow.Draw(spriteBatch, drawingPosition, font, drawingOpacity);
                }
                spriteBatch.DrawString(font, text, drawingPosition, color * drawingOpacity, 0f, Vector2.Zero, scale, SpriteEffects.None, depth);
            }
        }

        public override void DrawWithoutSource(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (visible)
            {
                Vector2 drawingPosition = GetDrawingPositionWithoutSource(offsetRect);
                if (shadow != null)
                {
                    shadow.Draw(spriteBatch, drawingPosition, font, drawingOpacity);
                }
                spriteBatch.DrawString(font, text, drawingPosition, color * drawingOpacity, 0f, Vector2.Zero, scale, SpriteEffects.None, depth);
            }
        }
    }
}