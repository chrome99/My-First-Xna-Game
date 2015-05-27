using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
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
        //private TmxMap map;
        private Texture2D tileset;
        private Rectangle mapRect;
        private float above = Game.DepthToFloat(Game.Depth.above);
        private float below = Game.DepthToFloat(Game.Depth.background);


        public TileMap(string path)
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

                        //depth
                        float newDepth = layers.Count - layers.IndexOf(layer);
                        cell.depth += newDepth / 1000;

                        //texture
                        if (tmxCell.Gid == 0)
                        {
                            cell.empty = true;
                        }
                        cell.texture = tmxCell.Gid;

                        //collision
                        /*
                        TmxTilesetTile result = null;
                        for (int counter = 0; counter < map.Tilesets[0].Tiles.Count; counter++)
                        {
                            if (map.Tilesets[0].Tiles[counter].Id == tmxCell.Gid)
                            {
                                result = map.Tilesets[0].Tiles[counter];
                                break;
                            }
                        }
                        if (result != null)
                        {
                            if (result.Properties["unPassable"].Length != 0)
                            {
                                cell.passable = false;

                                GameObject collisionObject = new GameObject(new Vector2(tmxCell.X, tmxCell.Y));
                                collisionObject.passable = false;

                                collisionObjectList = new List<GameObject>();
                                collisionObjectList.Add(collisionObject);
                            }
                        }*/
                    }
                }
            }
        }

        public void AddCollisionObjects(List<GameObject> gameObjectList)
        {
            /*foreach (TmxTilesetTile tile in map.Tilesets[0].Tiles)
            {
                if (tile.Properties["unPassable"].Length != 0)
                {
                    cell.passable = false;

                    GameObject collisionObject = new GameObject(new Vector2(tmxCell.X, tmxCell.Y));
                    collisionObject.passable = false;

                    collisionObjectList = new List<GameObject>();
                    collisionObjectList.Add(collisionObject);
                }
            }
            foreach (GameObject collisionObject in collisionObjectList)
            {
                gameObjectList.Add(collisionObject);
            }
            //collisionObjectList = null;*/
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
            foreach (Layer layer in layers)
            {
                for (int x = screenRect.X / Tile.size; x < (screenRect.X + screenRect.Width) / Tile.size; x++)
                {
                    for (int y = screenRect.Y / Tile.size; y < (screenRect.Y + screenRect.Height) / Tile.size; y++)
                    {
                        int positionX = x * Tile.size - offsetX;
                        int positionY = y * Tile.size - offsetY;
                        //foreach (Layer tileID in Rows[(int)MathHelper.Clamp(y + firstY, 0, height)].Columns[(int)MathHelper.Clamp(x + firstX, 0, width)].layers)
                        MapCell tileID = layer.Rows[(int)MathHelper.Clamp(y + firstY, 0, height-1)].Columns[(int)MathHelper.Clamp(x + firstX, 0, width-1)];
                        if (!tileID.empty)
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
                                Tile.getTileRectangle(tileIDX, tileIDY),
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
    }
}
