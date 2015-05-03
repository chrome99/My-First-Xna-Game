using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace My_first_xna_game
{
    public class Hostile : Actor
    {
        public struct Stats
        {
            public int health;
            public int maxHealth;
            public int mana;
            public int maxMana;
            public int strength;
            public int knockback;
            public float cooldown;
            public int defence;
            public int agility;

            /*Stats(int health, int maxHealth, int mana, int maxMana, int strength, int knockback, int defence, int agility)
            {
                this.health = health;
                this.maxHealth = maxHealth;
                this.mana = mana;
                this.maxMana = maxMana;
                this.strength = strength;
                this.knockback = knockback;
                this.defence = defence;
                this.agility = agility;
            }*/
        }
        public bool enableRunning = true;
        public Stats stats;
        public Timer cooldownTimer = new Timer(0f, false);

        public Hostile(Texture2D texture, Vector2 position, MovementManager.Auto autoMovement = MovementManager.Auto.off)
            : base(texture, position, autoMovement)
        {
            //update timer max
            cooldownTimer.max = stats.cooldown;
        }

        protected override void UpdateHostile()
        {
            UpdateEnemy();

            //if killed
            if (stats.health <= 0)
            {
                Kill();
            }
        }

        protected virtual void UpdateEnemy() { }
        protected virtual void UpdatePlayer() { }

        public void DealDamage(Hostile source)
        {
            if (cooldownTimer.result || cooldownTimer.counter == 0)
            {
                stats.health -= source.stats.strength;
                movementManager.Knockback(this, source.direction, source.stats.knockback);
                cooldownTimer.timerSwitch = true;
                cooldownTimer.counter = 0f;
            }
        }
    }
}
