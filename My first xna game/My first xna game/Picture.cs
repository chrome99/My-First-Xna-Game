using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Picture : WindowItem
    {
        public Rectangle? drawingRect = null;
        public Texture2D texture;

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
                spriteBatch.Draw(texture, GetDrawingPosition(windowPosition), drawingRect, Color.White * drawingOpacity, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
            }
        }

        public override void DrawWithoutSource(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (visible)
            {
                spriteBatch.Draw(texture, GetDrawingPositionWithoutSource(offsetRect), drawingRect, Color.White * drawingOpacity, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
            }
        }
    }
}
