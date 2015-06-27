using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Bow : Armor
    {
        Projectile.ProjectileData projectileData;

        public Bow(int iconID, int price, float weight, int durability, ArmorType armorType, Projectile.ProjectileData projectileData, Player.Stats changeStats)
            : base(iconID, price, weight, durability, armorType, changeStats)
        {
            this.projectileData = projectileData;
        }

        public void Attack(Map map, Player player)
        {
            Durability--;

            Projectile.LaunchProjectile(projectileData, map, player);
        }
        /*
                     Projectile projectile = new Projectile(Game.content.Load<Texture2D>("Textures\\Spritesheets\\wolf"),
                6, player, 60, Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
                Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"));
            projectile.AddLight(150, Color.Red);
            projectile.AddToGameObjectList(map);
            projectile.passable = true;
         */
    }
}
