using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Window : Sprite
    {
        public static Rectangle windowRect = new Rectangle(0, 0, 128, 128);
        public List<WindowItem> itemsList = new List<WindowItem>();
        public Vector2 thickness = new Vector2(10, 10);
        public bool offsetRect = true;
        private int width;
        private int height;

        // TODO: Make an opacity value for each state
        private float originalOpacity;
        private float subOpacity = 50f;
        private Player player;
        private bool playerCollision = false;

        public Window(Texture2D texture, Vector2 position, int width, int height, Player player = null)
            : base(texture, position, Game.Depth.windows, 0f)
        {
            this.width = width;
            this.height = height;
            this.player = player;
          

            originalOpacity = opacity;
            passable = true;
        }
        public static Vector2 GetWindowAbove(Rectangle positionBounds, Window window)
        {
            //set center
            Vector2 newPosition;
            newPosition.X = positionBounds.X + positionBounds.Width / 2 - window.bounds.Width / 2;
            newPosition.Y = positionBounds.Y + positionBounds.Height / 2 - window.bounds.Height / 2;

            //set above
            newPosition.Y = newPosition.Y - positionBounds.Height / 2 - window.bounds.Height / 2;

            return newPosition;
        }

        public void SetWindowAbove(Rectangle positionBounds)
        {
            //set center
            Vector2 newPosition;
            newPosition.X = positionBounds.X + positionBounds.Width / 2 - bounds.Width / 2;
            newPosition.Y = positionBounds.Y + positionBounds.Height / 2 - bounds.Height / 2;

            //set above
            newPosition.Y = newPosition.Y - positionBounds.Height / 2 - bounds.Height / 2;

            position = newPosition;
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
        }
        public override Rectangle core
        {
            get { return new Rectangle((int)position.X, (int)position.Y, width, height); }
        }

        public void AddItem(WindowItem item)
        {
            itemsList.Add(item);
            item.source = this;
        }

        public void setCorner(Camera camera, Camera.Corner corner)
        {
            Rectangle side = camera.setSide(bounds, corner);
            position.X = side.X;
            position.Y = side.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            if (visible && alive)
            {
                //draw window
                Rectangle drawingPosition = bounds;
                drawingPosition.X = screenPosition.X + drawingPosition.X - offsetRect.X;
                drawingPosition.Y = screenPosition.Y + drawingPosition.Y - offsetRect.Y;

                spriteBatch.Draw(texture, drawingPosition, windowRect, Color.White * getOpacity, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(depth));
                int a = itemsList.Count;
                //draw items
                foreach (WindowItem item in itemsList)
                {
                    if (item is Text || item is Picture)
                    {
                        item.Draw(spriteBatch, offsetRect, screenPosition);
                    }
                    
                }
            }
        }

        protected override void UpdateSprite(GameTime gameTime)
        {
            //fade
            if (fade)
            {
                if (fadeTimer.result && opacity > 0)
                {
                    opacity -= 20;
                    foreach (WindowItem item in itemsList)
                    {
                        if (item is Text || item is Picture)
                        {
                            item.opacity -= 20;
                        }
                    }
                    fadeTimer.counter = 0f;
                }
                bool result = true;
                foreach (WindowItem item in itemsList)
                {
                    if (item is Text || item is Picture)
                    {
                        if (item.opacity != 0)
                        {
                            result = false;
                        }
                    }
                }
                if (result && opacity == 0)
                {
                    Kill();
                }
            }

            //player collision
            if (player != null)
            {
                if (bounds.Intersects(player.bounds) && !playerCollision) //i've choosen precisely bounds over core.
                {
                    foreach (WindowItem item in itemsList)
                    {
                        if (item is Text || item is Picture)
                        {
                            if (item.opacity != subOpacity)
                            {
                                item.originalOpacityState = item.opacity;
                                item.opacity = subOpacity;
                            }

                        }

                    }
                    originalOpacity = opacity;
                    opacity = subOpacity;
                    playerCollision = true;
                }
                else if (!bounds.Intersects(player.bounds) && playerCollision)
                {
                    opacity = originalOpacity;
                    foreach (WindowItem item in itemsList)
                    {
                        if (item is Text || item is Picture)
                        {
                            item.opacity = item.originalOpacityState;
                        }

                    }
                    playerCollision = false;
                }
            }

        }
    }
}
