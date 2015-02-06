using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    public class MapCell
    {

        public List<Layer> layers = new List<Layer>();
        public Layer tileID
        {
            get
            {
                if (layers.Count > 0)
                {
                    return layers[0];
                }
                else
                {
                    return new Layer();
                }
            }
            set
            {
                if (layers.Count > 0)
                {
                    layers[0] = value;
                }
                else
                {
                    layers.Add(value);
                }

            }
        }

        public MapCell(Layer tileID)
        {
            this.tileID = tileID;
        }
    }

    public class Layer
    {
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
}
