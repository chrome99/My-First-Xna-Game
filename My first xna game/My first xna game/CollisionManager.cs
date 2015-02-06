﻿using System;
using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    class CollisionManager
    {
        public static bool GameObjectCollision(GameObject gameObject1, GameObject gameObject2)
        {
            if (!gameObject1.collision || !gameObject2.collision)
            { return false; }
            if (gameObject1.Equals(gameObject2))
            { return false; }

            if (gameObject1.core.Intersects(gameObject2.core))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GameObjectCollision(GameObject gameObject1, GameObject gameObject2, Rectangle gameObject1bounds, Rectangle gameObject2bounds)
        {
            if (!gameObject1.collision || !gameObject2.collision)
            { return false; }
            if (gameObject1.Equals(gameObject2))
            { return false; }

            if (gameObject1bounds.Intersects(gameObject2bounds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GameObjectTouch(Sprite touchingGameObject, GameObject touchedGameObject)
        {
            int thickness = (int)touchingGameObject.speed;

            if (!touchingGameObject.collision || !touchedGameObject.collision)
            { return false; }
            if (touchingGameObject.Equals(touchedGameObject))
            { return false; }

            Rectangle touchingGameObjectBounds = touchingGameObject.core;
            switch (touchingGameObject.view)
            {
                case MovementManager.Direction.down:
                    touchingGameObjectBounds.Y += thickness;
                    break;

                case MovementManager.Direction.left:
                    touchingGameObjectBounds.X -= thickness;
                    break;

                case MovementManager.Direction.right:
                    touchingGameObjectBounds.X += thickness;
                    break;

                case MovementManager.Direction.up:
                    touchingGameObjectBounds.Y -= thickness;
                    break;
            }
            return GameObjectCollision(touchingGameObject, touchedGameObject, touchingGameObjectBounds, touchedGameObject.core);
        }
    }
}