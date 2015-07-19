using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Map
    {
        public List<Light> lightsList = new List<Light>();
        public List<Hostile> hostilesList = new List<Hostile>();
        public List<GameObject> gameObjectList = new List<GameObject>();
        private TileMap tileMap;
        private MovementManager movementManager;
        
        public string name;

        public int width
        {
            get { return tileMap.width; }
        }
        public int height
        {
            get { return tileMap.height; }
        }

        public List<Layer> Layers
        {
            get { return tileMap.layers; }
        }

        public Map(TileMap tileMap, string name)
        {
            this.tileMap = tileMap;
            this.name = name;

            movementManager = new MovementManager(this);

            //add collision objects in tileMap
            tileMap.AddCollisionObjects(this);
        }

        public bool CheckTilePassability(int x, int y)
        {
            for (int i = Layers.Count - 1; i > -1; i--)
            {
                MapCell cell = Layers[i].Rows[y].Columns[x];
                if (!cell.empty)
                {
                    return cell.passable;
                }
            }
            return true;
        }

        public bool CheckTilePassability(Vector2 tilePosition)
        {
            tilePosition.X = (int)tilePosition.X / Tile.size;
            tilePosition.Y = (int)tilePosition.Y / Tile.size;
            for (int i = Layers.Count - 1; i > -1; i--)
            {
                MapCell cell = Layers[i].Rows[(int)tilePosition.Y].Columns[(int)tilePosition.X];
                if (!cell.empty)
                {
                    return cell.passable;
                }
            }
            return true;
        }

        public void RemoveTagObjects(string tag)
        {
            for (int i = 0; i < gameObjectList.Count; )
            {
                if (gameObjectList[i].tags.Contains(tag))
                {
                    RemoveObject(gameObjectList[i]);
                }
                else
                {
                    i++;
                }
            }
        }

        public void RemoveObject(GameObject gameObject)
        {
            //hostile list
            Hostile hostile = gameObject as Hostile;
            if (hostile != null)
            {
                hostilesList.Remove(hostile);
                foreach (GameObject gameObject2 in gameObjectList)
                {
                    Enemy enemy = gameObject2 as Enemy;
                    if (enemy != null)
                    {
                        enemy.hostilesList = hostilesList;
                    }
                }
            }


            if (gameObject.getLightSource != null)
            {
                lightsList.Remove(gameObject.getLightSource);
            }

            Player player = gameObject as Player;
            if (player != null)
            {
                
            }

            gameObjectList.Remove(gameObject);
        }

        public void AddObject(GameObject gameObject)
        {
            //hostile list
            Enemy enemy = gameObject as Enemy;
            if (enemy != null)
            {
                enemy.hostilesList = hostilesList;
            }
            else
            {
                Hostile hostile = gameObject as Hostile;
                if (hostile != null)
                {
                    hostilesList.Add(hostile);
                    foreach (GameObject gameObject2 in gameObjectList)
                    {
                        Enemy enemy2 = gameObject2 as Enemy;
                        if (enemy2 != null)
                        {
                            enemy2.hostilesList = hostilesList;
                        }
                    }
                }
            }

            IntializeMapVariables(gameObject);

            if (gameObject.getLightSource != null)
            {
                lightsList.Add(gameObject.getLightSource);
            }

            gameObjectList.Add(gameObject);
        }

        public void AddObjectCollection(ObjectCollection objectCollection)
        {
            foreach (GameObject gameObject in objectCollection.gameObjectList)
            {
                AddObject(gameObject);
            }
        }

        public void IntializeMapVariables(GameObject gameObject)
        {
            gameObject.mapRect = tileMap.mapRect;
            gameObject.movementManager = movementManager;
            Player player = gameObject as Player;
            if (player != null)
            {
                //update Player Manager (he updates players details on the map)
                player.IntializeMapParamters(this);
            }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            //update tilemap
            tileMap.Update(gameTime);

            //sort game object list
            gameObjectList.Sort(CompareGameObjectsDepth);

            //update GameObjects
            for (int counter = 0; counter < gameObjectList.Count; counter++)
            {
                gameObjectList[counter].Update(gameTime);
                Player player = gameObjectList[counter] as Player;
                if (player != null)
                {
                    player.UpdatePlayer(gameTime, newState, oldState);
                }
            }

            //update lights
            for (int counter = 0; counter < lightsList.Count; counter++)
            {
                lightsList[counter].Update();
            }
        }

        private int CompareGameObjectsDepth(GameObject gameObject1, GameObject gameObject2)
        {
            Sprite sprite1 = gameObject1 as Sprite;
            Sprite sprite2 = gameObject2 as Sprite;
            if (sprite1 == null)
            {
                if (sprite2 == null)
                {
                    return 0;
                }
                else
                {
                    //gameobject2 is greater
                    return -1;
                }
            }
            else
            {
                if (sprite2 == null)
                {
                    //gameobject1 is greater
                    return 1;
                }
                else
                {
                    //if both are not null
                    if (sprite1.depth > sprite2.depth)
                    {
                        return 1;
                    }
                    else if (sprite1.depth < sprite2.depth)
                    {
                        return -1;
                    }
                    else //if depth equal
                    {
                        int retval = sprite1.position.Y.CompareTo(sprite2.position.Y);

                        if (retval != 0)
                        {
                            //not equal
                            return retval;
                        }
                        else
                        {
                            // if equal return first (gameobject 1)
                            return 1;
                        }
                    }

                }
            }
        }

        public void UpdateCollision(List<Camera> camerasList)
        {
            List<GameObject> result = new List<GameObject>();
            foreach (GameObject gameObject in gameObjectList)
            {
                foreach (Camera camera in camerasList)
                {
                    if (camera.InCamera(gameObject) && !result.Contains(gameObject))
                    {
                        result.Add(gameObject);
                    }
                }
            }
            for (int counter1 = 0; counter1 < result.Count; counter1++)
            {
                for (int counter2 = 0; counter2 < result.Count; counter2++)
                {
                    //update specific collision
                    if (result[counter1].collisionFunction != null)
                    {
                        result[counter1].collisionFunction(result[counter1], result[counter2]);
                    }

                    //update general collision
                    UpdateGeneralCollision(result[counter1], result[counter2]);
                }
            }
        }

        private void UpdateGeneralCollision(GameObject gameObject, GameObject colidedWith)
        {
            //player collision
            Player gameObjectAsPlayer = gameObject as Player;
            if (gameObjectAsPlayer != null)
            {
                //boxes collision
                if (colidedWith.tags.Contains("box"))
                {
                    if (CollisionManager.GameObjectTouch(gameObjectAsPlayer, colidedWith)) { gameObjectAsPlayer.push(colidedWith); }
                }

                //enemies collision
                Enemy enemy = colidedWith as Enemy;
                if (enemy != null)
                {
                    if (CollisionManager.GameObjectTouch(enemy, gameObjectAsPlayer))
                    {
                        gameObjectAsPlayer.DealDamage(enemy);
                    }
                }
            }

            
            //projectiles collision
            Projectile projectile = gameObject as Projectile;
            if (projectile != null)
            {
                //enemy collision
                Enemy enemy = colidedWith as Enemy;
                if (enemy != null)
                {
                    if (CollisionManager.GameObjectCollision(enemy, projectile))
                    {
                        projectile.Colide(enemy);
                    }
                }

                //player collision (pvp!)
                Player colidedWithAsPlayer = colidedWith as Player;
                if (colidedWithAsPlayer != null && colidedWithAsPlayer != projectile.source)
                {
                    if (CollisionManager.GameObjectCollision(colidedWithAsPlayer, projectile))
                    {
                        projectile.Colide(colidedWithAsPlayer);
                    }
                }
            }
        }

        public MapCell GetTileByPosition(Vector2 position, int currentLayer)
        {
            return tileMap.GetTileByPosition(position, currentLayer);
        }

        public List<MapCell> GetTilesByTag(string tag)
        {
            return tileMap.GetTilesByTag(tag);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, bool low)
        {
            if (low)
            {
                //draw tilemap (low)
                tileMap.Draw(spriteBatch, camera.screenRect, camera.mapRect, true);

                //draw gameobjects
                foreach (GameObject gameObject in gameObjectList)
                {
                    if (camera.InCamera(gameObject))
                    {
                        Sprite sprite = gameObject as Sprite;
                        if (sprite == null)
                        {
                            Window window = gameObject as Window;
                            if (window != null)
                            {
                                if (window.offsetRect)
                                {
                                    window.Draw(spriteBatch, camera.mapRect);
                                }
                                else
                                {
                                    window.Draw(spriteBatch, new Rectangle());
                                }

                            }

                            Spritesheet spritesheet = gameObject as Spritesheet;
                            if (spritesheet != null)
                            {
                                spritesheet.Draw(spriteBatch, camera.mapRect);
                            }
                        }
                        else
                        {
                            sprite.Draw(spriteBatch, camera.mapRect);
                        }
                    }
                }
            }
            else
            {
                //draw tilemap (high)
                tileMap.Draw(spriteBatch, camera.screenRect, camera.mapRect, false);
                movementManager.DrawDebug(spriteBatch, camera.mapRect);
            }
        }

        public void DrawLights(SpriteBatch spriteBatch, Camera camera)
        {
            foreach(Light light in lightsList)
            {
                light.Draw(spriteBatch, camera.mapRect);
            }
        }

        /*public List<GameObject> FindTag(string tagName)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = 0; i < gameObjectList.Count; i++)
            {
                GameObject gameObject = gameObjectList[i];
                foreach (string tag in gameObject.tags)
                {
                    if (tag == tagName) { result.Add(gameObject); }
                }
            }
            return result;
        }*/
    }
}
