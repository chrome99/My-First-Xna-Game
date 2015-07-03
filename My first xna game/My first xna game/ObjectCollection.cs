using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    public class ObjectCollection
    {
        public List<GameObject> gameObjectList = new List<GameObject>();
        protected ContentManager Content;
        protected Map map;

        public ObjectCollection(Map map)
        {
            this.map = map;
            Content = Game.content;
        }
    }
}
