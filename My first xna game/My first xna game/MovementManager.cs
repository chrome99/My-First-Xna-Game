using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class MovementManager
    {
        private aStarCalculator cal;
        private Map map;
        public enum Direction { left, right, down, up }
        public enum MovingState { standing, walking, running }
        public enum Auto { off, random}
        public enum SpeedType { walking, knockback, running }
        public struct MovementString
        {
            public int destination;
            public Direction direction;
            public SpeedType speedType;
            public bool turn;
        }

        public MovementManager(Map map)
        {
            this.map = map;
            cal = new aStarCalculator(map);
        }

        public List<Vector2> WayTo(Vector2 startingPoint, Vector2 destination, Rectangle searchRect = new Rectangle(), bool diagonal = true)
        {
            return cal.FindWayTo(startingPoint, destination, diagonal, searchRect);
        }

        public void HighlightWayTo(Vector2 startingPoint, Vector2 destination, Rectangle searchRect = new Rectangle(), bool diagonal = true)
        {
            List<Vector2> way = cal.FindWayTo(startingPoint, destination, diagonal, searchRect);
            if (way == null) { return; }

            //remove old highlights
            map.RemoveTagObjects("debug highlightWay");

            //highlight destination
            GameObject light = new GameObject(way[way.Count-1]);
            light.passable = true;
            light.AddLight(32, Color.Gold);
            light.tags.Add("debug highlightWay");
            map.AddObject(light);
            way.Remove(way[way.Count - 1]);


            //highlight way
            foreach (Vector2 nodes in way)
            {
                light = new GameObject(nodes);
                light.passable = true;
                light.tags.Add("debug highlightWay");
                light.AddLight(32, Color.Red);
                map.AddObject(light);
            }
        }

        public void DrawDebug(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            cal.Draw(spriteBatch, offsetRect);
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

        public static Direction DirectionTo(Vector2 startingPoint, Vector2 destination)
        {
            Direction result = Direction.down;
            if (destination.X > startingPoint.X)
            {
                result = Direction.right;
            }
            else if (destination.X < startingPoint.X)
            {
                result = Direction.left;
            }
            else if (destination.Y > startingPoint.Y)
            {
                result = Direction.down;
            }
            else if (destination.Y < startingPoint.Y)
            {
                result = Direction.up;
            }
            return result;
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

        public static Direction OppositeDirection(Direction direction)
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

        public static Vector2 GetRectNextTo(Rectangle rect, Rectangle nextTo, Direction direction)
        {
            switch (direction)
            {
                case Direction.down:
                    rect.X = nextTo.X + nextTo.Width / 2 - rect.Width / 2;
                    rect.Y = nextTo.Y + nextTo.Height;
                    break;

                case Direction.up:
                    rect.X = nextTo.X + nextTo.Width / 2 - rect.Width / 2;
                    rect.Y = nextTo.Y - rect.Height;
                    break;

                case Direction.left:
                    rect.X = nextTo.X - rect.Width;
                    rect.Y = nextTo.Y + nextTo.Height / 2 - rect.Height / 2;
                    break;

                case Direction.right:
                    rect.X = nextTo.X + nextTo.Width;
                    rect.Y = nextTo.Y + nextTo.Height / 2 - rect.Height / 2;
                    break;
            }
            return new Vector2(rect.X, rect.Y);
        }

        public static void Knockback(Sprite target, Direction direction, int knockbackPower)
        {
            target.movementList.Add(new MovementString() { destination = knockbackPower, direction = direction,
                speedType = SpeedType.knockback, turn = false });
        }

        public bool MoveTo(GameObject gameObject, Vector2 destination)
        {
            if (!CollisionCheck(gameObject, MoveRectangle(gameObject.core, destination)))
            {
                gameObject.position = destination;
                gameObject.FixOutsideCollision();
            }
            return false;
        }

        public void MoveToDirection(GameObject gameObject, Direction direction, int speed, bool turn = true)
        {
            if (turn)
            {
                gameObject.view = direction;
            }
            Vector2 newPosition = MoveVector(gameObject.position, speed, direction);
            if (!CollisionCheck(gameObject, MoveRectangle(gameObject.core, direction, (int)speed)))
            {
                gameObject.position = newPosition;
                gameObject.FixOutsideCollision();
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
        public bool MoveActor(Actor actor, Direction direction, int speed, bool turn = true)
        {
            //if actor moving
            if (actor.enableMovement)
            {
                if (turn)
                {
                    TurnActor(actor, direction);
                }
                if (!CollisionCheck(actor, MoveRectangle(actor.core, direction, speed)))
                {
                    Vector2 destination = MoveVector(actor.position, speed, actor.direction);
                    Player player = actor as Player;
                    if (player != null)
                    {
                        if (player.impassableTilesTag != null && !player.passable)
                        {
                            if (player.impassableTilesTag.Count > 0)
                            {
                                MapCell currentCell;
                                if (direction == Direction.right || direction == Direction.down)
                                {
                                    Vector2 checkingDestination = MoveVector(destination, player.bounds.Width, Direction.right);
                                    checkingDestination = MoveVector(checkingDestination, player.bounds.Height, Direction.down);
                                    currentCell = player.map.GetTileByPosition(checkingDestination, 0);
                                }
                                else
                                {
                                    currentCell = player.map.GetTileByPosition(destination, 0);
                                }
                                if (currentCell != null)
                                {
                                    if (currentCell.tags.Contains(player.impassableTilesTag[0]))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    actor.StartAnimation(direction);
                    actor.position = destination;
                    actor.FixOutsideCollision();
                    return true;
                }
             }
            return false;
         }

        public bool InsideMap(GameObject gameObject, Rectangle gameObjectCore)
        {
            if (gameObjectCore.X > map.width * Tile.size - gameObjectCore.Width)
            {
                return false;
            }
            if (gameObjectCore.X < 0)
            {
                return false;
            }
            if (gameObjectCore.Y > map.height * Tile.size - gameObjectCore.Height)
            {
                return false;
            }
            if (gameObjectCore.Y < 0)
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

        public bool CollisionCheck(GameObject gameObject, Rectangle gameObjectBounds)
        {
            foreach (GameObject currentGameObject in map.gameObjectList)
            {
                if (CollisionManager.GameObjectCollision(gameObject, currentGameObject, gameObjectBounds, currentGameObject.core))//new Rectangle((int)currentGameObject.position.X, (int)currentGameObject.position.Y, currentGameObject.core.Width, currentGameObject.core.Height)))
                {
                    if (!gameObject.passable && !currentGameObject.passable)
                    {
                        return true;
                    }

                }

            }
            return false;
        }

    }
}
