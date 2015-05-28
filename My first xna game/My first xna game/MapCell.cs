using System.Collections.Generic;

namespace My_first_xna_game
{
    public class MapCell
    {
        public bool empty = false;
        public int texture = 0;
        public float depth = Game.DepthToFloat(Game.Depth.background);
        public List<string> tag = new List<string>();
        public bool passable = true;
        public float opacity = 100f;

        public float getOpacity
        {
            get { return opacity / 100; }
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
