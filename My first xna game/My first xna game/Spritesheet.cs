﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace My_first_xna_game
{
    public class Spritesheet : Sprite
    {
        private Rectangle rect;
        private float timer = 0f;
        private bool timerSwitch = false;
        protected float interval = 200f;
        private int currentFrameX = 0;
        private int currentFrameY = 0;

        public Spritesheet(Texture2D texture, Vector2 position, Game.Depth depth, float speed)
            : base(texture, position, depth, speed)
        {
            size.X = this.texture.Width / 4;
            size.Y = this.texture.Height / 4;
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width / 4, texture.Height / 4); }
        }

        public override Rectangle core
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width / 4, texture.Height / 4); }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            if (visible)
            {
                Rectangle drawingRect = bounds;
                drawingRect.X = screenPosition.X + drawingRect.X - offsetRect.X;
                drawingRect.Y = screenPosition.Y + drawingRect.Y - offsetRect.Y;
                spriteBatch.Draw(texture, drawingRect, rect, Color.White * getOpacity, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(depth));
            }
        }

        public override void UpdateSpritesheet(GameTime gameTime)
        {
            if (!alive) { return; }
            UpdateProjectile();
            UpdateActor();

            //-update spritesheet rect
            rect = new Rectangle(currentFrameX * (int)size.X, currentFrameY * (int)size.Y, (int)size.X, (int)size.Y);

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
                        timerSwitch = false;
                    }
                }
            }
            
        }

        protected virtual void UpdateProjectile() { }

        protected virtual void UpdateActor() { }

        public void StartAnimation(MovementManager.Direction Direction, bool animation = true)
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
            if (animation)
            {
                timerSwitch = true;
            }
        }

        protected void StopAnimation()
        {
            timerSwitch = false;
            currentFrameX = 0;
        }

    }
}