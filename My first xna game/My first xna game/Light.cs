using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Light
    {
        protected static Texture2D texture = Game.content.Load<Texture2D>("Textures\\Sprites\\lightmask");
        public GameObject source;

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {

        }
    }
}
