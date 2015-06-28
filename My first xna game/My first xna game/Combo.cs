using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Combo
    {
        public delegate void ComboFunction(Player player);
        public ComboFunction comboFunction;

        public ComboCollection.PlayerKeysIndex[] comboKeys;

        public Combo(ComboCollection.PlayerKeysIndex[] comboKeys, ComboFunction comboFunction)
        {
            this.comboKeys = comboKeys;
            this.comboFunction = comboFunction;
        }
    }
}
