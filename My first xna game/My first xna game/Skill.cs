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
        }
        public Texture2D choosePicture;
        public Color color;
        public string name;
        private int iconID;
        protected SkillStats stats;

        public Skill(Texture2D choosePicture, Color color, string name, int iconID, SkillStats stats)
        {
            this.choosePicture = choosePicture;
            this.color = color;
            this.name = name;
            this.iconID = iconID;
            this.stats = stats;
        }

        public Rectangle getRect
        {
            get { return new Rectangle(iconID % Item.itemsInRow * Item.size, iconID / Item.itemsInRow * Item.size, Item.size, Item.size); }
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

        public ProjectileSkill(Texture2D choosePicture, Color color, string name, int iconID, SkillStats stats, Projectile.ProjectileData projectileData)
            : base(choosePicture, color, name, iconID, stats)
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
        private int strength;

        public MapSkill(Texture2D choosePicture, Color color, string name, int iconID, SkillStats stats, Sprite mine, int strength)
            : base(choosePicture, color, name, iconID, stats)
        {
            this.mine = mine;
            this.strength = strength;
        }

        protected override void UseSkill(Map map, Player player)
        {
            Mine newMine = new Mine(player, strength, mine);
            
            newMine.source = player;
            map.AddObject(newMine);
        }
    }
}
