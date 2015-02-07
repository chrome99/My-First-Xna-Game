using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class Camera
    {
        public Map map;
        private Vector2 cellNumber(Player player)
        {
            Vector2 result;
            result.X = screenRect.Width / 2 - player.bounds.Width / 2;
            result.Y = screenRect.Height / 2 - player.bounds.Height / 2;
            return result;
        }
        //private Rectangle testView;
        public Rectangle mapRect;
        public Rectangle screenRect;
        public enum Corner {topLeft, topRight, bottomLeft, bottomRight}
        int option;

        public Camera(Rectangle screenRect, Map map, int option)
        {
            this.option = option;
            this.screenRect = screenRect;
            this.mapRect = screenRect;
            this.map = map;
        }

        /*public bool isParent(GameObject obj)
        {
            return obj.Equals(this.player);
        }*/

        public Rectangle view
        {
            get { return new Rectangle((int)mapRect.X, (int)mapRect.Y, mapRect.X + screenRect.Width, mapRect.Y + screenRect.Height); }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            switch (option)
            {
                case 0:
                    Move(map.player1.position - cellNumber(map.player1));
                    break;

                case 1:
                    Move(map.player2.position - cellNumber(map.player2));
                    break;

                case 2:
                    Move(map.player3.position - cellNumber(map.player3));
                    break;

                case 3:
                    Move(map.player4.position - cellNumber(map.player4));
                    break;

            }      
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch, this);
        }

        public Rectangle setCenter(Rectangle rect)
        {
            Rectangle result;
            result.X = view.Width / 2 - rect.Width / 2;
            result.Y = view.Height / 2 - rect.Height / 2;
            result.Width = rect.Width;
            result.Height = rect.Height;
            return result;
        }
        public Rectangle setSide(Rectangle rect, Corner corner)
        {
            switch (corner)
            {
                case Corner.topLeft:
                    return new Rectangle((int)mapRect.X, (int)mapRect.Y, rect.Width, rect.Height);

                case Corner.topRight:
                    return new Rectangle(view.Width - rect.Width, (int)mapRect.Y, rect.Width, rect.Height);

                case Corner.bottomLeft:
                    return new Rectangle((int)mapRect.X, view.Height - rect.Height, rect.Width, rect.Height);

                case Corner.bottomRight:
                    return new Rectangle(view.Width - rect.Width, view.Height - rect.Height, rect.Width, rect.Height);
            }
            return new Rectangle();
        }

        public bool InCamera(GameObject gameObject)
        {
            return map.gameObjectList.FindAll(obj => new Rectangle((int)obj.position.X, (int)obj.position.Y, (int)obj.size.X, (int)obj.size.Y).Intersects(new Rectangle(mapRect.X, mapRect.Y, screenRect.Width, screenRect.Height))).Contains(gameObject);
        }

        /*public bool Move(MovementManager.Direction direction, int speed, bool moveCamera = true)
        {
            if (direction == MovementManager.Direction.right || direction == MovementManager.Direction.left)
            {
                //if (mapRect.X < cellNumber.X && player.position.X - screenRect.X < cellNumber.X)
                if (view.Left < cellNumber.X && player.position.X - screenRect.X < cellNumber.X)
                {
                    return true;
                }
                if (mapRect.X == map.tileMap.width * Tile.size - screenRect.Width && player.mapPosition.X > map.tileMap.width * Tile.size - cellNumber.X)
                {
                    return true;
                }
            }

            if (direction == MovementManager.Direction.up || direction == MovementManager.Direction.down)
            {
                if (mapRect.Y == map.tileMap.height * Tile.size - screenRect.Height && player.mapPosition.Y > map.tileMap.height * Tile.size - cellNumber.Y)
                {
                    return true;
                }
                if (view.Top < cellNumber.Y && player.position.Y < cellNumber.Y)
                {
                    return true;
                }
            }
            bool result = MoveAlguritem(direction, speed, moveCamera);
            return result;
        }

        public Vector2 PlayerMovingCamera(Vector2 destination)
        {
            testView = view;
            Vector2 result = new Vector2(cellNumber.X, cellNumber.Y);

            testView.X = (int)MathHelper.Clamp(destination.X, 0, map.tileMap.width * Tile.size - screenRect.Width);//?
            testView.Y = (int)MathHelper.Clamp(destination.Y, 0, map.tileMap.height * Tile.size - screenRect.Height);
            if (testView.X == mapRect.X && destination.X < cellNumber.X)
            {
                result.X -= 0;//player.mapPosition.X / 100;
            }
            if (testView.Right == map.tileMap.width * Tile.size && destination.X > cellNumber.X)
            {
                result.X += (cellNumber.X - ((map.tileMap.width -1f) * Tile.size - player.mapPosition.X));
            }

            if (testView.Bottom == map.tileMap.height * Tile.size && destination.Y > cellNumber.Y)
            {
                result.Y += (cellNumber.Y - ((map.tileMap.height - 1f) * Tile.size - player.mapPosition.Y));
            }
            if (testView.Y == mapRect.Y && destination.Y < cellNumber.Y)
            {
                result.Y -= (cellNumber.Y - (mapRect.Y - player.mapPosition.Y));
            }
            return result;
        }*/

        public void Move(Vector2 destination)
        {
            mapRect.X = (int)MathHelper.Clamp(destination.X, 0, (map.tileMap.width - screenRect.Width / Tile.size) * Tile.size);//?
            mapRect.Y = (int)MathHelper.Clamp(destination.Y, 0, (map.tileMap.height - screenRect.Height / Tile.size) * Tile.size);
        }

        private bool MoveAlguritem(MovementManager.Direction direction, int speed, bool moveCamera)
        {
            Rectangle result = mapRect;
            bool returnValue = false;

            switch (direction)
            {
                case MovementManager.Direction.down:
                    result.Y = (int)MathHelper.Clamp(mapRect.Y + speed, 0, map.tileMap.height * Tile.size - screenRect.Height);//?
                    break;

                case MovementManager.Direction.left:
                    result.X = (int)MathHelper.Clamp(mapRect.X - speed, 0, map.tileMap.width * Tile.size - screenRect.Width);
                    break;

                case MovementManager.Direction.right:
                    result.X = (int)MathHelper.Clamp(mapRect.X + speed, 0, map.tileMap.width * Tile.size - screenRect.Width);
                    break;

                case MovementManager.Direction.up:
                    result.Y = (int)MathHelper.Clamp(mapRect.Y - speed, 0, map.tileMap.height * Tile.size - screenRect.Height);
                    break;
            }
            if (result == mapRect)
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            if (moveCamera)
            {
                mapRect = result;
            }
            return returnValue;
        }
    }
}
