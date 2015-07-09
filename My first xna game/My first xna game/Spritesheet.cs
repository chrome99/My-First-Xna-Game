using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Spritesheet : Sprite
    {
        private bool showAnimation = false;
        public bool ShowAnimation
        {
            get { return showAnimation; }
            set { showAnimation = value; timerSwitch = value; }
        }

        private Rectangle cropingRect;
        private float timer = 0f;
        private bool timerSwitch = false;
        protected float interval = 200f;
        private int currentFrameX = 0;
        private int currentFrameY = 0;

        public Spritesheet(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            size.X = this.texture.Width / 4;
            size.Y = this.texture.Height / 4;
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (visible)
            {
                DrawPlayer(spriteBatch, offsetRect);
                Rectangle drawingRect = bounds;
                drawingRect.X = drawingRect.X - offsetRect.X;
                drawingRect.Y = drawingRect.Y - offsetRect.Y;
                spriteBatch.Draw(texture, drawingRect, cropingRect, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
            }
        }

        public virtual void DrawPlayer(SpriteBatch spriteBatch, Rectangle offsetRect) { }

        protected override void UpdateSpritesheet(GameTime gameTime)
        {
            UpdateAnyway();
            if (!alive) { return; }
            UpdateProjectile();
            UpdateActor();

            //-update spritesheet rect
            cropingRect = new Rectangle(currentFrameX * (int)size.X, currentFrameY * (int)size.Y, (int)size.X, (int)size.Y);

            //-update animation timer
            if (timerSwitch == true)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer > interval)
                {
                    currentFrameX++;
                    timer = 0f;
                    if (currentFrameX == 4)
                    {
                        currentFrameX = 0;
                        if (!showAnimation)
                        {
                            timerSwitch = false;
                        }
                        else
                        {
                            timer = 0f;
                        }
                    }
                }
            }

            UpdatePlayer(gameTime);
            
        }

        protected virtual void UpdateAnyway() { }
        protected virtual void UpdateProjectile() { }
        protected virtual void UpdatePlayer(GameTime gameTime) { }
        protected virtual void UpdateActor() { }

        public void StartAnimation(MovementManager.Direction Direction, bool showAnimation = true)
        {
            switch (Direction)
            {
                case MovementManager.Direction.down:
                    currentFrameY = 0;
                    break;

                case MovementManager.Direction.left:
                    currentFrameY = 1;
                    break;

                case MovementManager.Direction.right:
                    currentFrameY = 2;
                    break;

                case MovementManager.Direction.up:
                    currentFrameY = 3;
                    break;
            }
            if (showAnimation)
            {
                timerSwitch = true;
            }
        }

        protected void StopAnimation()
        {
            if (!ShowAnimation)
            {
                timerSwitch = false;
                currentFrameX = 0;
            }
        }

    }
}
