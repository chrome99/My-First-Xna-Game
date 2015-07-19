using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Picture : WindowItem
    {
        public Rectangle? fileDrawingRect = null;
        public Vector2 drawingBoundaries = Vector2.Zero;
        public Texture2D texture;
        public float rotation = 0f;

        public Picture(Texture2D texture, Vector2 position, Window source, bool hidden = false)
            : base(source, hidden)
        {
            this.position = position;
            this.texture = texture;
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 windowPosition)
        {
            if (visible)
            {
                spriteBatch.Draw(texture, GetDrawingPosition(windowPosition), fileDrawingRect, Color.White * drawingOpacity, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
            }
        }

        public override void DrawWithoutSource(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (visible)
            {
                if (drawingBoundaries != Vector2.Zero)
                {
                    Vector2 drawingPosition = GetDrawingPositionWithoutSource(offsetRect);
                    Rectangle drawingRect = new Rectangle((int)drawingPosition.X, (int)drawingPosition.Y, (int)drawingBoundaries.X, (int)drawingBoundaries.Y);
                    spriteBatch.Draw(texture, drawingRect, fileDrawingRect, Color.White * drawingOpacity, rotation, Vector2.Zero, SpriteEffects.None, depth);
                }
                else
                {
                    spriteBatch.Draw(texture, GetDrawingPositionWithoutSource(offsetRect), fileDrawingRect, Color.White * drawingOpacity, rotation, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
                }
            }
        }
    }
}
