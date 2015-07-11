

namespace My_first_xna_game
{
    public class Bow : Weapon
    {
        Projectile.ProjectileData projectileData;

        public Bow(int iconID, int price, float weight, int durability, ArmorType armorType, Projectile.ProjectileData projectileData, Player.Stats changeStats)
            : base(iconID, price, weight, durability, armorType, changeStats)
        {
            this.projectileData = projectileData;
        }

        public override void Attack(Map map, Player player)
        {
            base.Attack(map, player);

            Projectile.LaunchProjectile(projectileData, map, player);
        }
    }
}
