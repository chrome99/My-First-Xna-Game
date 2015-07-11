using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    public class Sword : Weapon
    {
        float range;
        SoundEffect attackSound;
        SoundEffect swingSound;

        public Sword(int iconID, int price, float weight, int durability, ArmorType armorType, float range, SoundEffect attackSound, SoundEffect swingSound, Player.Stats changeStats)
            : base(iconID, price, weight, durability, armorType, changeStats)
        {
            this.range = range;
            this.attackSound = attackSound;
            this.swingSound = swingSound;
        }

        public override void Attack(Map map, Player player)
        {
            base.Attack(map, player);

            //attack
            Rectangle rangeRect = new Rectangle(0, 0, (int)(Tile.size * range), (int)(Tile.size * range));
            Vector2 rangePosition = MovementManager.GetRectNextTo(rangeRect, player.bounds, player.direction);
            rangeRect.X = (int)rangePosition.X;
            rangeRect.Y = (int)rangePosition.Y;

            bool hit = false;
            foreach (GameObject gameObject in player.map.gameObjectList)
            {
                if (player.camera.InCamera(gameObject))
                {
                    Enemy enemy = gameObject as Enemy;
                    if (enemy != null)
                    {
                        if (rangeRect.Intersects(enemy.core))
                        {
                            enemy.DealDamage(player);
                            hit = true;
                        }
                    }
                }
            }

            if (hit)
            {
                attackSound.Play();
            }
            else
            {
                swingSound.Play();
            }
        }
    }
}
