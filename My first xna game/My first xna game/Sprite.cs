using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Sprite : GameObject
    {
        public Texture2D texture;
        public float speed;
        public Game.Depth depth;
        public bool visible = true;
        protected bool fade = false;
        public float opacity = 100f;
        public float drawingOpacity
        {
            get { return opacity / 100; }
        }
        protected Timer fadeTimer = new Timer(50f);
        
        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
        }

        public Rectangle drawingCoordinates;

        public Sprite(Texture2D texture, Vector2 position, Game.Depth depth, float speed = 2f, Rectangle drawingCoordinates = new Rectangle())
            : base(position)
        {
            //intialize variables
            this.texture = texture;
            this.position = position;
            this.depth = depth;
            this.speed = speed;
            this.drawingCoordinates = drawingCoordinates;

            //intialize size
            if (drawingCoordinates.Equals(new Rectangle()))
            {
                size.X = this.texture.Width;
                size.Y = this.texture.Height;
            }
            else
            {
                size.X = drawingCoordinates.Width;
                size.Y = drawingCoordinates.Height;
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (visible)
            {
                Rectangle drawingRect = bounds;
                drawingRect.X = drawingRect.X - offsetRect.X;
                drawingRect.Y = drawingRect.Y - offsetRect.Y;

                if (drawingCoordinates.Equals(new Rectangle()))
                {
                    spriteBatch.Draw(texture, drawingRect, null, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(depth));
                }
                else
                {
                    spriteBatch.Draw(texture, drawingRect, drawingCoordinates, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(depth));
                }
            }
        }

        protected override void UpdateSprite(GameTime gameTime)
        {
            /*if (updated) { return; }
            updated = true;*/
            UpdateSpritesheet(gameTime);

            if (fade)
            {
                if (fadeTimer.result && opacity > 0)
                {
                    opacity -= 20;
                    fadeTimer.counter = 0f;
                }
                if (opacity == 0)
                {
                    Kill();
                }
            }
        }

        protected virtual void UpdateSpritesheet(GameTime gameTime) { }

        public void Reset()
        {
            movementManager.MoveTo(this, new Vector2(10 * 32, 30 * 32));
        }

        public void Fade()
        {
            fade = true;
        }

        public override void Kill()
        {
            canCollide = false;
            visible = false;
            alive = false;
        }

        public override void Revive()
        {
            canCollide = true;
            visible = true;
            alive = true;
        }

    }
}
