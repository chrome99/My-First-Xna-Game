using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Hostile : Actor
    {
        public struct Equipment
        {
            public Armor head;
            public Armor body;
            public Armor shoes;
            public Armor rightHand;
            public Armor leftHand;
        }

        public struct Stats
        {
            public int health;
            public int maxHealth;
            public int mana;
            public int maxMana;

            public int strength;
            public int defence;
            public int agility;

            public int knockback;
        }

        private float cooldown;
        public float Cooldown
        {
            get { return cooldown; }
            set
            {
                cooldownTimer.max = value;
                cooldown = value;
            }
        }

        public bool enableRunning = true;
        public Stats stats;
        public Equipment equipment;
        public List<Armor> equipmentList = new List<Armor>();
        private Timer cooldownTimer = new Timer(1000f, false);

        public Hostile(Texture2D texture, Vector2 position, MovementManager.Auto autoMovement = MovementManager.Auto.off)
            : base(texture, position, autoMovement)
        {

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
                //set new health
                int oldHealth = stats.health;
                int newHealth;
                Player player = this as Player;
                if (player != null)
                {
                    if (player.defendingTimer.Counting)
                    {
                        newHealth = oldHealth;
                    }
                    else
                    {
                        newHealth = stats.health - source.stats.strength + stats.defence;
                    }
                }
                else
                {
                    newHealth = stats.health - source.stats.strength + stats.defence;
                }
                if (newHealth > -1)
                {
                    stats.health = newHealth;
                }
                else
                {
                    stats.health = 0;
                }

                //knockback
                movementManager.Knockback(this, source.direction, source.stats.knockback);

                //durability
                for (int counter = 0; counter < equipmentList.Count; counter++)
                {
                    Armor armor = equipmentList[counter];
                    if (armor.armorType != Armor.ArmorType.oneHanded && armor.armorType != Armor.ArmorType.twoHanded)
                    {
                        armor.Durability--;
                    }
                }

                //update player hud and menu
                if (player != null)
                {
                    player.HandleHit(oldHealth - newHealth);
                }

                //timer
                cooldownTimer.Reset();
            }
        }

        public void UnEquip(Armor armor)
        {
            if (equipmentList.Contains(armor))
            {
                equipmentList.Remove(armor);
                subArmorStats(armor);
                switch (armor.armorType)
                {
                    case Armor.ArmorType.body:
                        equipment.body = null;
                        break;

                    case Armor.ArmorType.head:
                        equipment.head = null;
                        break;

                    case Armor.ArmorType.twoHanded:
                        equipment.leftHand = null;
                        equipment.rightHand = null;
                        break;

                    case Armor.ArmorType.oneHanded:
                        equipment.leftHand = null;
                        break;

                    case Armor.ArmorType.shoes:
                        equipment.shoes = null;
                        break;
                }
            }
        }

        public void Equip(Armor armor)
        {
            switch (armor.armorType)
            {
                case Armor.ArmorType.body:
                    subArmorStats(equipment.body);
                    equipmentList.Remove(equipment.body);
                    equipment.body = armor;
                    break;

                case Armor.ArmorType.head:
                    subArmorStats(equipment.head);
                    equipmentList.Remove(equipment.body);
                    equipment.head = armor;
                    break;

                case Armor.ArmorType.twoHanded:
                    equipmentList.Remove(equipment.rightHand);
                    equipmentList.Remove(equipment.leftHand);
                    subArmorStats(equipment.rightHand);
                    subArmorStats(equipment.leftHand);
                    equipment.leftHand = armor;
                    equipment.rightHand = ItemCollection.occupiedArmor;
                    break;

                case Armor.ArmorType.oneHanded:
                    if (equipment.leftHand == null)
                    {
                        equipmentList.Remove(equipment.leftHand);
                        subArmorStats(equipment.leftHand);
                        equipment.leftHand = armor;
                    }
                    else if (equipment.rightHand == null)
                    {
                        equipmentList.Remove(equipment.rightHand);
                        subArmorStats(equipment.rightHand);
                        equipment.rightHand = armor;
                    }
                    else
                    {
                        equipmentList.Remove(equipment.leftHand);
                        subArmorStats(equipment.leftHand);
                        equipment.leftHand = armor;
                    }
                    break;

                case Armor.ArmorType.shoes:
                    equipmentList.Remove(equipment.shoes);
                    subArmorStats(equipment.shoes);
                    equipment.shoes = armor;
                    break;
            }
            armor.source = this;
            equipmentList.Add(armor);
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
            stats.defence -= armor.changeStats.defence;
            stats.agility -= armor.changeStats.agility;
        }
    }
}
