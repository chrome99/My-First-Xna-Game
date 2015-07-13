using System.Collections.Generic;

namespace My_first_xna_game
{
    class MapCollection
    {
        public static Map classic;
        public static Map tower;
        public static List<Map> mapsList = new List<Map>();

        static MapCollection()
        {
            TileMap tileMap = new TileMap("Maps\\classic.tmx");
            classic = new Map(tileMap, "classic");
            classic.AddObjectCollection(new ObjectCollection1(classic));
            mapsList.Add(classic);

            TileMap tileMap2 = new TileMap("Maps\\tower.tmx");
            tower = new Map(tileMap2, "tower");
            mapsList.Add(tower);
        }
    }
}
