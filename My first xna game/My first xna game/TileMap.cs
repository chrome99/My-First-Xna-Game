using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using System.IO;

namespace My_first_xna_game
{
    public class TileMap
    {
        public List<GameObject> collisionObjectList = new List<GameObject>();
        public List<Layer> layers = new List<Layer>(); //todo: make it an array
        public int width;
        public int height;
        public Rectangle mapRect;
        private Texture2D tileset;
        private List<MapCell> highCells = new List<MapCell>();
        private List<MapCell> mapCellsList = new List<MapCell>();


        public TileMap(string path, bool debugTileset = false)
        {
            TmxMap map = new TmxMap(path);

            // 1. intialize
            this.tileset = Game.content.Load<Texture2D>("Textures\\Tilesets\\" + Path.GetFileNameWithoutExtension(map.Tilesets[0].Image.Source));
            this.width = map.Width; //todo: /32
            this.height = map.Height;

            for (int counter = 0; counter < map.Layers.Count; counter++)
            {
                layers.Add(new Layer());
            }
            mapRect = new Rectangle(0, 0, width * Tile.size, height * Tile.size);

            // 2. build blank map, or if you want - intializing all layers
            foreach (Layer layer in layers)
            {
                for (int y = 0; y < height; y++)
                {
                    //create new row
                    MapRow newRow = new MapRow();

                    //add to its columns new cells
                    for (int x = 0; x < width; x++)
                    {
                        newRow.Columns.Add(new MapCell());
                    }

                    //add the new row to the current layer
                    layer.Rows.Add(newRow);
                }
            }
            // 3. add map design
            for (int i = 0; i < layers.Count; i++)
            {
                Layer layer = layers[i];
                for (int y = 0; y < height; y++)
                {
                    MapRow row = layer.Rows[y];
                    for (int x = 0; x < width; x++)
                    {
                        MapCell cell = row.Columns[x];
                        int currentCell = y * (height) + x;
                        TmxLayerTile tmxCell = map.Layers[i].Tiles[currentCell];

                        //texture
                        if (tmxCell.Gid == 0)
                        {
                            cell.empty = true;
                        }
                        cell.texture = tmxCell.Gid;

                        TmxTilesetTile result = null;
                        for (int counter = 0; counter < map.Tilesets[0].Tiles.Count; counter++)
                        {
                            if (counter == tmxCell.Gid - 1)
                            {
                                result = map.Tilesets[0].Tiles[counter];
                                break;
                            }
                        }
                        if (result != null)
                        {
                            //collision
                            if (result.Properties["Passable"] == "X")
                            {
                                cell.passable = false;

                                GameObject collisionObject = new GameObject(new Vector2(tmxCell.X * Tile.size, tmxCell.Y * Tile.size));

                                if (debugTileset)
                                {
                                    collisionObject.AddLight(70, Color.White);
                                }

                                collisionObjectList.Add(collisionObject);
                            }

                            //height
                            if (result.Properties["Height"] == "1")
                            {
                                cell.high = true;
                                highCells.Add(cell);
                            }
                            else
                            {
                                mapCellsList.Add(cell);
                            }
                        }
                    }
                }
            }
        }

        public void AddCollisionObjects(Map map)
        {
            //todo: srsly? why not map do this?
            foreach (GameObject collisionObject in collisionObjectList)
            {
                map.AddObject(collisionObject);
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(height * Tile.size / 2, width * Tile.size / 2);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle screenRect, Rectangle mapRect, bool low)
        {
            Vector2 firstSquare = new Vector2(mapRect.X / Tile.size, mapRect.Y / Tile.size);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2(mapRect.X % Tile.size, mapRect.Y % Tile.size);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;
            foreach (Layer layer in layers)
            {
                for (int x = 0; x <= screenRect.Width / Tile.size; x++)
                {
                    for (int y = 0; y < screenRect.Height / Tile.size + 2; y++)
                    {
                        int positionX = x * Tile.size - offsetX;
                        int positionY = y * Tile.size - offsetY;
                        MapCell tileID = layer.Rows[(int)MathHelper.Clamp(y + firstY - 0, 0, height - 1)].Columns[(int)MathHelper.Clamp(x + firstX - 0, 0, width - 1)];
                        bool heightCheck;
                        if (low)
                        {
                            heightCheck = !tileID.high;
                        }
                        else
                        {
                            heightCheck = tileID.high;
                        }
                        if (!tileID.empty && heightCheck)
                        {
                            int tileIDX;
                            int tileIDY = tileID.texture / 8;
                            if (tileID.texture < 0 || tileID.texture % 8 != 0)
                            {
                                tileIDX = tileID.texture % 8;
                            }
                            else
                            {
                                tileIDX = 8;
                            }
                            if (tileID.texture != tileIDY * 8)
                            {
                                tileIDY = tileID.texture / 8;
                            }
                            else
                            {
                                tileIDY = tileID.texture / 8 - 1;
                            }
                            spriteBatch.Draw(
                                tileset,
                                new Rectangle(
                                    positionX, positionY, Tile.size, Tile.size),
                                Tile.getTileRectangle(new Vector2(tileIDX, tileIDY)),
                                Color.White * tileID.getOpacity);
                        }
                    }
                }
            }
        }

        /*public void DrawLow(SpriteBatch spriteBatch, Rectangle screenRect, Rectangle mapRect)
        {
            //UpdateDrawingVariables(screenRect, mapRect);
            foreach (MapCell cell in mapCellsList)
            {
                spriteBatch.Draw(
                    tileset,
                    new Rectangle(
                        cell.positionX, cell.positionY, Tile.size, Tile.size),
                    Tile.getTileRectangle(cell.tileIdPosition),
                    Color.White * cell.getOpacity);
            }
        }

        public void DrawHigh(SpriteBatch spriteBatch, Rectangle screenRect, Rectangle mapRect)
        {
            //UpdateDrawingVariables(screenRect, mapRect);
            foreach (MapCell cell in highCells)
            {
                if (!cell.empty)
                {
                    spriteBatch.Draw(
                        tileset,
                        new Rectangle(
                            cell.positionX, cell.positionY, Tile.size, Tile.size),
                        Tile.getTileRectangle(cell.tileIdPosition),
                        Color.White * cell.getOpacity);
                }
            }
        }*/

    }
}
