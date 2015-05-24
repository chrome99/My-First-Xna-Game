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
        public struct Equipment
        {
            public Armor head;
            public Armor rightHand;
            public Armor leftHand;
            public Armor chest;
            public Armor legs;
            public Armor shoes;
        }

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

            /*public Stats(int health, int maxHealth)
            {

            }*/
        }
        public bool enableRunning = true;
        public Stats stats;
        public Equipment equipment;
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

        public void Equip(Armor armor)
        {


            switch (armor.armorType)
            {
                case Armor.ArmorType.chest:
                    subArmorStats(equipment.chest);
                    equipment.chest = armor;
                    break;

                case Armor.ArmorType.head:
                    subArmorStats(equipment.head);
                    equipment.head = armor;
                    break;

                case Armor.ArmorType.twoHanded:
                    equipment.leftHand = armor;
                    equipment.rightHand = ItemCollection.occupiedArmor;
                    break;

                case Armor.ArmorType.oneHanded:
                    if (equipment.leftHand == null)
                    {
                        subArmorStats(equipment.leftHand);
                        equipment.leftHand = armor;
                    }
                    else if (equipment.rightHand == null)
                    {
                        subArmorStats(equipment.rightHand);
                        equipment.rightHand = armor;
                    }
                    else
                    {
                        subArmorStats(equipment.leftHand);
                        equipment.leftHand = armor;
                    }
                    break;

                case Armor.ArmorType.shoes:
                    subArmorStats(equipment.shoes);
                    equipment.shoes = armor;
                    break;

                case Armor.ArmorType.legs:
                    subArmorStats(equipment.legs);
                    equipment.legs = armor;
                    break;
            }
            addArmorStats(armor);
        }

        private void addArmorStats(Armor armor)
        {
            stats.health += armor.changeStats.health;
            stats.maxHealth += armor.changeStats.maxHealth;
            stats.mana += armor.changeStats.mana;
            stats.maxMana += armor.changeStats.maxMana;
            stats.strength += armor.changeStats.strength;
            stats.knockback += armor.changeStats.knockback;
            stats.cooldown += armor.changeStats.cooldown;
            stats.defence += armor.changeStats.defence;
            stats.agility += armor.changeStats.agility;
        }

        private void subArmorStats(Armor armor)
        {
            if (armor == null) { return; }
            stats.health -= armor.changeStats.health;
            stats.maxHealth -= armor.changeStats.maxHealth;
            stats.mana -= armor.changeStats.mana;
            stats.maxMana -= armor.changeStats.maxMana;
            stats.strength -= armor.changeStats.strength;
            stats.knockback -= armor.changeStats.knockback;
            stats.cooldown -= armor.changeStats.cooldown;
            stats.defence -= armor.changeStats.defence;
            stats.agility -= armor.changeStats.agility;
        }
    }
}
