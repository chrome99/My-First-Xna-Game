using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class PlayerManager
    {
        public static List<Player> playersList;
        public static List<Player> playersOnShop = new List<Player>();

        static PlayerManager()
        {
            //add players to PlayerManager
            playersList = new List<Player> { PlayerCollection.player1, PlayerCollection.player2, PlayerCollection.player3, PlayerCollection.player4 };
        }

        public static void UpdatePlayersMapParameters(KeyboardState newState, KeyboardState oldState)
        {
            foreach (Player player in playersList)
            {
                player.UpdateMapParameters(newState, oldState);
            }
        }

        public static bool TwoPlayersSameShop(Player currentPlayer, Vector2 merchantPosition)
        {
            if (playersOnShop.Count > 0)
            {
                List<Player> otherPlayersOnInventory = playersOnShop;
                otherPlayersOnInventory.Remove(currentPlayer);
                foreach (Player otherPlayer in otherPlayersOnInventory)
                {
                    if (merchantPosition == otherPlayer.getMerchantPosition())
                    {
                        return true;
                    }
                    else
                    {
                        playersOnShop.Add(currentPlayer);
                        return false;
                    }
                }
            }
            playersOnShop.Add(currentPlayer);
            return false;
        }
    }
}
