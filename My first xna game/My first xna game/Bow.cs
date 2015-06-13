using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
        [Serializable]
    public class Bow : Armor
    {
        public struct ProjectileData
        {
            public Texture2D texture;
            public SoundEffect launchSound;
            public SoundEffect hitSound;
            public bool lit;
            public int lightLevel;
            public Color lightColor;
            public int lightOpacity;
            public int speed;
            public int pathDestination;
        }
        ProjectileData projectileData;

        public Bow() { }

        public Bow(int iconID, int price, float weight, int durability, ArmorType armorType, ProjectileData projectileData, Player.Stats changeStats)
            : base(iconID, price, weight, durability, armorType, changeStats)
        {
            this.projectileData = projectileData;
        }

        public void Attack(Map map, Player player)
        {
            Durability--;

            Projectile projectile = new Projectile(projectileData.texture, projectileData.speed, player,
                projectileData.pathDestination, projectileData.launchSound, projectileData.hitSound);

            if (projectileData.lit)
            {
                projectile.AddLight(projectileData.lightLevel, projectileData.lightColor, projectileData.lightOpacity);
            }

            projectile.AddToGameObjectList(map);

            projectile.passable = true;
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
