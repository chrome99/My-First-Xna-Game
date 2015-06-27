using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class SkillCollection
    {
        public static Skill fire = new Skill(Game.content.Load<Texture2D>("Textures\\SkillsPictures\\fire"), Color.Red, "Wall of Fire");
        public static Skill ice = new Skill(Game.content.Load<Texture2D>("Textures\\SkillsPictures\\ice"), Color.LightBlue, "Godess Of Ice");
    }
}
