using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            public int exp;
            public int level;
        }

        public struct Stat
        {
            public int value;
            public string name;
        }

        public static List<Stat> StatsToStatList(Stats stats)
        {
            List<Stat> result = new List<Stat>();
            if (stats.agility != 0)
            {
                result.Add(new Stat { value = stats.agility, name = "Agility" });
            }
            if (stats.defence != 0)
            {
                result.Add(new Stat() { value = stats.defence, name = "Defence" });
            }
            if (stats.exp != 0)
            {
                result.Add(new Stat() { value = stats.exp, name = "Experience" });
            }
            if (stats.health != 0)
            {
                result.Add(new Stat() { value = stats.health, name = "Health" });
            }
            if (stats.knockback != 0)
            {
                result.Add(new Stat() { value = stats.knockback, name = "Knockback" });
            }
            if (stats.level != 0)
            {
                result.Add(new Stat() { value = stats.level, name = "Level" });
            }
            if (stats.mana != 0)
            {
                result.Add(new Stat() { value = stats.mana, name = "Mana" });
            }
            if (stats.maxHealth != 0)
            {
                result.Add(new Stat() { value = stats.maxHealth, name = "Max Health" });
            }
            if (stats.maxMana != 0)
            {
                result.Add(new Stat() { value = stats.maxMana, name = "Max Mana" });
            }
            if (stats.strength != 0)
            {
                result.Add(new Stat() { value = stats.strength, name = "Strength" });
            }

            return result;
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
        protected List<Skill> skillsList = new List<Skill>();
        private Timer cooldownTimer = new Timer(1000f, false);

        private Text dmgText;
        private Timer dmgTextTimer = new Timer(400f, false);
        private int dmgTextFlyingAnimation = 0;

        public Hostile(Texture2D texture, Vector2 position, MovementManager.Auto autoMovement = MovementManager.Auto.off)
            : base(texture, position, autoMovement)
        {

        }

        protected override void UpdateHostile()
        {
            UpdateEnemy();
        }

        protected override void UpdateAnyway()
        {
            if (dmgText != null)
            {
                if (!dmgText.visible)
                {
                    dmgTextFlyingAnimation = 0;
                }
                if (dmgTextTimer.Counting)
                {
                    dmgTextFlyingAnimation++;
                    dmgText.position = position;
                    dmgText.position.X += bounds.Width / 2 - dmgText.bounds.Width / 2;
                    dmgText.position.Y -= dmgText.bounds.Height - 20;
                    dmgText.position.Y -= dmgTextFlyingAnimation;
                }
                if (dmgTextTimer.result)
                {
                    dmgText.Fade();
                    dmgTextTimer.Reset();
                }
                dmgText.UpdateFade();
            }
        }

        protected virtual void UpdateEnemy() { }

        public void LearnSkill(Skill skill)
        {
            skillsList.Add(skill);
        }

        public void ForgetSkill(Skill skill)
        {
            skillsList.Remove(skill);
        }

        public void DealDamage(Hostile source, bool sourceDamage = true, int damage = 0, bool showDamage = true)
        {
            if (cooldownTimer.result || cooldownTimer.counter == 0)
            {
                //set new health
                bool noDamage = false;
                bool defending =false;
                int oldHealth = stats.health;
                int newHealth = stats.health;
                Player thisAsPlayer = this as Player;
                if (thisAsPlayer != null)
                {
                    if (thisAsPlayer.defendingTimer.Counting)
                    {
                        defending = true;
                    }
                    else
                    {
                        if (damage == 0) // new health by stats
                        {
                            newHealth += stats.defence - source.stats.strength;
                        }
                        if (!sourceDamage) //new health by number
                        {
                            newHealth += stats.defence - damage;
                        }
                    }
                }
                else
                {
                    if (damage == 0) // new health by stats
                    {
                        newHealth += stats.defence - source.stats.strength;
                    }
                    if (!sourceDamage) //new health by number
                    {
                        newHealth += stats.defence - damage;
                    }
                }
                
                //limit damage to more than zero
                if (oldHealth - newHealth > -1)
                {
                    // limit health to zero
                    if (newHealth > 0)
                    {
                        stats.health = newHealth;
                    }
                    else
                    {
                        stats.health = 0;
                        Player sourceAsPlayer = source as Player;
                        if (sourceAsPlayer != null)
                        {
                            sourceAsPlayer.AddExp(stats.exp);
                        }
                        fade = true;
                        Kill();
                    }
                }
                else
                {
                    noDamage = true;
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
                if (thisAsPlayer != null)
                {
                    thisAsPlayer.HandleHit(oldHealth - newHealth);
                }

                //timer
                cooldownTimer.Reset();

                //show damage
                if (showDamage)
                {
                    string text;
                    Color color;
                    if (defending)
                    {
                        text = "Defending";
                        color = Color.DimGray;
                    }
                    else if (noDamage)
                    {
                        text = "0";
                        color = Color.DimGray;
                    }
                    else
                    {
                        text = "" + (oldHealth - newHealth);
                        color = Color.Red;
                    }
                    dmgText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, color, text, null, new Vector2(10, 10));

                    dmgText.position = position;
                    dmgText.position.X += bounds.Width / 2 - dmgText.bounds.Width / 2;
                    dmgText.position.Y -= dmgText.bounds.Height - 20;

                    dmgTextTimer.Active();
                }
            }
        }

        public void UnEquip(Armor armor)
        {
            if (equipmentList.Contains(armor))
            {
                if (armor != null)
                {
                    SubStats(armor.changeStats);
                }
                equipmentList.Remove(armor);
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

        public int checkIfEquiped(Armor armor)
        {
            //0 is nothing, 1 is equiped or left hand, 2 is right hand, and 3 is both right and left hands

            if (armor.armorType != Armor.ArmorType.oneHanded)
            {
                if (equipmentList.Contains(armor))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (equipment.leftHand == null)
                {
                    if (equipment.rightHand == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    if (equipment.rightHand == null)
                    {
                        return 1;
                    }
                    else if (equipment.leftHand.armorType != Armor.ArmorType.twoHanded)
                    {
                        return 3;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public void Equip(Armor armor)
        {
            switch (armor.armorType)
            {
                case Armor.ArmorType.body:
                    UnEquip(equipment.body);
                    equipment.body = armor;
                    break;

                case Armor.ArmorType.head:
                    UnEquip(equipment.head);
                    equipment.head = armor;
                    break;

                case Armor.ArmorType.twoHanded:
                    UnEquip(equipment.leftHand);
                    equipment.rightHand = null;
                    equipment.leftHand = armor;
                    break;

                case Armor.ArmorType.oneHanded:
                    if (leftHandOrRightHand())
                    {
                        UnEquip(equipment.leftHand);
                        equipment.leftHand = armor;
                    }
                    else
                    {
                        UnEquip(equipment.rightHand);
                        equipment.rightHand = armor;
                    }
                    break;

                case Armor.ArmorType.shoes:
                    UnEquip(equipment.shoes);
                    equipment.shoes = armor;
                    break;
            }
            armor.source = this;
            equipmentList.Add(armor);
            AddStats(armor.changeStats);
        }

        public bool leftHandOrRightHand()
        {
            if (equipment.leftHand != null)
            {
                if (equipment.leftHand.armorType == Armor.ArmorType.twoHanded)
                {
                    return true;
                }
                else
                {
                    if (equipment.rightHand == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        public void AddStats(Stats changeStats)
        {
            stats.health += changeStats.health;
            stats.maxHealth += changeStats.maxHealth;
            stats.mana += changeStats.mana;
            stats.maxMana += changeStats.maxMana;
            stats.strength += changeStats.strength;
            stats.knockback += changeStats.knockback;
            stats.defence += changeStats.defence;
            stats.agility += changeStats.agility;
            stats.exp += changeStats.exp;
            stats.level += changeStats.level;
        }

        public void SubStats(Stats changeStats)
        {
            stats.health -= changeStats.health;
            stats.maxHealth -= changeStats.maxHealth;
            stats.mana -= changeStats.mana;
            stats.maxMana -= changeStats.maxMana;
            stats.strength -= changeStats.strength;
            stats.knockback -= changeStats.knockback;
            stats.defence -= changeStats.defence;
            stats.agility -= changeStats.agility;
            stats.exp -= changeStats.exp;
            stats.level -= changeStats.level;
        }

        public void DrawDmg(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (dmgText != null)
            {
                dmgText.DrawWithoutSource(spriteBatch, offsetRect);
            }
        }
    }
}