using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace My_first_xna_game
{
    // TODO: A class definition should never have the word "Instance" in it's name
    public class ObjectInstance
    {
        public Map.UpdateCollision updateCollision;
        public List<GameObject> gameObjectList = new List<GameObject>();
        protected ContentManager Content;

        public ObjectInstance()
        {
            Content = Game.content;
        }
    }
}
