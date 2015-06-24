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

            //if killed
            if (stats.health <= 0)
            {
                Fade();
                Kill();
            }
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
        public virtual void UpdatePlayer(GameTime gameTime, KeyboardState newState, KeyboardState oldState) { }

        public void DealDamage(Hostile source, int damage = 0, bool showDamage = true)
        {
            if (cooldownTimer.result || cooldownTimer.counter == 0)
            {
                //set new health
                bool noDamage = false;
                bool defending =false;
                int oldHealth = stats.health;
                int newHealth;
                Player player = this as Player;
                if (player != null)
                {
                    if (player.defendingTimer.Counting)
                    {
                        newHealth = oldHealth;
                        defending = true;
                    }
                    else if (damage == 0) // new health by number
                    {
                        newHealth = stats.health - source.stats.strength + stats.defence;
                    }
                    else //new health by stats
                    {
                        newHealth = stats.health - damage + stats.defence;
                    }
                }
                else
                {
                    if (damage == 0)
                    {
                        newHealth = stats.health - source.stats.strength + stats.defence;
                    }
                    else
                    {
                        newHealth = stats.health - damage + stats.defence;
                    }
                }
                
                //limit damage to more than zero
                if (oldHealth - newHealth > -1)
                {
                    // limit health to zero
                    if (newHealth > -1)
                    {
                        stats.health = newHealth;
                    }
                    else
                    {
                        stats.health = 0;
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
                if (player != null)
                {
                    player.HandleHit(oldHealth - newHealth);
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
                subArmorStats(armor);
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
            addArmorStats(armor);
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

        public void DrawDmg(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (dmgText != null)
            {
                dmgText.Draw(spriteBatch, offsetRect);
            }
        }
    }
}