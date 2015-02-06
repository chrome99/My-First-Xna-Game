using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Picture : WindowItem
    {
        public override Vector2 drawingPosition()
        {
            if (source == null)
            {
                return position + thickness;
            }
            else
            {
                return position + thickness + source.position + source.thickness;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                spriteBatch.Draw(texture, drawingPosition(), drawingRect, Color.White * getOpacity, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
                
            }
        }
    }
}
