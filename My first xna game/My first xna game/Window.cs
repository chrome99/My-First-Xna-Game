using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Window : Sprite
    {
        public static Rectangle windowRect = new Rectangle(0, 0, 128, 128);
        public List<WindowItem> itemsList = new List<WindowItem>();
        public List<WindowItem> hiddenItemsList = new List<WindowItem>();
        public Vector2 thickness = new Vector2(10, 10);
        public bool offsetRect = true;
        public int width;
        public int height;
        public Window source;

        // TODO: Make an opacity value for each state
        private float originalOpacity;
        private float subOpacity = 50f;
        private Player player;
        private bool playerCollision = false;

        public Window(Map map, Texture2D texture, Vector2 position, int width, int height, Player player = null)
            : base(texture, position, Game.Depth.windows)
        {
            this.width = width;
            this.height = height;
            this.player = player;

            map.IntializeMapVariables(this);

            originalOpacity = opacity;
            passable = true;
        }

        private Vector2 GetWindowCenter(Rectangle positionBounds)
        {
            //set center
            Vector2 result;
            result.X = positionBounds.X + positionBounds.Width / 2 - bounds.Width / 2;
            result.Y = positionBounds.Y + positionBounds.Height / 2 - bounds.Height / 2;

            return result;
        }

        public void SetWindowCenter(Rectangle positionBounds)
        {
            //set center
            position = GetWindowCenter(positionBounds);
            FixOutsideCollision();
        }

        public void SetWindowAbove(Rectangle positionBounds)
        {
            //set center
            Vector2 newPosition = GetWindowCenter(positionBounds);

            //set above
            newPosition.Y = newPosition.Y - positionBounds.Height / 2 - bounds.Height / 2 - 5; //TODO: why -5?


            position = newPosition;
            FixOutsideCollision();
        }

        public void SetWindowBelow(Rectangle positionBounds)
        {
            //set center
            Vector2 newPosition = GetWindowCenter(positionBounds);

            //set above
            newPosition.Y = newPosition.Y + positionBounds.Height / 2 + bounds.Height / 2;

            position = newPosition;
            FixOutsideCollision();
        }

        public void SetWindowLeft(Rectangle positionBounds)
        {
            //set center
            Vector2 newPosition = GetWindowCenter(positionBounds);

            //set above
            newPosition.X = newPosition.X - positionBounds.Width / 2 - bounds.Width / 2;

            position = newPosition;
            FixOutsideCollision();
        }

        public void SetWindowRight(Rectangle positionBounds)
        {
            //set center
            Vector2 newPosition = GetWindowCenter(positionBounds);

            //set above
            newPosition.X = newPosition.X + positionBounds.Width / 2 + bounds.Width / 2;

            position = newPosition;
            FixOutsideCollision();
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

        public void AddHiddenItem(WindowItem item)
        {
            hiddenItemsList.Add(item);
            item.source = this;
        }

        public void setCorner(Camera camera, Camera.Corner corner)
        {
            Rectangle side = camera.setSide(bounds, corner);
            position.X = side.X;
            position.Y = side.Y;
        }

        private Rectangle GetDrawingRect(Rectangle offsetRect)
        {
            Rectangle newPosition = bounds;
            if (source != null)
            {
                Vector2 calculatedPosition = position + source.position + source.thickness;
                newPosition.X = (int)calculatedPosition.X;
                newPosition.Y = (int)calculatedPosition.Y;
            }
            newPosition.X = newPosition.X - offsetRect.X;
            newPosition.Y = newPosition.Y - offsetRect.Y;
            return newPosition;
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (visible && alive)
            {
                //draw window
                Rectangle drawingPosition = GetDrawingRect(offsetRect);

                spriteBatch.Draw(texture, drawingPosition, windowRect, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, Game.DepthToFloat(depth));

                //draw items
                foreach (WindowItem item in itemsList)
                {
                    item.Draw(spriteBatch, new Vector2(drawingPosition.X, drawingPosition.Y));
                }
                foreach (WindowItem item in hiddenItemsList)
                {
                    item.Draw(spriteBatch, new Vector2(drawingPosition.X, drawingPosition.Y));
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

        public void UpdateSelectorAndTextBox(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            foreach (WindowItem item in itemsList)
            {
                Selector selector = item as Selector;
                if (selector != null)
                {
                    selector.UpdateSelector(newState, oldState, gameTime);
                }

                Textbox textbox = item as Textbox;
                if (textbox != null)
                {
                    textbox.UpdateTextbox(gameTime, newState, oldState);
                }
            }

            foreach (WindowItem item in hiddenItemsList)
            {
                Selector selector = item as Selector;
                if (selector != null)
                {
                    selector.UpdateSelector(newState, oldState, gameTime);
                }

                Textbox textbox = item as Textbox;
                if (textbox != null)
                {
                    textbox.UpdateTextbox(gameTime, newState, oldState);
                }
            }
        }
    }
}