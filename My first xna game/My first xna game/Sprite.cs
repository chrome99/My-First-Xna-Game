using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Sprite : GameObject
    {
        public Texture2D texture;

        public float speed = 1;
        public float walkingSpeed = 2;
        public float knockbackSpeed = 6;
        public float runningSpeed = 4;
        public float GetSpeedByType(MovementManager.SpeedType speedType)
        {
            switch (speedType)
            {
                case MovementManager.SpeedType.walking:
                    return walkingSpeed;

                case MovementManager.SpeedType.knockback:
                    return knockbackSpeed;

                case MovementManager.SpeedType.running:
                    return runningSpeed;
            }
            return 0;
        }

        protected bool forceMoving = false;
        public List<MovementManager.MovementString> movementList = new List<MovementManager.MovementString>();

        public float depth { get; protected set; }
        public Game.MapDepth MapDepth
        {
            get
            {
                return Game.FloatToMapDepth(depth);
            }
            set
            {
                depth = Game.DepthToFloat((int)value);
            }
        }

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

        public Sprite(Texture2D texture, Vector2 position, Rectangle drawingCoordinates = new Rectangle())
            : base(position)
        {
            //intialize variables
            this.texture = texture;
            this.position = position;
            this.drawingCoordinates = drawingCoordinates;

            MapDepth = Game.MapDepth.player;

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
                    spriteBatch.Draw(texture, drawingRect, null, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
                }
                else
                {
                    spriteBatch.Draw(texture, drawingRect, drawingCoordinates, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
                }
            }
        }

        protected override void UpdateSprite(GameTime gameTime)
        {
            /*if (updated) { return; }
            updated = true;*/
            UpdateSpritesheet(gameTime);

            HandleMovementList();

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

        int movementCount = 0;
        private void HandleMovementList()
        {
            if (movementList.Count > 0)
            {
                forceMoving = true;
                int maxMovementCount = movementList[0].destination / (int)speed;

                if (movementCount < maxMovementCount)
                {
                    move(movementList[0].direction, (int)GetSpeedByType(movementList[0].speedType));
                    movementCount++;
                }
                else
                {
                    movementList.Remove(movementList[0]);
                    movementCount = 0;
                }
            }
            else
            {
                forceMoving = false; 
            }
        }

        protected virtual void move(MovementManager.Direction direction, int speed)
        {
            movementManager.MoveToDirection(this, direction, speed);
        }

        public override void FixOutsideCollision()
        {
            if (mapRect == new Rectangle()) { return; }
            Rectangle fixedMapRect = mapRect;
            fixedMapRect.Width -= bounds.Width;
            fixedMapRect.Height -= bounds.Height;
            if (position.X < fixedMapRect.X)
            {
                position.X = 0;
            }
            if (position.X > fixedMapRect.Width)
            {
                position.X = fixedMapRect.Width;
            }
            if (position.Y < fixedMapRect.Y)
            {
                position.Y = 0;
            }
            if (position.Y > fixedMapRect.Height)
            {
                position.Y = fixedMapRect.Height;
            }
        }

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
