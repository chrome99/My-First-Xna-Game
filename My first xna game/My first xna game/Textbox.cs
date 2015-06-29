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
            text = new Text(Game.content.Load<SpriteFont>("Fonts\\Debug1"), Vector2.Zero, Color.Black, input, box);
        }

        public void Reset()
        {
            input = "";
        }

        public void UpdateTextbox(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            box.Update(gameTime);
            text.UpdateTextString(input);

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            char newInput;
            if (Game.TryConvertKeyboardInput(newState, oldState, out newInput))
            {
                input = input + newInput;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 windowPosition)
        {
            Rectangle drawingRect = bounds;
            Vector2 drawingPosition = GetDrawingPosition(windowPosition);
            drawingRect.X = (int)drawingPosition.X;
            drawingRect.Y = (int)drawingPosition.Y;
            box.Draw(spriteBatch, drawingRect);
        }

        public override void DrawWithoutSource(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            box.Draw(spriteBatch, offsetRect);
        }
    }
}
