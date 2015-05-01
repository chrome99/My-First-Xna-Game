using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Picture : WindowItem
    {
        // TODO: A function's name should describe what it does, and should be a verb.
        public override Vector2 drawingPosition()
        {
            if (source == null)
            {
                return position;
            }
            else
            {
                return position + source.position + source.thickness;
            }
        }
        public Rectangle? drawingRect = null;
        private Texture2D texture;

        public Picture(Texture2D texture, Vector2 position, Window source)
            : base(source)
        {
            this.position = position;
            this.texture = texture;
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenRect)
        {
            if (visible)
            {
                Vector2 newPosition = drawingPosition();
                newPosition.X = newPosition.X + screenRect.X - offsetRect.X;
                newPosition.Y = newPosition.Y + screenRect.Y - offsetRect.Y;
                spriteBatch.Draw(texture, newPosition, drawingRect, Color.White * getOpacity, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
                
            }
        }
    }
}
