using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    public class ObjectCollection
    {
        public List<GameObject> gameObjectList = new List<GameObject>();
        protected ContentManager Content;
        protected Map map;

        public ObjectCollection(Map map)
        {
            this.map = map;
            Content = Game.content;
        }

        public Sprite CreatePickup(Sprite sprite, Item item, Vector2 position)
        {
            Sprite result = new Sprite(Item.IconSet, position, Game.Depth.below, 2f, item.getRect());
            result.passable = true;
            result.collisionParameters[0] = item;
            result.collisionFunction = UpdatePickupCollision;
            return result;
        }

        private void UpdatePickupCollision(GameObject pickUp)
        {
            foreach (GameObject gameObject in map.gameObjectList)
            {
                Player player = gameObject as Player;
                if (player != null)
                {
                    if (CollisionManager.GameObjectCollision(pickUp, player))
                    {
                            Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                            player.pack.AddItem(pickUp.collisionParameters[0]);
                            pickUp.Kill();
                    }
                }
            }

        }
    }
}
