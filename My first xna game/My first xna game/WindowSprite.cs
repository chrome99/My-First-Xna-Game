using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class WindowSprite
    {
        int width;
        int height;
        Texture2D texture;
        List<WindowTile> tilesList;

        public WindowSprite(int width, int height, Texture2D texture)
        {
            this.width = (int)(width / WindowTile.size);
            this.height = (int)(height / WindowTile.size);
            this.texture = texture;

            CreateTiles();
        }

        private void CreateTiles()
        {
            tilesList = new List<WindowTile>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    WindowTile windowTile = new WindowTile();
                    windowTile.position = new Vector2(x * WindowTile.size, y * WindowTile.size);
                    if (x == 0)
                    {
                        if (y == 0)
                        {
                            windowTile.textureRect = WindowTile.leftUpCorner;
                        }
                        else if (y == height - 1)
                        {
                            windowTile.textureRect = WindowTile.leftDownCorner;
                        }
                        else
                        {
                            windowTile.textureRect = WindowTile.left;
                        }
                    }
                    else if (x == width - 1)
                    {
                        if (y == 0)
                        {
                            windowTile.textureRect = WindowTile.rightUpCorner;
                        }
                        else if (y == height - 1)
                        {
                            windowTile.textureRect = WindowTile.rightDownCorner;
                        }
                        else
                        {
                            windowTile.textureRect = WindowTile.right;
                        }
                    }
                    else
                    {
                        if (y == 0)
                        {
                            windowTile.textureRect = WindowTile.up;
                        }
                        else if (y == height - 1)
                        {
                            windowTile.textureRect = WindowTile.down;
                        }
                        else
                        {
                            windowTile.textureRect = WindowTile.middle;
                        }
                    }
                    tilesList.Add(windowTile);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawingPosition, float drawingOpacity, float depth)
        {
            foreach (WindowTile tile in tilesList)
            {
                spriteBatch.Draw(texture, drawingPosition + tile.position, tile.textureRect, Color.White * drawingOpacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
            }

            //spriteBatch.Draw(texture, drawingPosition, windowRect, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }
    }

    class WindowTile
    {
        public static Rectangle left = new Rectangle(0, 32, 32, 32);
        public static Rectangle right = new Rectangle(96, 32, 32, 32);
        public static Rectangle up = new Rectangle(32, 0, 32, 32);
        public static Rectangle down = new Rectangle(32, 96, 32, 32);

        public static Rectangle middle = new Rectangle(32, 32, 32, 32);

        public static Rectangle leftUpCorner = new Rectangle(0, 0, 32, 32);
        public static Rectangle leftDownCorner = new Rectangle(0, 96, 32, 32);
        public static Rectangle rightUpCorner = new Rectangle(96, 0, 32, 32);
        public static Rectangle rightDownCorner = new Rectangle(96, 96, 32, 32);

        public const int size = 32;

        public Vector2 position;
        public Rectangle textureRect;
    }
}
