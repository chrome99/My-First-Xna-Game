using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    class ObjectInstance1 : ObjectInstance
    {
        private Map map;
        private MovementManager movementManager;
        public Enemy wolf;
        public Actor npc;
        public Sprite block;
        public Sprite runningSwitch;
        public Sprite box1;
        public Sprite box2;
        public Sprite portal;
        public Sprite groundSwitch;

        public ObjectInstance1(Map map) : base()
        {
            this.map = map;
            movementManager = new MovementManager(map);

            wolf = new Enemy(Content.Load<Texture2D>("wolf"), new Vector2(100f, 0f));
            wolf.stats.maxHealth = 16;
            wolf.stats.health = 16;
            wolf.stats.maxMana = 16;
            wolf.stats.mana = 16;
            wolf.stats.strength = 4;
            wolf.stats.knockback = 100;
            wolf.stats.cooldown = 750f;
            wolf.stats.defence = 2;
            wolf.stats.agility = 1;
            npc = new Actor(Content.Load<Texture2D>("wolf"), new Vector2(500f, 500f));
            npc.pack = new Pack();
            npc.pack.AddItem(ItemInstance.apple);
            npc.pack.AddItem(ItemInstance.bread);
            npc.pack.AddItem(ItemInstance.healthPotion);
            block = new Sprite(Content.Load<Texture2D>("box1"), new Vector2(700f, 750f), Game.Depth.player, 2);
            runningSwitch = new Sprite(Content.Load<Texture2D>("brick1"), new Vector2(200f, 250f), Game.Depth.below, 2);
            runningSwitch.through = true;
            box1 = new Sprite(Content.Load<Texture2D>("box1"), new Vector2(400f, 400f), Game.Depth.player, 2);
            box1.tags.Add("box");
            box2 = new Sprite(Content.Load<Texture2D>("box1"), new Vector2(400f, 450f), Game.Depth.player, 2);
            box2.tags.Add("box");
            portal = new Sprite(Content.Load<Texture2D>("player1"), new Vector2(50f, 100f), Game.Depth.player, 2);
            portal.through = true;
            groundSwitch = new Sprite(Content.Load<Texture2D>("brick1"), new Vector2(300f, 150f), Game.Depth.below, 2);
            groundSwitch.through = true;
            updateCollision = new Map.UpdateCollision(UpdateCollision);
            gameObjectList.Add(npc);
            //gameObjectList.Add(wolf);
            gameObjectList.Add(block);
            gameObjectList.Add(runningSwitch);
            gameObjectList.Add(box1);
            gameObjectList.Add(box2);
            gameObjectList.Add(portal);
            gameObjectList.Add(groundSwitch);
        }

        private void UpdateCollision()
        {
            //player collision
            foreach (GameObject gameObject in map.gameObjectList)
            {
                Player player = gameObject as Player;
                if (player != null)
                {
                    PlayerCollision(player);
                }
            }

            //boxs collision
            bool blockKilled = false;
            foreach (GameObject boxs in map.FindTag("box"))
            {
                if (!blockKilled)
                {
                    if (CollisionManager.GameObjectCollision(boxs, groundSwitch))
                    {
                        block.Kill();
                        blockKilled = true;
                    }
                    else
                    {
                        block.Revive();
                    }
                }
            }

            //portal collision
            foreach (GameObject gameObject in map.gameObjectList)
            {
                Window window = gameObject as Window;
                if (window == null)
                {
                    if (CollisionManager.GameObjectCollision(portal, gameObject)) { gameObject.reset(); }
                }
                
            }
        }

        private void PlayerCollision(Player player)
        {
            //npc and player
            if (CollisionManager.GameObjectTouch(player, npc))
            {
                if (npc.collisionFunction && player.collisionFunction)
                {
                    movementManager.TurnActor(npc, MovementManager.OppsiteDirection(player.direction));
                    player.Shop(npc);
                    //movementManager.Knockback(player, MovementManager.Direction.left, 100);
                    //player.MessageWindow(npc.bounds, "the great king wants to see you. \n no, he dosent.");
                    player.collisionFunction = false;
                    npc.collisionFunction = false;
                }
            }
            else
            {
                player.collisionFunction = true;
                npc.collisionFunction = true;
            }
            
            //running switch and player
            if (CollisionManager.GameObjectCollision(player, runningSwitch))
            {
                if (runningSwitch.collisionFunction && player.collisionFunction)
                {
                    player.runningSwitch();
                    player.pack.AddItem(ItemInstance.RandomItem());
                    player.collisionFunction = false;
                    runningSwitch.collisionFunction = false;
                }
            }
            else
            {
                player.collisionFunction = true;
                runningSwitch.collisionFunction = true;
            }
            //enemy and player
            foreach (GameObject gameObject in gameObjectList)
            {
                Enemy enemy = gameObject as Enemy;
                if (enemy != null)
                {
                    if (CollisionManager.GameObjectTouch(enemy, player))
                    {
                        if (enemy.collisionFunction)
                        {
                            player.DealDamage(enemy);
                            enemy.collisionFunction = false;
                        }
                    }
                    else
                    {
                        enemy.collisionFunction = true;
                    }
                }
            }
             
        }
    }
}
