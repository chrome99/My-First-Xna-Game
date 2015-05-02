using System;
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
    public class MovementManager
    {
        Map map;
        public enum Direction { left, right, down, up }
        public enum MovingState { standing, walking, running }
        public enum Auto { off, random}

        public MovementManager(Map map)
        {
            this.map = map;
        }

        public static Direction RandomDirection
        {
            get
            {
                Array directionValues = Enum.GetValues(typeof(Direction));
                Random random = new Random();
                Direction result = (Direction)directionValues.GetValue(random.Next(0, directionValues.Length));
                return result;
            }
        }

        public static Direction DirectionToGameObject(GameObject startingPoint, GameObject gameObject)
        {
            Direction result = Direction.down;
            if (gameObject.position.X > startingPoint.position.X)
            {
                result = Direction.right;
            }
            else if (gameObject.position.X < startingPoint.position.X)
            {
                result = Direction.left;
            }
            else if (gameObject.position.Y > startingPoint.position.Y)
            {
                result = Direction.down;
            }
            else if (gameObject.position.Y < startingPoint.position.Y)
            {
                result = Direction.up;
            }
            return result;
        }

        // TODO: typo
        public static Direction OppsiteDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.down:
                    return Direction.up;

                case Direction.up:
                    return Direction.down;

                case Direction.right:
                    return Direction.left;

                case Direction.left:
                    return Direction.right;
            }
            return 0;
        }

        public static bool IsMoving(MovingState gameObject)
        {
            if (gameObject == MovingState.walking || gameObject == MovingState.running)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Knockback(Spritesheet target, Direction direction, int knockbackPower)
        {
            Player player = target as Player;
            if (player == null)
            {
                MoveToDirection(target, knockbackPower, direction);
            }
            else
            {
                MoveActor(player, direction, knockbackPower);
            }
            
        }

        public bool MoveTo(GameObject gameObject, Vector2 destination)
        {
            if (!CollisionCheck(gameObject, MoveRectangle(gameObject.core, destination)))
            {
                /*Player player = gameObject as Player;
                if (player != null && player.cameraCenter != null)
                {

                    player.mapPosition = destination;
                    player.cameraCenter.Move(destination);
                    player.position = player.cameraCenter.PlayerMovingCamera(destination);
                    
                }
                else
                {
                    gameObject.position = destination;
                }
                */
                gameObject.position = destination;
            }
            return false;
        }

        public void MoveToDirection(GameObject gameObject, float speed, Direction direction)
        {
            gameObject.view = direction;
            Vector2 newPosition = MoveVector(gameObject.position, speed, direction);
            if (!CollisionCheck(gameObject, MoveRectangle(gameObject.core, direction, (int)speed)))
            {
                gameObject.position = newPosition;
            }
        }

        public static Vector2 MoveVector(Vector2 vector, float speed, Direction direction)
        {
            switch (direction)
            {
                case Direction.right:
                    vector.X += speed;
                    break;

                case Direction.left:
                    vector.X -= speed;
                    break;

                case Direction.up:
                    vector.Y -= speed;
                    break;

                case Direction.down:
                    vector.Y += speed;
                    break;
            }
            return vector;
        }

        public void TurnActor(Actor actor, Direction direction)
        {
            actor.view = direction;
            actor.direction = direction;
            actor.StartAnimation(direction, false);
        }
        public bool MoveActor(Actor actor, Direction direction, int speed)
        {
            //if actor moving
            if (actor.enableMovement)
            {
                TurnActor(actor, direction);
                if (!CollisionCheck(actor, MoveRectangle(actor.core, direction, speed)))
                {
                    Vector2 destination = MoveVector(actor.position, speed, actor.direction);
                    actor.StartAnimation(direction);
                    actor.position = destination;
                    return true;
                }
             }
            return false;
         }

        /*public bool MovePlayer(Player player, Direction direction, int speed)
        {
            //if actor moving
            if (player.enableMovement)
            {
                TurnActor(player, direction);
                Vector2 destination = MoveVector(player.position, speed, player.direction);

                if (!CollisionCheck(player, MoveRectangle(new Rectangle((int)player.mapPosition.X, (int)player.mapPosition.Y, player.core.Width, player.core.Height), direction, speed)) && InsideMap(player, MoveVector(player.mapPosition, speed, direction)))
                {
                    player.StartAnimation(direction);
                    if (player.cameraCenter != null)
                    {
                        if (player.cameraCenter.Move(direction, speed))
                        {
                            player.position = destination;
                        }
                    }
                    else
                    {
                        player.position = destination;
                    }
                    player.mapPosition = MoveVector(player.mapPosition, speed, direction);
                    return true;

                }
                
             }
            return false;
         }*/
        public bool InsideMap(GameObject gameObject, Vector2 gameObjectPosition)
        {
            if (gameObjectPosition.X > map.tileMap.width * Tile.size - gameObject.size.X)
            {
                return false;
            }
            if (gameObjectPosition.X < 0)
            {
                return false;
            }
            if (gameObjectPosition.Y > map.tileMap.height * Tile.size - gameObject.size.Y)
            {
                return false;
            }
            if (gameObjectPosition.Y < 0)
            {
                return false;
            }
            return true;
        }

        private static Rectangle MoveRectangle(Rectangle rect, Vector2 destination)
        {
            return new Rectangle((int)destination.X, (int)destination.Y, rect.Width, rect.Height);
        }

        private static Rectangle MoveRectangle(Rectangle rect, Direction direction, int speed)
        {
            Rectangle newRect = new Rectangle();
            switch (direction)
            {
                case Direction.right:
                    newRect = new Rectangle(rect.X + speed, rect.Y, rect.Width, rect.Height);
                    break;

                case Direction.left:
                    newRect = new Rectangle(rect.X - speed, rect.Y, rect.Width, rect.Height);
                    break;

                case Direction.up:
                    newRect = new Rectangle(rect.X, rect.Y - speed, rect.Width, rect.Height);
                    break;

                case Direction.down:
                    newRect = new Rectangle(rect.X, rect.Y + speed, rect.Width, rect.Height);
                    break;
            }
            return newRect;
        }

        private bool CollisionCheck(GameObject gameObject, Rectangle gameObjectBounds)
        {
            foreach (GameObject currentGameObject in map.gameObjectList)
            {
                if (CollisionManager.GameObjectCollision(gameObject, currentGameObject, gameObjectBounds, new Rectangle((int)currentGameObject.position.X, (int)currentGameObject.position.Y, currentGameObject.core.Width, currentGameObject.core.Height)))
                {
                    if (!gameObject.through && !currentGameObject.through)
                    {
                        return true;
                    }

                }

            }
            return false;
        }

    }
}
