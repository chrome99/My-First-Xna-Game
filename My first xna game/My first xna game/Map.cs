using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class Map : Scene
    {
        public static List<Hostile> defultTargetsList = new List<Hostile>();
        public List<GameObject> gameObjectList = new List<GameObject>();
        public List<Projectile> projectilesList = new List<Projectile>();
        public Player player1;
        public Player player2;
        private Debug debug1;
        private Debug debug2;
        private Window hud;
        private Text hudText;
        private Picture hudPicture;
        public TileMap tileMap;
        public delegate void UpdateCollision();
        private List<UpdateCollision> UpdateInstanceCollision = new List<UpdateCollision>();

        public Map(TileMap tileMap, int option)
        {
            this.tileMap = tileMap;
            player1 = new Player(Game.content.Load<Texture2D>("starlord"), new Vector2(0f, 0f));
            //player.cameraCenter = this;
            Map.defultTargetsList.Add(player1);
            player1.keys.attack = Keys.Space;
            player1.keys.left = Keys.A;
            player1.keys.right = Keys.D;
            player1.keys.up = Keys.W;
            player1.keys.down = Keys.S;
            player1.keys.menu = Keys.Escape;
            player1.keys.run = Keys.LeftShift;

            player1.stats.maxHealth = 16;
            player1.stats.health = 16;
            player1.stats.maxMana = 16;
            player1.stats.mana = 16;
            player1.stats.strength = 4;
            player1.stats.knockback = 30;
            player1.stats.cooldown = 1000f;
            player1.stats.defence = 2;
            player1.stats.agility = 1;

            debug1 = new Debug(Game.content.Load<SpriteFont>("Debug1"), Color.Blue, player1, Keys.F2);

            //intialize player
            player2 = new Player(Game.content.Load<Texture2D>("rocket"), new Vector2(260f, 260f));
            player2.keys.attack = Keys.D1;
            player2.keys.left = Keys.Left;
            player2.keys.right = Keys.Right;
            player2.keys.up = Keys.Up;
            player2.keys.down = Keys.Down;
            player2.keys.menu = Keys.Home;
            player2.keys.run = Keys.RightShift;

            player2.stats.maxHealth = 16;
            player2.stats.health = 16;
            player2.stats.maxMana = 16;
            player2.stats.mana = 16;
            player2.stats.strength = 4;
            player2.stats.knockback = 30;
            player2.stats.cooldown = 1000f;
            player2.stats.defence = 2;
            player2.stats.agility = 1;
            debug2 = new Debug(Game.content.Load<SpriteFont>("Debug1"), Color.Red, player2, Keys.F4);
            debug2.position.X = -400f;

            //bla
            hud = new Window(Game.content.Load<Texture2D>("windowskin"), new Vector2(0f, 0f), 120, 90, player1);
            hudText = new Text(Game.content.Load<SpriteFont>("Debug1"), new Vector2(0f, 0f), Color.White, "", hud);
            hudPicture = new Picture(Game.content.Load<Texture2D>("brick1"), new Vector2(0f, 32f), hud);
            hud.itemsList.Add(hudText);
            hud.itemsList.Add(hudPicture);

            //AddObject(hud);
            AddObject(player1);
            AddObject(player2);

            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.movementManager = new MovementManager(this); ;
            }
        }

        public void AddObject(GameObject gameObject)
        {
            gameObject.movementManager = new MovementManager(this); ;
            gameObjectList.Add(gameObject);
        }

        public void AddObjectInstance(ObjectInstance objectInstance)
        {
            foreach (GameObject gameObject in objectInstance.gameObjectList)
            {
                gameObject.movementManager = new MovementManager(this); ;
                gameObjectList.Add(gameObject);
            }
            UpdateInstanceCollision.Add(objectInstance.updateCollision);
        }

        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            //update spritesheet drawing
            foreach (GameObject gameObject in gameObjectList)
            {
                Sprite sprite = gameObject as Sprite;
                if (sprite != null)
                {
                    sprite.Update(gameTime);
                }
            }

            foreach (Projectile projectile in projectilesList)
            {
                //projectile.Update(gameTime);
            }

            if (UpdateInstanceCollision != null)
            {
                foreach (UpdateCollision colisionFunction in UpdateInstanceCollision)
                {
                    colisionFunction();
                }
            }


            debug1.Update();
            debug2.Update();

            hud.Update(gameTime);
            hudText.Update("Health " + player1.stats.health);

            UpdateInput(newState, oldState, Game.content);
            UpdateTypeCollision();
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState, ContentManager Content)
        {
            debug1.UpdateInput(newState, oldState);
            debug2.UpdateInput(newState, oldState);
            player1.UpdateInput(newState, oldState, Content, this);
            player2.UpdateInput(newState, oldState, Content, this);
        }

        private void UpdateTypeCollision()
        {
            foreach (GameObject gameObject in gameObjectList)
            {
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
            foreach (Projectile projectile in projectilesList)
            {
                foreach (GameObject gameObject in gameObjectList)
                {
                    Enemy enemy = gameObject as Enemy;
                    if (enemy != null)
                    {
                        if (CollisionManager.GameObjectCollision(enemy, projectile))
                        {
                            enemy.DealDamage(projectile.source);
                            projectile.Kill();
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                if (camera.InCamera(gameObject))
                {
                    Sprite sprite = gameObject as Sprite;
                    if (sprite == null)
                    {
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
            foreach (GameObject gameObject in gameObjectList)
            {
                Window window = gameObject as Window;
                if (window != null)
                {
                    window.Draw(spriteBatch, new Rectangle(), new Rectangle());
                }
            }
            foreach (Projectile projectile in projectilesList)
            {
                projectile.Draw(spriteBatch, camera.mapRect, camera.screenRect);
            }

            tileMap.Draw(spriteBatch, camera.screenRect, camera.mapRect);
            debug1.Draw(spriteBatch);
            debug2.Draw(spriteBatch);
            //then draw objects;
            //and update only object in camera
            //change position of game objects.
            //cheack for collision only within the camera.
        }

        public List<GameObject> FindTag(string tagName)
        {
            List<GameObject> result = new List<GameObject>();
            foreach (GameObject gameObject in gameObjectList)
            {
                foreach (string tag in gameObject.tags)
                {
                    if (tag == tagName) { result.Add(gameObject); }
                }
            }
            return result;
        }
    }
}
