using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class LightStructure : Light
    {
        List<LightSource> lightSourceList;

        public LightStructure(GameObject source, List<LightSource> lightSourceList)
        {
            this.source = source;
            this.lightSourceList = lightSourceList;

            foreach (LightSource lightSource in lightSourceList)
            {
                lightSource.source = source;
            }
        }

        public override void Update()
        {
            foreach (LightSource lightSource in lightSourceList)
            {
                lightSource.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            foreach (LightSource lightSource in lightSourceList)
            {
                lightSource.Draw(spriteBatch, offsetRect);
            }
        }
    }
}
