using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class ObjectCollection1 : ObjectCollection
    {
        private MovementManager movementManager;
        private Enemy wolf;
        private Actor npc;
        private Vehicle boat;
        private GameObject boatCollision;
        private GameObject portal;

        private Sprite block;
        private Sprite holdBox;
        private Sprite pickUpBread;
        private Sprite runningSwitch;
        private Sprite box1;
        private Sprite box2;
        private Sprite groundSwitch;

        public ObjectCollection1(Map map)
            : base(map)
        {
            movementManager = new MovementManager(map);

            wolf = new Enemy(Content.Load<Texture2D>("Textures\\Spritesheets\\wolf"), new Vector2(41 * 32, 43 * 32));
            wolf.stats.maxHealth = 16;
            wolf.stats.health = 16;
            wolf.stats.maxMana = 16;
            wolf.stats.mana = 16;
            wolf.stats.strength = 4;
            wolf.stats.knockback = 30;
            wolf.stats.defence = 2;
            wolf.stats.agility = 1;
            wolf.Cooldown = 750f;

            npc = new Actor(Content.Load<Texture2D>("Textures\\Spritesheets\\mage"), new Vector2(22 * 32, 16 * 32));
            npc.pack = new Pack(npc);
            npc.pack.AddItem(ItemCollection.ironChestArmor);
            npc.pack.AddItem(ItemCollection.bread);
            npc.pack.AddItem(ItemCollection.bread);
            npc.collisionFunction = UpdateNpcCollision;

            block = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(37 * 32, 21 * 32), Game.Depth.player, 2);

            boatCollision = new GameObject(new Vector2(33 * 32, 29 * 32));
            boatCollision.collisionFunction = UpdateBoatCollision;

            boat = new Vehicle(Content.Load<Texture2D>("Textures\\Spritesheets\\boat"), new Vector2(33.5f * 32, 30 * 32), new List<string>() { "water" }, new List<string>() { "grass" });
            boat.ShowAnimation = true;
            movementManager.TurnActor(boat, MovementManager.Direction.left);

            holdBox = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(40 * 32, 21 * 32), Game.Depth.player, 2);
            holdBox.collisionFunction = UpdateHoldBoxCollision;

            pickUpBread = CreatePickup(pickUpBread, ItemCollection.bread, new Vector2(11 * 32, 34 * 32));

            groundSwitch = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\brick1"), new Vector2(38 * 32, 25 * 32), Game.Depth.below, 2);
            groundSwitch.passable = true;
            groundSwitch.collisionFunction = UpdateGroundSwitchCollision;

            box1 = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(27 * 32, 25 * 32), Game.Depth.player, 2);
            box1.tags.Add("box");

            box2 = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(30 * 32, 25 * 32), Game.Depth.player, 2);
            box2.tags.Add("box");

            runningSwitch = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\brick1"), new Vector2(27 * 32, 18 * 32), Game.Depth.below, 2);
            runningSwitch.passable = true;
            runningSwitch.collisionFunction = UpdateRunningSwitchCollision;

            //portal = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\player1"), new Vector2(34 * 32, 22* 32), Game.Depth.player, 2);
            portal = new GameObject(new Vector2(34 * 32, 22 * 32));
            portal.passable = true;
            portal.collisionFunction = UpdatePortalCollision;


            gameObjectList.Add(npc);
            gameObjectList.Add(boat);
            gameObjectList.Add(boatCollision);
            gameObjectList.Add(portal);

            //gameObjectList.Add(wolf);
            //gameObjectList.Add(block);
            //gameObjectList.Add(holdBox);
            //gameObjectList.Add(pickUpBread);
            //gameObjectList.Add(runningSwitch);
            //gameObjectList.Add(groundSwitch);
            //gameObjectList.Add(box1);
            //gameObjectList.Add(box2);
        }


        private void UpdateBoatCollision(GameObject boatCollision)
        {
            for (int i = 0; i < map.gameObjectList.Count; i++)
            {
                int collisionID = boatCollision.GetID(map.gameObjectList);
                Player player = map.gameObjectList[i] as Player;
                if (player != null)
                {
                    if (CollisionManager.GameObjectTouch(player, boatCollision) && !player.collisionsList.Contains(collisionID))
                    {
                        if (player.Riding)
                        {
                            player.Backoff();
                            player.collisionsList.Add(collisionID);
                        }
                        else
                        {
                            player.Ride(boat);
                        }
                        
                    }
                    else
                    {
                        player.collisionsList.Remove(collisionID);
                    }
                }
            }
        }

        private void UpdateHoldBoxCollision(GameObject HoldBox)
        {
            for (int i = 0; i < map.gameObjectList.Count; i++)
            {
                Player player = map.gameObjectList[i] as Player;
                if (player != null)
                {
                    if (CollisionManager.GameObjectTouch(player, holdBox))
                    {
                        player.HoldObject(holdBox);
                    }
                }
            }
        }

        private void UpdatePortalCollision(GameObject portal)
        {
            for (int i = 0; i < map.gameObjectList.Count; i++)
            {
                GameObject gameObject = map.gameObjectList[i];
                Window window = gameObject as Window;
                Sprite sprite = gameObject as Sprite;
                if (window == null && sprite != null)
                {
                    if (CollisionManager.GameObjectCollision(portal, sprite))
                    {
                        MapCollection.map.RemoveObject(sprite);
                        MapCollection.map2.AddObject(sprite);
                        sprite.Reset();
                    }
                }

            }
        }

        private void UpdateGroundSwitchCollision(GameObject groundSwitch)
        {
            bool blockKilled = false;
            foreach (GameObject boxes in map.FindTag("box"))
            {
                if (!blockKilled)
                {
                    if (CollisionManager.GameObjectCollision(boxes, groundSwitch))
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
        }


        private void UpdateNpcCollision(GameObject gameObject1)
        {
            //TODO: fix npc2

            Actor npc2 = gameObject1 as Actor;
            foreach (GameObject gameObject2 in map.gameObjectList)
            {
                Player player = gameObject2 as Player;
                if (player != null)
                {
                    int collisionID = npc2.GetID(map.gameObjectList);
                    //npc and player
                    if (CollisionManager.GameObjectTouch(player, npc2))
                    {
                        if (!player.collisionsList.Contains(collisionID))
                        {
                            if (player.canInteract())
                            {
                                if (player.Shop(npc))
                                {
                                    movementManager.TurnActor(npc2, MovementManager.OppositeDirection(player.direction));
                                    //movementManager.Knockback(player, MovementManager.Direction.left, 100);
                                    //player.MessageWindow(npc2.bounds, new List<string> {"the great king wants to see you. \n no, he dosent.", "asd"}, true);
                                    //player.MessageWindow(npc2.bounds, "the king wants to see you. \n no, he dosent.", true);
                                    player.collisionsList.Add(collisionID);
                                }
                            }
                        }
                    }
                    else
                    {
                        player.collisionsList.Remove(collisionID);
                    }
                }
            }
        }


        private void UpdateRunningSwitchCollision(GameObject runningSwitch)
        {
            //running switch and player
            foreach (GameObject gameObject in map.gameObjectList)
            {
                Player player = gameObject as Player;
                if (player != null)
                {
                    int collisionID = runningSwitch.GetID(map.gameObjectList);
                    if (CollisionManager.GameObjectCollision(player, runningSwitch))
                    {
                        if (!player.collisionsList.Contains(collisionID))
                        {
                            player.FlipRunning();
                            player.collisionsList.Add(collisionID);
                        }
                    }
                    else
                    {
                        player.collisionsList.Remove(collisionID);
                    }
                }
            }
        }
    }
}
