using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class WindowItem
    {
        public Vector2 position;
        public virtual Vector2 GetDrawingPosition() { return position; }
        public bool visible = true;
        public float depth;
        public float originalOpacityState;
        public float opacity = 100f;
        public bool sourceCanDrawThis = true; //TODO: SRSLY?
        private bool fade = false;
        private Timer fadeTimer = new Timer(50f);

        protected float drawingOpacity
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

        public void Fade()
        {
            fade = true;
        }

        public void UpdateFade()
        {
            if (fade)
            {
                if (fadeTimer.result && opacity > 0)
                {
                    opacity -= 20;
                    fadeTimer.counter = 0f;
                }
                if (opacity == 0)
                {
                    visible = false;
                    //// TODO: Destroy instance
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle offsetRect) { }
    }
}
