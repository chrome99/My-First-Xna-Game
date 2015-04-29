using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Sprite : GameObject
    {
        public Texture2D texture { get; set; }
        public float speed;
        public Game.Depth depth;
        public bool visible = true;
        public bool fade = false;
        public float opacity = 100f;
        public float getOpacity
        {
            get { return opacity / 100; }
        }
        protected Timer fadeTimer = new Timer(50f);
        
        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public override Rectangle core
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public Sprite(Texture2D texture, Vector2 position, Game.Depth depth, float speed = 2f)
            : base(position)
        {
            //intialize variables
            this.texture = texture;
            this.position = position;
            this.depth = depth;
            this.speed = speed;

            //intialize size
            size.X = this.texture.Width;
            size.Y = this.texture.Height;
        }
        
        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            if (visible)
            {
                Rectangle drawingPosition = bounds;
                drawingPosition.X = screenPosition.X + drawingPosition.X - offsetRect.X;
                drawingPosition.Y = screenPosition.Y + drawingPosition.Y - offsetRect.Y;
                
                spriteBatch.Draw(texture, drawingPosition, null, Color.White * getOpacity, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(depth));
            }
        }

        public override void Update(GameTime gameTime)
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

        public virtual void UpdateSpritesheet(GameTime gameTime) { }

        public override void Kill()
        {
            collision = false;
            visible = false;
            alive = false;
        }

        public override void Revive()
        {
            collision = true;
            visible = true;
            alive = true;
        }

    }
}
