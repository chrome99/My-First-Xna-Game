using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class MapCell
    {
        public bool empty = false;
        public Vector2 position;
        public int texture = 0;
        public bool high = false;
        public List<string> tag = new List<string>();
        public bool passable = true;
        public float opacity = 100f;
        public Texture2D tileset;
        public int tilesetID;
        public bool autotile = false;
        public int autotileAnimationCount = 0;
        public Corner autotileCorner = Corner.none;
        public enum Corner { none, topLeft, topRight, bottomLeft, bottomRight }

        public float getOpacity
        {
            get { return opacity / 100; }
        }

        public int tilesetWidth
        {
            get { return tileset.Width / Tile.size; }
        }
    }

    public class Layer
    {
        public List<MapRow> Rows = new List<MapRow>();
    }

    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }
}
