using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class ComboCollection
    {
        public enum PlayerKeysIndex
        {
            move,
            mvUp, mvDown, mvLeft, mvRight,
            attack, jump, defend
        }
        public static Combo fireCombo = new Combo("fire combo", new ComboCollection.PlayerKeysIndex[] { PlayerKeysIndex.defend, PlayerKeysIndex.move, PlayerKeysIndex.attack }, FireCombo);

        public static List<Combo> list = new List<Combo>() { fireCombo };

        private static void FireCombo(Player player)
        {
            Projectile.LaunchProjectile(new Projectile.ProjectileData
            {
                texture = Game.content.Load<Texture2D>("Textures\\Spritesheets\\blue fireball"),
                hitSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball launch"),
                launchSound = Game.content.Load<SoundEffect>("Audio\\Waves\\fireball hit"),
                lit = true,
                lightLevel = 150,
                lightColor = Color.LightBlue,
                lightOpacity = 100,
                pathDestination = 400,
                speed = 4,
                exploading = false
            }, player.map, player);
        }
    }
}
