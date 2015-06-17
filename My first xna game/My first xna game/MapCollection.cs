using System.Collections.Generic;

namespace My_first_xna_game
{
    class MapCollection
    {
        public static Map map;
        public static Map map2;
        public static List<Map> mapsList = new List<Map>();

        static MapCollection()
        {
            TileMap tileMap = new TileMap("Maps\\classic.tmx", false);
            map = new Map(tileMap, "classic");
            map.AddObjectCollection(new ObjectCollection1(map));

            TileMap tileMap2 = new TileMap("Maps\\tower.tmx", false);
            map2 = new Map(tileMap2, "tower");

            mapsList.Add(map);
            mapsList.Add(map2);
        }
    }
}
