using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class PlayerManager
    {
        public static List<Player> playersList;

        public static void UpdatePlayersMapParameters(Map map, KeyboardState newState, KeyboardState oldState)
        {
            foreach (Player player in playersList)
            {
                player.UpdateMapParameters(map, newState, oldState);
            }
        }
        /*
        public static bool TwoPlayersSameShop(int itemID)
        {
            List<Player> playersOnInventory = new List<Player>();
            foreach (Player player in playersList)
            {
                if (player.IsOnInventory())
                {
                    playersOnInventory.Add(player);
                }
            }

            if (playersOnInventory.Count > 1)
            {
                Player currentPlayer = null;
                foreach(Player currentPlayer2 in playersList)
                {
                    if (currentPlayer2.shop.buyInventory.window.itemsList.Count == currentPlayer2.shop.merchant.pack.items.Count - 1)
                    {
                        currentPlayer = currentPlayer2;
                        break;
                    }
                }
                List<Player> otherPlayersOnInventory = playersOnInventory;
                otherPlayersOnInventory.Remove(currentPlayer);
                foreach (Player otherPlayer in otherPlayersOnInventory)
                {
                    bool result = currentPlayer.pauseState == otherPlayer.pauseState && currentPlayer.getMerchantPosition() == otherPlayer.getMerchantPosition();
                    if (result)
                    {
                        if (otherPlayer.position == currentPlayer.position)
                        {
                            int asdasd = otherPlayer.shop.buyInventory.window.itemsList.Count;

                        }
                        int asdasdsdf = currentPlayer.shop.buyInventory.window.itemsList.Count;
                        currentPlayer.UpdateShopItems(itemID, currentPlayer.getBuyInventoryOrSellInventory());
                    }
                    return result;
                }
            }
            return false;
        }*/
    }
}
