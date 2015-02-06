using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    static class Tile
    {
        static public int size = 32;

        static public Rectangle getTileRectangle(int tileX, int tileY)
        {
            return new Rectangle(tileX * size, tileY * size, size, size);
        }
    }
}
