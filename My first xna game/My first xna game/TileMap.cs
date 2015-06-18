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
        private Texture2D[] tilesets;
        private List<MapCell> highCells = new List<MapCell>();
        private List<MapCell> mapCellsList = new List<MapCell>();


        public TileMap(string path, bool debugTileset = false)
        {
            TmxMap map = new TmxMap(path);

            // 1. intialize
            tilesets = new Texture2D[map.Tilesets.Count];
            for (int i = 0; i < map.Tilesets.Count; i++)
            {
                tilesets[i] = (Game.content.Load<Texture2D>("Textures\\Tilesets\\" + Path.GetFileNameWithoutExtension(map.Tilesets[i].Image.Source)));
            }
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
            for (int layersCounter = 0; layersCounter < layers.Count; layersCounter++)
            {
                Layer layer = layers[layersCounter];
                for (int y = 0; y < height; y++)
                {
                    MapRow row = layer.Rows[y];
                    for (int x = 0; x < width; x++)
                    {
                        MapCell cell = row.Columns[x];
                        int currentCell = y * (height) + x;
                        TmxLayerTile tmxCell = map.Layers[layersCounter].Tiles[currentCell];

                        //texture and tileset
                        if (tmxCell.Gid == 0)
                        {
                            cell.empty = true;
                        }
                        cell.texture = tmxCell.Gid;
                        cell.tileset = tilesets[0];
                        int tilesSoFar = map.Tilesets[0].Tiles.Count;
                        for (int tilesetsCounter = 0; tilesetsCounter < map.Tilesets.Count - 1; tilesetsCounter++)
                        {
                            if (tmxCell.Gid > tilesSoFar)
                            {
                                cell.texture = tmxCell.Gid - tilesSoFar;
                                cell.tileset = tilesets[tilesetsCounter + 1];
                            }
                            tilesSoFar += map.Tilesets[tilesetsCounter + 1].Tiles.Count;
                        }

                        //tile properties
                        TmxTilesetTile tileResult = null;
                        bool breakLoop = false;

                        for (int tilesetCounter = 0; tilesetCounter < map.Tilesets.Count; tilesetCounter++)
                        {
                            for (int tilesCounter = 0; tilesCounter < map.Tilesets[tilesetCounter].Tiles.Count; tilesCounter++)
                            {
                                if (tilesCounter == cell.texture - 1)
                                {
                                    tileResult = map.Tilesets[tilesetCounter].Tiles[tilesCounter];
                                    breakLoop = true;
                                    break;
                                }
                                if (breakLoop) { break; }
                            }
                            if (breakLoop) { break; }
                        }
                        if (tileResult != null)
                        {
                            //collision
                            if (tileResult.Properties["Passable"] == "X")
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
                            if (tileResult.Properties["Height"] == "1")
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
            //todo: srsly? why not map do this? i am rusin
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
                        int tilesetWidth = tileID.tileset.Width / Tile.size;
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
                            int tileIDY = tileID.texture / tilesetWidth;
                            if (tileID.texture < 0 || tileID.texture % tilesetWidth != 0)
                            {
                                tileIDX = tileID.texture % tilesetWidth;
                            }
                            else
                            {
                                tileIDX = tilesetWidth;
                            }
                            if (tileID.texture != tileIDY * tilesetWidth)
                            {
                                tileIDY = tileID.texture / tilesetWidth;
                            }
                            else
                            {
                                tileIDY = tileID.texture / tilesetWidth - 1;
                            }
                            spriteBatch.Draw(
                                tileID.tileset,
                                new Rectangle(
                                    positionX, positionY, Tile.size, Tile.size),
                                Tile.getTileRectangle(new Vector2(tileIDX, tileIDY)),
                                Color.White * tileID.getOpacity);
                        }
                    }
                }
            }
        }
    }
}