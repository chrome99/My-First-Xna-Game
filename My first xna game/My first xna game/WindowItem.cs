using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class WindowItem
    {
        public Vector2 position;
        public virtual Vector2 drawingPosition() { return position; }
        public bool visible = true;
        public float depth;
        public float originalOpacity;
        public float opacity = 100f;
        protected float getOpacity
        {
            get { return opacity / 100; }
        }
        public virtual Rectangle bounds
        {
            get { return new Rectangle(); }
        }
        public Window source;

        public WindowItem(Window source)
        {
            this.source = source;
            depth = Game.DepthToFloat(Game.Depth.windowsData);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenRect) { }
    }
}
