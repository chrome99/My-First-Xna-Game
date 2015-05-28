using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    public class Armor : Item
    {
        public enum ArmorType { head, oneHanded, twoHanded, body, shoes }
        public ArmorType armorType;
        public Player.Stats changeStats;
        public Hostile source;
        private int durability;
        public int Durability
        {
            get { return durability; }
            set
            {
                if (value < 1)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\fart");
                    source.pack.SubItem(this);
                    source.UnEquip(this);
                }
                else
                {
                    durability = value;
                }
            }
        }

        public Armor(int iconID, int price, float weight, int durability, ArmorType armorType, Player.Stats changeStats)
            : base(iconID, null, price, weight, false)
        {
            this.durability = durability;
            this.armorType = armorType;
            this.changeStats = changeStats;
        }
    }
}
