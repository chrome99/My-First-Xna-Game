using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Vehicle : Actor
    {
        public List<string> passableTilesTags;
        public List<string> impassableTilesTags;

        public Vehicle(Texture2D texture, Vector2 position, List<string> passableTilesTags, List<string> impassableTilesTags)
            : base(texture, position)
        {
            this.passableTilesTags = passableTilesTags;
            this.impassableTilesTags = impassableTilesTags;
        }
    }
}
