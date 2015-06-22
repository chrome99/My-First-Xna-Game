using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class MiniMap
    {
        Map map;
        public MiniMap(Map map)
        {
            this.map = map;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //map.Draw(spriteBatch, camera, true);
        }

    }
}
