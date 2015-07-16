using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class ObjectCollection1 : ObjectCollection
    {
        private MovementManager movementManager;
        private Enemy wolf;
        private Enemy wolf2;
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

            wolf = new Enemy(Content.Load<Texture2D>("Textures\\Spritesheets\\wolf"), new Vector2(25 * 32, 16 * 32));
            wolf.speed = 2;
            wolf.stats.maxHealth = 16;
            wolf.stats.health = 16;
            wolf.stats.maxMana = 16;
            wolf.stats.mana = 16;
            wolf.stats.strength = 4;
            wolf.stats.knockback = 30;
            wolf.stats.defence = 2;
            wolf.stats.agility = 1;
            wolf.stats.exp = 5;
            wolf.Cooldown = 750f;
            wolf.core = new Rectangle(7, 30, 18, 16);

            wolf2 = new Enemy(Content.Load<Texture2D>("Textures\\Spritesheets\\wolf"), new Vector2(30 * 32, 16 * 32));
            wolf2.speed = 2;
            wolf2.stats.maxHealth = 16;
            wolf2.stats.health = 16;
            wolf2.stats.maxMana = 16;
            wolf2.stats.mana = 16;
            wolf2.stats.strength = 4;
            wolf2.stats.knockback = 30;
            wolf2.stats.defence = 2;
            wolf2.stats.agility = 1;
            wolf2.stats.exp = 7;
            wolf2.Cooldown = 750f;
            wolf2.core = new Rectangle(7, 30, 18, 16);

            npc = new Actor(Content.Load<Texture2D>("Textures\\Spritesheets\\mage"), new Vector2(22 * 32, 18 * 32));
            npc.pack = new Pack(npc);
            npc.pack.AddItem(ItemCollection.ironChestArmor);
            npc.pack.AddItem(ItemCollection.bread);
            npc.pack.AddItem(ItemCollection.bread);
            npc.interactFunction = NpcInteraction;
            npc.core = new Rectangle(7, 30, 18, 16);

            block = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(30 * 32, 30 * 32));

            boatCollision = new GameObject(new Vector2(33 * 32, 29 * 32));
            boatCollision.collisionFunction = BoatcollisionCollision;

            boat = new Vehicle(Content.Load<Texture2D>("Textures\\Spritesheets\\boat"), new Vector2(33.5f * 32, 30 * 32), new List<string>() { "water" }, new List<string>() { "grass" });
            boat.ShowAnimation = true;
            movementManager.TurnActor(boat, MovementManager.Direction.left);

            holdBox = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(20 * 32, 30 * 32));
            holdBox.collisionFunction = HoldBoxCollision;

            pickUpBread = new Pickup(pickUpBread, ItemCollection.bread, new Vector2(11 * 32, 34 * 32));

            groundSwitch = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\brick1"), new Vector2(36 * 32, 25 * 32));
            groundSwitch.MapDepth = Game.MapDepth.below;
            groundSwitch.passable = true;
            groundSwitch.collisionFunction = GroundSwitchCollision;

            box1 = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(27 * 32, 25 * 32));
            box1.tags.Add("box");

            box2 = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(30 * 32, 25 * 32));
            box2.tags.Add("box");

            runningSwitch = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\brick1"), new Vector2(27 * 32, 18 * 32));
            runningSwitch.MapDepth = Game.MapDepth.below;
            runningSwitch.passable = true;
            runningSwitch.collisionFunction = RunningSwitchCollision;

            portal = new GameObject(new Vector2(34 * 32, 22 * 32));
            portal.passable = true;
            portal.collisionFunction = PortalCollision;

            gameObjectList.Add(runningSwitch);
            gameObjectList.Add(groundSwitch);
            gameObjectList.Add(npc);
            gameObjectList.Add(boat);
            gameObjectList.Add(boatCollision);
            gameObjectList.Add(portal);
            gameObjectList.Add(wolf);
            gameObjectList.Add(wolf2);
            gameObjectList.Add(block);
            gameObjectList.Add(holdBox);
            gameObjectList.Add(pickUpBread);
            gameObjectList.Add(box1);
            gameObjectList.Add(box2);
        }


        private void BoatcollisionCollision(GameObject boatCollision, GameObject colidedWith)
        {
            int collisionID = map.gameObjectList.IndexOf(boatCollision);
            Player player = colidedWith as Player;
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

        private void HoldBoxCollision(GameObject HoldBox, GameObject colidedWith)
        {
            Player player = colidedWith as Player;
            if (player != null)
            {
                if (CollisionManager.GameObjectTouch(player, holdBox))
                {
                    player.HoldObject(holdBox);
                }
            }
        }

        private void PortalCollision(GameObject portal, GameObject colidedWith)
        {
            Window window = colidedWith as Window;
            Sprite sprite = colidedWith as Sprite;
            if (window == null && sprite != null)
            {
                if (CollisionManager.GameObjectCollision(portal, sprite))
                {
                    MapCollection.classic.RemoveObject(sprite);
                    MapCollection.tower.AddObject(sprite);
                    sprite.Reset();
                }
            }
        }

        private void GroundSwitchCollision(GameObject groundSwitch, GameObject colidedWith)
        {
            if (colidedWith.tags.Contains("box"))
            {
                bool blockKilled = false;
                if (!blockKilled)
                {
                    if (CollisionManager.GameObjectCollision(colidedWith, groundSwitch))
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


        private void NpcInteraction(Player player, GameObject interactedWith)
        {
            if (player.canInteract())
            {
                if (player.Shop(npc))
                {
                    movementManager.TurnActor(npc, MovementManager.OppositeDirection(player.direction));
                    //movementManager.Knockback(player, MovementManager.Direction.left, 100);
                    //player.MessageWindow(npc2.bounds, new List<string> {"the great king wants to see you. \n no, he dosent.", "asd"}, true);
                    //player.MessageWindow(npc2.bounds, "the king wants to see you. \n no, he dosent.", true);
                }
            }
        }


        private void RunningSwitchCollision(GameObject runningSwitch, GameObject colidedWith)
        {
            //running switch and player
            Player player = colidedWith as Player;
            if (player != null)
            {
                int collisionID = map.gameObjectList.IndexOf(runningSwitch);
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
