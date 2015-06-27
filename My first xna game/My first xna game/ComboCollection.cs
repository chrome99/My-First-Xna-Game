using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class ComboCollection
    {
        public enum PlayerKeysIndex
        {
            mvUp, mvDown, mvLeft, mvRight,
            attack, jump, defend
        }
        public static Combo fireCombo = new Combo(new ComboCollection.PlayerKeysIndex[] { PlayerKeysIndex.defend, PlayerKeysIndex.mvRight, PlayerKeysIndex.attack }, FireCombo);

        private static void FireCombo()
        {
            int asd = 123;
        }
    }
}
