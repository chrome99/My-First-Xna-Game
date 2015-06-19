using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Map
    {
        public List<LightSource> lightsList = new List<LightSource>();
        public List<Hostile> hostilesList = new List<Hostile>();
        public List<GameObject> gameObjectList = new List<GameObject>();
        public TileMap tileMap;
        public string name;

        public Map(TileMap tileMap, string name)
        {
            this.tileMap = tileMap;
            this.name = name;

            //add collision objects in tileMap
            tileMap.AddCollisionObjects(this);
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
            gameObject.movementManager = new MovementManager(this);
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

            //update GameObjects
            for (int counter = 0; counter < gameObjectList.Count; counter++)
            {
                gameObjectList[counter].Update(gameTime);
            }

            //update lights
            for (int counter = 0; counter < lightsList.Count; counter++)
            {
                lightsList[counter].Update();
            }

            //update Player Manager (he updates players details on the map)
            PlayerManager.UpdatePlayersMapParameters(newState, oldState);

            //update general collision
            UpdateTypeCollision();
            
        }

        private void UpdateTypeCollision()
        {
            //player collision
            foreach(GameObject gameObject1 in gameObjectList)
            {
                Player player = gameObject1 as Player;
                if (player != null)
                {
                    //boxes collision
                    foreach (GameObject boxes in FindTag("box"))
                    {
                        if (CollisionManager.GameObjectTouch(player, boxes)) { player.push(boxes); }
                    }

                    //enemies collision //TODO: what does this do?
                    foreach (GameObject gameObject2 in gameObjectList)
                    {
                        Enemy enemy = gameObject2 as Enemy;
                        if (enemy != null)
                        {
                            if (CollisionManager.GameObjectTouch(enemy, player))
                            {
                                player.DealDamage(enemy);
                            }
                        }
                    }
                }
            }

            
            //projectiles collision
            foreach (GameObject gameObject1 in gameObjectList)
            {
                Projectile projectile = gameObject1 as Projectile;
                if (projectile != null)
                {
                    //enemy collision
                    foreach (GameObject gameObject2 in gameObjectList)
                    {
                        Enemy enemy = gameObject2 as Enemy;
                        if (enemy != null)
                        {
                            if (CollisionManager.GameObjectCollision(enemy, projectile))
                            {
                                projectile.PlayHitSound();
                                enemy.DealDamage(projectile.source);
                                projectile.Kill();
                            }
                        }
                    }

                    //player collision (pvp!)
                    foreach (GameObject gameObject2 in gameObjectList)
                    {
                        Player player = gameObject2 as Player;
                        if (player != null && player != projectile.source)
                        {
                            if (CollisionManager.GameObjectCollision(player, projectile))
                            {
                                projectile.PlayHitSound();
                                player.DealDamage(projectile.source);
                                projectile.Kill();
                            }
                        }
                    }
                }
            }
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
            }
        }

        public void DrawLights(SpriteBatch spriteBatch, Camera camera)
        {
            foreach(LightSource lightSource in lightsList)
            {
                lightSource.Draw(spriteBatch, camera.mapRect);
            }
        }

        public List<GameObject> FindTag(string tagName)
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
        }
    }
}
