using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Skill
    {
        public struct SkillStats
        {
            public int cost;
            public int strength;
        }
        public Texture2D choosePicture;
        public Color color;
        public string name;
        protected SkillStats stats;

        public Skill(Texture2D choosePicture, Color color, string name, SkillStats stats)
        {
            this.choosePicture = choosePicture;
            this.color = color;
            this.name = name;
            this.stats = stats;
        }

        public void Use(Map map, Player player)
        {
            if (player.stats.mana >= stats.cost)
            {
                player.stats.mana -= stats.cost;
                UseSkill(map, player);
            }
            else
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
            }
        }

        protected virtual void UseSkill(Map map, Player player) { }
    }

    class ProjectileSkill : Skill
    {
        public Projectile.ProjectileData projectileData;

        public ProjectileSkill(Texture2D choosePicture, Color color, string name, SkillStats stats, Projectile.ProjectileData projectileData)
            : base(choosePicture, color, name, stats)
        {
            this.projectileData = projectileData;
        }

        protected override void UseSkill(Map map, Player player)
        {
            Projectile.LaunchProjectile(projectileData, map, player);
        }
    }

    class MapSkill : Skill
    {
        private Sprite mine;

        public MapSkill(Texture2D choosePicture, Color color, string name, SkillStats stats, Sprite mine)
            : base(choosePicture, color, name, stats)
        {
            this.mine = mine;
        }

        protected override void UseSkill(Map map, Player player)
        {
            Mine newMine = new Mine(player, stats.strength, mine);
            
            newMine.source = player;
            map.AddObject(newMine);
        }
    }
}
