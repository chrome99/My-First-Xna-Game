using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    class SkillCollection
    {
        public static ProjectileSkill ballOfDestruction = new ProjectileSkill(Game.content.Load<Texture2D>("Textures\\SkillsPictures\\fire"),
            Color.OrangeRed, "Ball of Destruction", 104, new Skill.SkillStats() { cost = 1 }, new Projectile.ProjectileData
            {
                texture = Game.content.Load<Texture2D>("Textures\\Spritesheets\\fireball"),
                hitSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
                launchSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"),
                lit = true,
                lightLevel = 150,
                lightColor = Color.Red,
                lightOpacity = 100,
                pathDestination = 150,
                speed = 5,
                strength = 2
            });

        public static ProjectileSkill ballOfDestruction2 = new ProjectileSkill(Game.content.Load<Texture2D>("Textures\\SkillsPictures\\fire"),
    Color.OrangeRed, "Ball of Destruction", 202, new Skill.SkillStats() { cost = 1 }, new Projectile.ProjectileData
    {
        texture = Game.content.Load<Texture2D>("Textures\\Spritesheets\\fireball"),
        hitSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
        launchSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"),
        lit = true,
        lightLevel = 150,
        lightColor = Color.Red,
        lightOpacity = 100,
        pathDestination = 150,
        speed = 5,
        strength = 2
    });

        public static ProjectileSkill ballOfDestruction3 = new ProjectileSkill(Game.content.Load<Texture2D>("Textures\\SkillsPictures\\fire"),
    Color.OrangeRed, "Ball of Destruction", 203, new Skill.SkillStats() { cost = 1 }, new Projectile.ProjectileData
    {
        texture = Game.content.Load<Texture2D>("Textures\\Spritesheets\\fireball"),
        hitSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
        launchSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"),
        lit = true,
        lightLevel = 150,
        lightColor = Color.Red,
        lightOpacity = 100,
        pathDestination = 150,
        speed = 5,
        strength = 2
    });

        public static ProjectileSkill ballOfDestruction4 = new ProjectileSkill(Game.content.Load<Texture2D>("Textures\\SkillsPictures\\fire"),
Color.OrangeRed, "Ball of Destruction", 204, new Skill.SkillStats() { cost = 1 }, new Projectile.ProjectileData
{
    texture = Game.content.Load<Texture2D>("Textures\\Spritesheets\\fireball"),
    hitSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
    launchSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"),
    lit = true,
    lightLevel = 150,
    lightColor = Color.Red,
    lightOpacity = 100,
    pathDestination = 150,
    speed = 5,
    strength = 2
});

        public static Skill wallOfFire = new MapSkill(Game.content.Load<Texture2D>("Textures\\SkillsPictures\\ice"),
            Color.LightCyan, "Wall of Fire", 135, new Skill.SkillStats() { cost = 1 },
            new Sprite(Game.content.Load<Texture2D>("Textures\\Sprites\\box1"), Vector2.Zero), 5);

        public static List<Skill> list = new List<Skill>() { ballOfDestruction, wallOfFire };
    }
}
