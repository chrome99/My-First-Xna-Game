using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class WindowItem
    {
        public Vector2 position;

        protected Vector2 GetDrawingPosition(Vector2 windowPosition)
        {
            return windowPosition + position + source.thickness;
        }

        protected Vector2 GetDrawingPositionWithoutSource(Rectangle offsetRect)
        {
            Vector2 newPosition;
            newPosition.X = position.X - offsetRect.X;
            newPosition.Y = position.Y - offsetRect.Y;
            return newPosition;
        }

        public bool visible = true;

        protected float depth;
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

        public float originalOpacityState;
        public float opacity = 100;
        public bool sourceCanDrawThis = true; //TODO: SRSLY?
        private bool fade = false;
        private bool fadeBack = false;
        private int fadeingOpacity;
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

        public WindowItem(Window source, bool hidden = false)
        {
            if (source != null)
            {
                if (hidden)
                {
                    source.AddHiddenItem(this);
                }
                else
                {
                    source.AddItem(this);
                }
            }

            WindowDepth = Game.WindowDepth.windowsData;
        }

        public bool Fade(int newOpacity = 0)
        {
            if (!fadeBack)
            {
                fade = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FadeBack(int newOpacity = 100)
        {
            if (!fade)
            {
                visible = true;
                fadeBack = true;
                fadeingOpacity = newOpacity;
                return true;
            }
            else
            {
                return false;
            }
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
                if (opacity <= 0)
                {
                    visible = false;
                    fade = false;
                    //// TODO: Destroy instance
                }
            }
            if (fadeBack)
            {
                if (fadeTimer.result && opacity < fadeingOpacity)
                {
                    opacity += 20;
                    fadeTimer.counter = 0f;
                }
                if (opacity >= fadeingOpacity)
                {
                    opacity = fadeingOpacity;
                    fadeBack = false;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 windowPosition) { }
        public virtual void DrawWithoutSource(SpriteBatch spriteBatch, Rectangle offsetRect) { }
    }
}
