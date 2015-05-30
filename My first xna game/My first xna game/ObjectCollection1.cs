using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class ObjectCollection1 : ObjectCollection
    {
        private MovementManager movementManager;
        private Enemy wolf;
        private Actor npc;
        private Sprite block;
        private Sprite pickUpBread;
        private Sprite runningSwitch;
        private Sprite box1;
        private Sprite box2;
        private Sprite portal;
        private Sprite groundSwitch;
        private ItemCollection ItemCollection;

        public ObjectCollection1(Map map)
            : base(map)
        {
            movementManager = new MovementManager(map);
            ItemCollection = new ItemCollection();

            wolf = new Enemy(Content.Load<Texture2D>("Textures\\Spritesheets\\wolf"), new Vector2(1000f, 0f));
            wolf.AddLight(32 * 6 * 2, Color.Red);
            wolf.stats.maxHealth = 16;
            wolf.stats.health = 16;
            wolf.stats.maxMana = 16;
            wolf.stats.mana = 16;
            wolf.stats.strength = 4;
            wolf.stats.knockback = 30;
            wolf.stats.defence = 2;
            wolf.stats.agility = 1;
            wolf.Cooldown = 750f;

            npc = new Actor(Content.Load<Texture2D>("Textures\\Spritesheets\\wolf"), new Vector2(700f, 500f));
            npc.AddLight(300, Color.Yellow);
            npc.pack = new Pack(npc);
            npc.pack.AddItem(ItemCollection.ironChestArmor);
            npc.pack.AddItem(ItemCollection.bread);
            npc.pack.AddItem(ItemCollection.bread);
            npc.collisionFunction = UpdateNpcCollision;

            block = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(700f, 750f), Game.Depth.player, 2);

            pickUpBread = CreatePickup(pickUpBread, ItemCollection.bread, new Vector2(100f, 500f));
            runningSwitch = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\brick1"), new Vector2(200f, 250f), Game.Depth.below, 2);
            runningSwitch.passable = true;
            runningSwitch.collisionFunction = UpdateRunningSwitchCollision;

            box1 = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(400f, 400f), Game.Depth.player, 2);
            box1.tags.Add("box");
            box1.coreCollision.Y = 1;

            box2 = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\box1"), new Vector2(400f, 450f), Game.Depth.player, 2);
            box2.tags.Add("box");

            portal = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\player1"), new Vector2(50f, 100f), Game.Depth.player, 2);
            portal.AddLight(100, Color.Purple);
            portal.passable = true;
            portal.collisionFunction = UpdatePortalCollision;

            groundSwitch = new Sprite(Content.Load<Texture2D>("Textures\\Sprites\\brick1"), new Vector2(300f, 150f), Game.Depth.below, 2);
            groundSwitch.passable = true;
            groundSwitch.collisionFunction = UpdateGroundSwitchCollision;


            gameObjectList.Add(npc);
            gameObjectList.Add(wolf);
            gameObjectList.Add(block);
            gameObjectList.Add(pickUpBread);
            gameObjectList.Add(runningSwitch);
            gameObjectList.Add(box1);
            gameObjectList.Add(box2);
            gameObjectList.Add(portal);
            gameObjectList.Add(groundSwitch);
        }


        private void UpdatePortalCollision(GameObject portal)
        {
            foreach (GameObject gameObject in map.gameObjectList)
            {
                Window window = gameObject as Window;
                Sprite sprite = gameObject as Sprite;
                if (window == null && sprite != null)
                {
                    if (CollisionManager.GameObjectCollision(portal, gameObject)) { sprite.Reset(); }
                }

            }
        }

        private void UpdateGroundSwitchCollision(GameObject groundSwitch)
        {
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
                        if (!player.collisionsList.Contains(collisionID))//!runningSwitch.collisionHandled && 
                        {
                            //player.FlipRunning();
                            player.pack.AddItem(ItemCollection.RandomItem());
                            //player.Equip(ItemCollection.ironSword);
                            //player.collisionsList.Add(collisionID);
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
