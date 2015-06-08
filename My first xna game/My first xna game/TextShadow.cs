using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class TextShadow
    {
        private Text source;
        private Vector2 layout;

        public TextShadow(Text source, Vector2 layout)
        {
            this.source = source;
            this.layout = layout;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawingPosition, SpriteFont font, float drawingOpacity)
        {
            if (source.visible)
            {
                spriteBatch.DrawString(font, source.text, drawingPosition + layout, Color.Black * (drawingOpacity / 7),
                    0f, Vector2.Zero, 1.0f, SpriteEffects.None, Game.DepthToFloat(Game.Depth.windowDataShadow));
            }
        }
    }
}
