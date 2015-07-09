using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using System.IO;

namespace My_first_xna_game
{
    public class TileMap
    {
        private TmxMap tmxMap;
        public List<GameObject> collisionObjectList = new List<GameObject>();
        public List<Layer> layers = new List<Layer>(); //todo: make it an array
        public int width;
        public int height;
        public Rectangle mapRect;
        private Texture2D[] tilesets;
        private List<MapCell> highCells = new List<MapCell>();
        private List<MapCell> mapCellsList = new List<MapCell>();

        public const string configName = "config";
        private TmxMap config;

        private bool debugTileset;

        Timer animationTimer = new Timer(500f, true);

        public TileMap(string path, bool debugTileset = false)
        {
            this.debugTileset = debugTileset;

            tmxMap = new TmxMap(path);
            config = new TmxMap("Maps\\" + configName + ".tmx");

            // 1. intialize
            tilesets = new Texture2D[tmxMap.Tilesets.Count];
            for (int i = 0; i < tmxMap.Tilesets.Count; i++)
            {
                string name = Path.GetFileNameWithoutExtension(tmxMap.Tilesets[i].Image.Source);
                tilesets[i] = (Game.content.Load<Texture2D>("Textures\\Tilesets\\" + name));
                tilesets[i].Name = name;
            }
            this.width = tmxMap.Width; //todo: /32
            this.height = tmxMap.Height;

            for (int counter = 0; counter < tmxMap.Layers.Count; counter++)
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
                        TmxLayerTile tmxCell = tmxMap.Layers[layersCounter].Tiles[currentCell];

                        //texture and tileset
                        if (tmxCell.Gid == 0)
                        {
                            cell.empty = true;
                        }
                        cell.texture = tmxCell.Gid;
                        cell.tileset = tilesets[0];
                        int tilesSoFar = tmxMap.Tilesets[0].Tiles.Count;
                        for (int tilesetsCounter = 0; tilesetsCounter < tmxMap.Tilesets.Count - 1; tilesetsCounter++)
                        {
                            if (tmxCell.Gid > tilesSoFar)
                            {
                                cell.texture = tmxCell.Gid - tilesSoFar;
                                cell.tileset = tilesets[tilesetsCounter + 1];
                            }
                            tilesSoFar += tmxMap.Tilesets[tilesetsCounter + 1].Tiles.Count;
                        }

                        //position
                        cell.position = new Vector2(tmxCell.X * Tile.size, tmxCell.Y * Tile.size);

                        //if autotile

                        if (FindTileset(cell.tileset.Name).Properties["Autotile"] == "true")
                        {
                            cell.autotile = true;
                        }

                        //tile properties
                        TmxTilesetTile tileResult = GetMapTile(cell);

                        if (tileResult != null)
                        {
                            //collision
                            if (tileResult.Properties["Passable"] == "X")
                            {
                                CreateCollisionObject(cell, tileResult, out cell.customCollisionSize);
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

                            //tags
                            string tag = tileResult.Properties["Tag"];
                            if (tag != "")
                            {
                                cell.tags.Add(tag);
                            }
                        }
                    }
                }
            }

