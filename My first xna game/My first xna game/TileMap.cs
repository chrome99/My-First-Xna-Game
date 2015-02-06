using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class TileMap
    {
        public enum MapDesign
        {
            blank, trees
        }
        MapDesign mapDesign;
        public List<MapRow> Rows = new List<MapRow>();
        public int width;
        public int height;
        private Texture2D tileset;
        private Rectangle rect;
        private float above = Game.DepthToFloat(Game.Depth.above);
        private float below = Game.DepthToFloat(Game.Depth.background);


        public TileMap(Texture2D tileset, MapDesign mapDesign = MapDesign.blank, int width = 20, int height = 15)
        {
            // 1. intialize
            this.tileset = tileset;
            this.mapDesign = mapDesign;
            this.width = width;
            this.height = height;

            rect = new Rectangle(0, 0, width * Tile.size, height * Tile.size);

            // 2. build blank map
            for (int y = 0; y <= height; y++)
            {
                //create new row
                MapRow newRow = new MapRow();

                //add to its columns new cells
                for (int x = 0; x <= width; x++)
                {
                    newRow.Columns.Add(new MapCell(new Layer()));
                }

                //add the new row to the map
                Rows.Add(newRow);
            }

            // 3. add map design
            switch (mapDesign)
            {
                case MapDesign.blank:
                    break;

                case MapDesign.trees:
                    //tiles
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            Layer newLayer = new Layer();
                            newLayer.texture = 40 + x + y * 8;
                            //newLayer.opacity = 50f;
                            if (y == 4)
                            {
                                newLayer.depth = below - Rows[y + 5].Columns[x + 5].layers.Count / 100f;
                            }
                            else
                            {
                                newLayer.depth = above;
                            }
                            Rows[y + 5].Columns[x + 5].layers.Add(newLayer);
                        }
                    }
                    break;
            }

        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(height * Tile.size / 2, width * Tile.size / 2);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle screenRect, Rectangle mapRect)
        {
            Vector2 firstSquare = new Vector2(mapRect.X / Tile.size, mapRect.Y / Tile.size);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2(mapRect.X % Tile.size, mapRect.Y % Tile.size);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;
            for (int x = screenRect.X / Tile.size; x < (screenRect.X + screenRect.Width) / Tile.size; x++)
            {
                for (int y = screenRect.Y / Tile.size; y < (screenRect.Y + screenRect.Height) / Tile.size; y++)
                {
                    int positionX = x * Tile.size - offsetX;
                    int positionY = y * Tile.size - offsetY;
                    foreach (Layer tileID in Rows[y + firstY].Columns[x + firstX].layers)
                    {
                        spriteBatch.Draw(
                            tileset,
                            new Rectangle(
                                positionX, positionY, Tile.size, Tile.size),
                            Tile.getTileRectangle(tileID.texture % 8, tileID.texture / 8),
                            Color.White * tileID.getOpacity
                            , 0f
                            , Vector2.Zero
                            , SpriteEffects.None
                            , tileID.depth);
                    }
                }
            }
        }
    }

    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }
}
