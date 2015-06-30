using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    public class Combo
    {
        public delegate void ComboFunction(Player player);
        public ComboFunction comboFunction;
        public string name;

        public ComboCollection.PlayerKeysIndex[] comboKeys;

        public Combo(string name, ComboCollection.PlayerKeysIndex[] comboKeys, ComboFunction comboFunction)
        {
            this.name = name;
            this.comboKeys = comboKeys;
            this.comboFunction = comboFunction;
        }
    }
}