            //check for autotiles
            for (int layersCounter = 0; layersCounter < layers.Count; layersCounter++)
            {
                Layer layer = layers[layersCounter];
                for (int y = 1; y < height - 1; y++)
                {
                    MapRow row = layer.Rows[y];
                    for (int x = 1; x < width - 1; x++)
                    {
                        MapCell cell = row.Columns[x];
                        int currentCell = y * (height) + x;
                        TmxLayerTile tmxCell = tmxMap.Layers[layersCounter].Tiles[currentCell];

                        //same layer collision
                        if (layersCounter == 0) //do it for just one time
                        {
                            if (CheckLayersPassableTag(tmxMap, y, x))
                            {
                                List<GameObject> SameLayerCollisionList = collisionObjectList.FindAll(collisionObject => collisionObject.position == cell.position);
                                foreach (GameObject gameObject in SameLayerCollisionList)
                                {
                                    collisionObjectList.Remove(gameObject);
                                }
                            }
                        }

                        //check for autotiles
                        if (cell.autotile)
                        {
                            if (row.Columns[x - 1].autotile && row.Columns[x + 1].autotile)
                            {
                                if (layer.Rows[y - 1].Columns[x].autotile && layer.Rows[y + 1].Columns[x].autotile)
                                {
                                    //corners
                                    //upper left
                                    if (!layer.Rows[y - 1].Columns[x - 1].autotile)
                                    {
                                        cell.autotileCorner = MapCell.Corner.topLeft;
                                    }

                                    //upper right
                                    else if (!layer.Rows[y - 1].Columns[x + 1].autotile)
                                    {
                                        cell.autotileCorner = MapCell.Corner.topRight;
                                    }

                                    //lower left
                                    else if (!layer.Rows[y + 1].Columns[x - 1].autotile)
                                    {
                                        cell.autotileCorner = MapCell.Corner.bottomLeft;
                                    }

                                    //lower right
                                    else if (!layer.Rows[y + 1].Columns[x + 1].autotile)
                                    {
                                        cell.autotileCorner = MapCell.Corner.bottomRight;
                                    }

                                    //center
                                }
                                else
                                {
                                    //down link
                                    if (layer.Rows[y - 1].Columns[x].autotile)
                                    {
                                        cell.texture += cell.tilesetWidth;
                                    }

                                    //up link
                                    if (layer.Rows[y + 1].Columns[x].autotile)
                                    {
                                        cell.texture -= cell.tilesetWidth;
                                    }
                                }
                            }
                            else if (row.Columns[x - 1].autotile)
                            {
                                //right link

                                if (layer.Rows[y - 1].Columns[x].autotile)
                                {
                                    //down link
                                    cell.texture += cell.tilesetWidth;
                                }

                                if (layer.Rows[y + 1].Columns[x].autotile)
                                {
                                    //up link
                                    cell.texture -= cell.tilesetWidth;
                                }
                                cell.texture++;
                            }
                            else if (row.Columns[x + 1].autotile)
                            {
                                //left link

                                if (layer.Rows[y - 1].Columns[x].autotile)
                                {
                                    //down link
                                    cell.texture += cell.tilesetWidth;
                                }

                                if (layer.Rows[y + 1].Columns[x].autotile)
                                {
                                    //up link
                                    cell.texture -= cell.tilesetWidth;
                                }

                                cell.texture--;
                            }
                        }
                    }
                }
            }
        }

        private void CreateCollisionObject(MapCell cell, TmxTilesetTile tileResult, out bool customCollisionSize)
        {
            TmxObjectGroup.TmxObject tmxObject = null;
            for (int objectGroupCount = 0; objectGroupCount < tileResult.ObjectGroups.Count; objectGroupCount++)
            {
                for (int objectCount = 0; objectCount < tileResult.ObjectGroups.Count; objectCount++)
                {
                    tmxObject = tileResult.ObjectGroups[objectGroupCount].Objects[objectCount];
                }
            }

            cell.passable = false;

            GameObject collisionObject = new GameObject(cell.position);//cell.position.X / 32 == 33 && cell.position.Y / 32 == 29

            if (debugTileset)
            {
                collisionObject.AddLight(70, Color.White);
            }

            if (tmxObject != null)
            {
                customCollisionSize = true;
                collisionObject.position += new Vector2((float)tmxObject.X, (float)tmxObject.Y);
                collisionObject.size = new Vector2((float)tmxObject.Width, (float)tmxObject.Height);
            }
            else
            {
                customCollisionSize = false;
            }

            string tag = tileResult.Properties["Tag"];
            if (tag != "")
            {
                collisionObject.tags.Add(tag);
            }

            collisionObjectList.Add(collisionObject);
        }

        private TmxTileset FindTileset(string name)
        {
            foreach(TmxTileset tileset in config.Tilesets)
            {
                if (tileset.Name == name)
                {
                    return tileset;
                }
            }
            return null;
        }

        private bool CheckLayersPassableTag(TmxMap map, int y, int x)
        {
            for (int i = layers.Count -1; i >= 0; i--)
            {
                MapCell cell = layers[i].Rows[y].Columns[x];
                if (!cell.empty)
                {
                    if (cell.customCollisionSize)
                    {
                        return true;
                    }
                    else
                    {
                        return cell.passable && !cell.high;
                    }
                }
            }
            return false;
        }

        private TmxTilesetTile GetMapTile(MapCell cell)
        {
            int texture = cell.texture;
            if (texture == 0) { return null; }

            TmxTileset tmxTileset = FindTileset(cell.tileset.Name);

            for (int tilesCounter = 0; tilesCounter < tmxTileset.Tiles.Count; tilesCounter++)
            {
                if (tilesCounter == texture - 1)
                {
                    return tmxTileset.Tiles[tilesCounter];
                }
            }
            return null;
        }

        private int GetCorner(MapCell cell)
        {
            return cell.texture - cell.tilesetWidth * 2 + 1;
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

        public void Update(GameTime gameTime)
        {
            animationTimer.Update(gameTime);
            if (animationTimer.result)
            {
                for (int layersCounter = 0; layersCounter < layers.Count; layersCounter++)
                {
                    Layer layer = layers[layersCounter];
                    for (int y = 1; y < height - 1; y++)
                    {
                        MapRow row = layer.Rows[y];
                        for (int x = 1; x < width - 1; x++)
                        {
                            MapCell cell = row.Columns[x];
                            if (cell.autotile)
                            {
                                if (cell.autotileAnimationCount == 3)
                                {
                                    cell.texture -= 3 * 3;
                                    cell.autotileAnimationCount = 0;
                                }
                                else
                                {
                                    cell.texture += 3;
                                    cell.autotileAnimationCount++;
                                }
                                animationTimer.Reset();
                            }
                        }
                    }
                }
            }
        }

        public MapCell GetTileByPosition(Vector2 position, int currentLayer)
        {
            Layer layer = layers[currentLayer];
            for (int y = 1; y < height - 1; y++)
            {
                MapRow row = layer.Rows[y];
                for (int x = 1; x < width - 1; x++)
                {
                    MapCell cell = row.Columns[x];
                    int cellPositionX = (int)(cell.position / new Vector2(Tile.size)).X;
                    int cellPositionY = (int)(cell.position / new Vector2(Tile.size)).Y;

                    int positionX = (int)(position / new Vector2(Tile.size)).X;
                    int positionY = (int)(position / new Vector2(Tile.size)).Y;
                    if (cellPositionX == positionX)
                    {
                        if (cellPositionY == positionY)
                        {
                            return cell;
                        }
                    }
                }
            }
            return null;
        }

        public List<MapCell> GetTilesByTag(string tag)
        {
            List<MapCell> result = new List<MapCell>();
            for (int layersCounter = 0; layersCounter < layers.Count; layersCounter++)
            {
                Layer layer = layers[layersCounter];
                for (int y = 1; y < height - 1; y++)
                {
                    MapRow row = layer.Rows[y];
                    for (int x = 1; x < width - 1; x++)
                    {
                        MapCell cell = row.Columns[x];
                        if (cell.tags.Contains(tag))
                        {
                            result.Add(cell);
                        }
                    }
                }
            }

            return result;
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
                            int tileIDY = tileID.texture / tileID.tilesetWidth;
                            if (tileID.texture < 0 || tileID.texture % tileID.tilesetWidth != 0)
                            {
                                tileIDX = tileID.texture % tileID.tilesetWidth;
                            }
                            else
                            {
                                tileIDX = tileID.tilesetWidth;
                            }
                            if (tileID.texture != tileIDY * tileID.tilesetWidth)
                            {
                                tileIDY = tileID.texture / tileID.tilesetWidth;
                            }
                            else
                            {
                                tileIDY = tileID.texture / tileID.tilesetWidth - 1;
                            }
                            spriteBatch.Draw(
                                tileID.tileset,
                                new Rectangle(
                                    positionX, positionY, Tile.size, Tile.size),
                                Tile.getTileRectangle(new Vector2(tileIDX, tileIDY)),
                                Color.White * tileID.getOpacity);
                            
                            if (tileID.autotileCorner != MapCell.Corner.none)
                            {
                                Vector2 cornerPosition = new Vector2(positionX, positionY);
                                Vector2 cropingPosition = Vector2.Zero;
                                switch (tileID.autotileCorner)
                                {
                                    case MapCell.Corner.topRight:
                                        cornerPosition.X += Tile.size / 2;
                                        cropingPosition.X += Tile.size / 2;
                                        break;

                                    case MapCell.Corner.bottomLeft:
                                        cornerPosition.Y += Tile.size / 2;
                                        cropingPosition.Y += Tile.size / 2;
                                        break;

                                    case MapCell.Corner.bottomRight:
                                        cornerPosition.X += Tile.size / 2;
                                        cropingPosition.X += Tile.size / 2;

                                        cornerPosition.Y += Tile.size / 2;
                                        cropingPosition.Y += Tile.size / 2;
                                        break;
                                }
                                int cornerTexture = GetCorner(tileID);
                                int tileIDX2;
                                int tileIDY2 = cornerTexture / tileID.tilesetWidth;
                                if (cornerTexture < 0 || cornerTexture % tileID.tilesetWidth != 0)
                                {
                                    tileIDX2 = cornerTexture % tileID.tilesetWidth;
                                }
                                else
                                {
                                    tileIDX2 = tileID.tilesetWidth;
                                }
                                if (cornerTexture != tileIDY2 * tileID.tilesetWidth)
                                {
                                    tileIDY2 = cornerTexture / tileID.tilesetWidth;
                                }
                                else
                                {
                                    tileIDY2 = cornerTexture / tileID.tilesetWidth - 1;
                                }
                                Rectangle rect = Tile.getTileRectangle(new Vector2(tileIDX2, tileIDY2), true);
                                rect.X += (int)cropingPosition.X;
                                rect.Y += (int)cropingPosition.Y;
                                spriteBatch.Draw(
                                    tileID.tileset,
                                    new Rectangle(
                                        (int)cornerPosition.X, (int)cornerPosition.Y, Tile.size / 2, Tile.size / 2),
                                    rect,
                                    Color.White * tileID.getOpacity);
                            }
                        }
                    }
                }
            }
        }
    }
}