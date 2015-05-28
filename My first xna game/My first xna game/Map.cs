using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Map
    {
        public List<Hostile> hostilesList = new List<Hostile>();
        public List<GameObject> gameObjectList = new List<GameObject>();
        public Player player1;
        public Player player2;
        public Player player3;
        public Player player4;
        public TileMap tileMap;

        public Map(TileMap tileMap)
        {
            this.tileMap = tileMap;

            //add collision objects in tileMap
            tileMap.AddCollisionObjects(gameObjectList);

            Player.PlayerKeys player1Keys;
            player1Keys.attack = Keys.Space;
            player1Keys.mvLeft = Keys.A;
            player1Keys.mvRight = Keys.D;
            player1Keys.mvUp = Keys.W;
            player1Keys.mvDown = Keys.S;
            player1Keys.opMenu = Keys.LeftControl;
            player1Keys.run = Keys.LeftShift;
            player1Keys.opDebug = Keys.F2;

            Hostile.Stats player1Stats;
            player1Stats.maxHealth = 26;
            player1Stats.health = 26;
            player1Stats.maxMana = 16;
            player1Stats.mana = 16;
            player1Stats.strength = 4;
            player1Stats.knockback = 30;
            player1Stats.defence = 2;
            player1Stats.agility = 1;

            player1 = new Player(this, Game.content.Load<Texture2D>("Textures\\Spritesheets\\starlord"), new Vector2(250f, 260f), player1Keys, player1Stats);
            player1.gold = 25;


            player1.pack.AddItem(new List<Item> {
                ItemCollection.goldBoots, ItemCollection.hat, ItemCollection.helmate, ItemCollection.ironBoots,
                ItemCollection.ironChestArmor, ItemCollection.ironSword, ItemCollection.leatherShoes, ItemCollection.mask,
                ItemCollection.shirt, ItemCollection.copperChestArmor, ItemCollection.apple, ItemCollection.apple, ItemCollection.apple
            });

            //intialize player
            Player.PlayerKeys player2Keys;
            player2Keys.attack = Keys.RightControl;
            player2Keys.mvLeft = Keys.Left;
            player2Keys.mvRight = Keys.Right;
            player2Keys.mvUp = Keys.Up;
            player2Keys.mvDown = Keys.Down;
            player2Keys.opMenu = Keys.Back;
            player2Keys.run = Keys.RightShift;
            player2Keys.opDebug = Keys.F4;

            Hostile.Stats player2Stats;
            player2Stats.maxHealth = 16;
            player2Stats.health = 16;
            player2Stats.maxMana = 16;
            player2Stats.mana = 16;
            player2Stats.strength = 4;
            player2Stats.knockback = 30;
            player2Stats.defence = 2;
            player2Stats.agility = 1;

            player2 = new Player(this, Game.content.Load<Texture2D>("Textures\\Spritesheets\\rocket"), new Vector2(300f, 260f), player2Keys, player2Stats);
            player2.gold = 25;

            //intialize player
            Player.PlayerKeys player3Keys;
            player3Keys.attack = Keys.R;
            player3Keys.mvLeft = Keys.F;
            player3Keys.mvRight = Keys.H;
            player3Keys.mvUp = Keys.T;
            player3Keys.mvDown = Keys.G;
            player3Keys.opMenu = Keys.B;
            player3Keys.run = Keys.Y;
            player3Keys.opDebug = Keys.F6;

            Hostile.Stats player3Stats;
            player3Stats.maxHealth = 16;
            player3Stats.health = 16;
            player3Stats.maxMana = 16;
            player3Stats.mana = 16;
            player3Stats.strength = 4;
            player3Stats.knockback = 30;
            player3Stats.defence = 2;
            player3Stats.agility = 1;

            player3 = new Player(this, Game.content.Load<Texture2D>("Textures\\Spritesheets\\drax"), new Vector2(350f, 260f), player3Keys, player3Stats);
            player3.gold = 25;

            //intialize player
            Player.PlayerKeys player4Keys;
            player4Keys.attack = Keys.U;
            player4Keys.mvLeft = Keys.J;
            player4Keys.mvRight = Keys.L;
            player4Keys.mvUp = Keys.I;
            player4Keys.mvDown = Keys.K;
            player4Keys.opMenu = Keys.M;
            player4Keys.run = Keys.O;
            player4Keys.opDebug = Keys.F8;

            Hostile.Stats player4Stats;
            player4Stats.maxHealth = 16;
            player4Stats.health = 16;
            player4Stats.maxMana = 16;
            player4Stats.mana = 16;
            player4Stats.strength = 4;
            player4Stats.knockback = 30;
            player4Stats.defence = 2;
            player4Stats.agility = 1;

            player4 = new Player(this, Game.content.Load<Texture2D>("Textures\\Spritesheets\\gamora"), new Vector2(400f, 260f), player4Keys, player4Stats);
            player4.gold = 25;
            
            /*player1.coreCollision.Y = 4;
            player2.coreCollision.Y = 4;
            player3.coreCollision.Y = 4;
            player4.coreCollision.Y = 4;*/
            
            //add players to PlayerManager
            PlayerManager.playersList = new List<Player> { player1, player2, player3, player4 };

            AddObject(player1);
            AddObject(player2);
            AddObject(player3);
            AddObject(player4);
        }

        public void AddObject(GameObject gameObject)
        {
            //additional intializetion
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
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            //update GameObjects
            for (int counter = 0; counter < gameObjectList.Count; counter++)
            {
                gameObjectList[counter].Update(gameTime);
            }

            //update Player Manager (he updates players details on the map)
            PlayerManager.UpdatePlayersMapParameters(this, newState, oldState);

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
                    //boxs collision
                    foreach (GameObject boxs in FindTag("box"))
                    {
                        if (CollisionManager.GameObjectTouch(player, boxs)) { player.push(boxs); }
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

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
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

            //draw tilemap
            tileMap.Draw(spriteBatch, camera.screenRect, camera.mapRect);
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
