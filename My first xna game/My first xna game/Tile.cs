using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    static class Tile
    {
        static public int size = 32;

        static public Rectangle getTileRectangle(int tileX, int tileY)
        {
            tileX -= 1;
            if (tileX < 0)
            {
                tileX = 0;
            }
            if (tileY < 0)
            {
                tileY = 0;
            }
            return new Rectangle(tileX * size, tileY * size, size, size);
        }
    }
}
