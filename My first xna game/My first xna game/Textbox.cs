using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Textbox : WindowItem
    {
        private Window box;
        public Text text;

        public string input;

        public Textbox(Window source, Player player, Vector2 position, Vector2 size)
            : base(source)
        {
            input = "";

            box = new Window(player.map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), position, (int)size.X, (int)size.Y);
            text = new Text(Game.content.Load<SpriteFont>("Fonts\\Debug1"), position, Color.Black, input, box);
        }

        public void UpdateTextbox(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            box.Update(gameTime);
            text.UpdateTextString(input);

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            box.Draw(spriteBatch, offsetRect);
        }
    }
}
