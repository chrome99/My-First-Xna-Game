using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class Map
    {
        public static List<Hostile> defultTargetsList = new List<Hostile>();
        public List<GameObject> gameObjectList = new List<GameObject>();
        public Player player1;
        public Player player2;
        public Player player3;
        public Player player4;
        private Window hud;
        private Text hudText;
        private Picture hudPicture;
        public TileMap tileMap;
        public delegate void UpdateCollision();
        private List<UpdateCollision> UpdateInstanceCollision = new List<UpdateCollision>();

        public Map(TileMap tileMap)
        {
            this.tileMap = tileMap;

            Player.PlayerKeys player1Keys;
            player1Keys.attack = Keys.Space;
            player1Keys.mvLeft = Keys.A;
            player1Keys.mvRight = Keys.D;
            player1Keys.mvUp = Keys.W;
            player1Keys.mvDown = Keys.S;
            player1Keys.opMenu = Keys.LeftControl;
            player1Keys.run = Keys.LeftShift;
            player1Keys.opDebug = Keys.F2;

            player1 = new Player(Game.content.Load<Texture2D>("starlord"), new Vector2(250f, 260f), player1Keys);
            Map.defultTargetsList.Add(player1);

            player1.stats.maxHealth = 16;
            player1.stats.health = 16;
            player1.stats.maxMana = 16;
            player1.stats.mana = 16;
            player1.stats.strength = 4;
            player1.stats.knockback = 30;
            player1.stats.cooldown = 1000f;
            player1.stats.defence = 2;
            player1.stats.agility = 1;

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

            player2 = new Player(Game.content.Load<Texture2D>("rocket"), new Vector2(300f, 260f), player2Keys);
            Map.defultTargetsList.Add(player2);

            player2.stats.maxHealth = 16;
            player2.stats.health = 16;
            player2.stats.maxMana = 16;
            player2.stats.mana = 16;
            player2.stats.strength = 4;
            player2.stats.knockback = 30;
            player2.stats.cooldown = 1000f;
            player2.stats.defence = 2;
            player2.stats.agility = 1;

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

            player3 = new Player(Game.content.Load<Texture2D>("drax"), new Vector2(350f, 260f), player3Keys);
            Map.defultTargetsList.Add(player3);

            player3.stats.maxHealth = 16;
            player3.stats.health = 16;
            player3.stats.maxMana = 16;
            player3.stats.mana = 16;
            player3.stats.strength = 4;
            player3.stats.knockback = 30;
            player3.stats.cooldown = 1000f;
            player3.stats.defence = 2;
            player3.stats.agility = 1;

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

            player4 = new Player(Game.content.Load<Texture2D>("gamora"), new Vector2(400f, 260f), player4Keys);
            Map.defultTargetsList.Add(player4);

            player4.stats.maxHealth = 16;
            player4.stats.health = 16;
            player4.stats.maxMana = 16;
            player4.stats.mana = 16;
            player4.stats.strength = 4;
            player4.stats.knockback = 30;
            player4.stats.cooldown = 1000f;
            player4.stats.defence = 2;
            player4.stats.agility = 1;

            //bla
            hud = new Window(Game.content.Load<Texture2D>("windowskin"), new Vector2(0f, 0f), 120, 90, player1);
            hudText = new Text(Game.content.Load<SpriteFont>("Debug1"), new Vector2(0f, 0f), Color.White, "", hud);
            hudPicture = new Picture(Game.content.Load<Texture2D>("brick1"), new Vector2(0f, 32f), hud);
            hud.AddItem(hudText);
            hud.AddItem(hudPicture);

            //AddObject(hud);
            AddObject(player1);
            AddObject(player2);
            AddObject(player3);
            AddObject(player4);

            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.movementManager = new MovementManager(this); ;
            }
        }

        public void AddObject(GameObject gameObject)
        {
            gameObject.movementManager = new MovementManager(this);
            gameObjectList.Add(gameObject);
        }

        public void AddObjectInstance(ObjectCollection objectInstance)
        {
            foreach (GameObject gameObject in objectInstance.gameObjectList)
            {
                AddObject(gameObject);
            }
            UpdateInstanceCollision.Add(objectInstance.updateCollision);
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            for (int i = 0; i < gameObjectList.Count; i++ )
            {
                GameObject gameObject = gameObjectList[i];
                if (true)//camera.InCamera(gameObject))
                {
                    Player player = gameObject as Player;
                    if (player != null)
                    {
                        player.UpdatePlayer(gameTime, newState, oldState, Game.content, this);

                    }
                    Sprite sprite = gameObject as Sprite;
                    if (sprite != null)
                    {
                        sprite.Update(gameTime);
                    }
                }

            }
             

            if (UpdateInstanceCollision != null)
            {
                foreach (UpdateCollision colisionFunction in UpdateInstanceCollision)
                {
                    colisionFunction();
                }
            }

            hud.Update(gameTime);
            hudText.Update("Health " + player1.stats.health);

            UpdateTypeCollision();
            
        }

        private void UpdateTypeCollision()
        {
            for (int i = 0; i < gameObjectList.Count; i++)
            {
                GameObject gameObject = gameObjectList[i];
                Player player = gameObject as Player;
                if (player != null)
                {
                    //boxs collision
                    foreach (GameObject boxs in FindTag("box"))
                    {
                        if (CollisionManager.GameObjectTouch(player, boxs)) { player.push(boxs); }
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
                                window.Draw(spriteBatch, camera.mapRect, camera.screenRect);
                            }
                            else
                            {
                                window.Draw(spriteBatch, new Rectangle(), camera.screenRect);
                            }
                            
                        }

                        Spritesheet spritesheet = gameObject as Spritesheet;
                        if (spritesheet != null)
                        {
                            spritesheet.Draw(spriteBatch, camera.mapRect, camera.screenRect);
                        }
                    }
                    else
                    {
                        sprite.Draw(spriteBatch, camera.mapRect, camera.screenRect);
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
