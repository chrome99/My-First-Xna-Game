using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    class Pickup : Sprite
    {
        Item item;
        public Pickup(Sprite sprite, Item item, Vector2 position)
            : base(Item.IconSet, position, item.getRect)
        {
            this.item = item;

            MapDepth = Game.MapDepth.below;

            passable = true;
            collisionFunction = UpdatePickupCollision;
        }

        private void UpdatePickupCollision(GameObject gameObject, GameObject colidedWith)
        {
            Player player = colidedWith as Player;
            if (player != null)
            {
                Pickup pickUp = gameObject as Pickup;
                if (CollisionManager.GameObjectCollision(pickUp, player))
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                    player.pack.AddItem(ItemCollection.CopyItem(pickUp.item));
                    pickUp.Kill();
                }
            }
        }
    }
}
