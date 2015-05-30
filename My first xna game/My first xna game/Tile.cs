using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    static class Tile
    {
        static public int size = 32;

        static public Rectangle getTileRectangle(Vector2 tile)
        {
            int tileX = (int)tile.X;
            int tileY = (int)tile.Y;
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
