using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class TextShadow
    {
        private Text source;
        private Vector2 layout;
        private float depth;
        public Game.WindowDepth WindowDepth
        {
            get
            {
                return Game.FloatToWindowDepth(depth);
            }
            set
            {
                depth = Game.DepthToFloat((int)value);
            }
        }

        public TextShadow(Text source, Vector2 layout)
        {
            this.source = source;
            this.layout = layout;

            WindowDepth = Game.WindowDepth.windowDataShadow;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawingPosition, SpriteFont font, float drawingOpacity)
        {
            if (source.visible)
            {
                spriteBatch.DrawString(font, source.text, drawingPosition + layout, Color.Black * (drawingOpacity / 7),
                    0f, Vector2.Zero, source.scale, SpriteEffects.None, depth);
            }
        }
    }
}
