using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

namespace My_first_xna_game
{
    // TODO: Rename to DebugDisplay, DebugOSD, DebugHUD etc.
    class Debug
    {
        public Vector2 position = new Vector2(0f, 0f);
        private bool keyReleased = false;

        // TODO: Bad name, doesn't say much
        private bool debugSwitch = false;
        private string text;
        private SpriteFont font;
        private Color color;
        private Player player;
        private Keys key;

        public Debug(SpriteFont font, Color color, Player player, Keys key)
        {
            this.font = font;
            this.color = color;
            this.player = player;
            this.key = key;
        }

        public void Update()
        {
            text =
                "\nX:" + player.position.X / Tile.size +
                "\nY: " + player.position.Y / Tile.size +
                "\nMovement: " + player.movingState +
                //"\n blabla: " + Shop.blabla +
                "\nDirection: " + player.direction;
        }

        public void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(key) && keyReleased)
            {
                if (debugSwitch)
                {
                    debugSwitch = false;
                }
                else
                {
                    debugSwitch = true;
                }
                keyReleased = false;
            }
            else if (!oldState.IsKeyDown(key))
            {
                keyReleased = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle screenPosition)
        {
            if (debugSwitch)
            {
                //draw window
                Vector2 drawingPosition = position;
                drawingPosition.X = screenPosition.X + drawingPosition.X;
                drawingPosition.Y = screenPosition.Y + drawingPosition.Y;

                spriteBatch.DrawString(font, text, drawingPosition, color
                , 0f, new Vector2(3f, 3f), 1.0f, SpriteEffects.None, Game.DepthToFloat(Game.Depth.front));
            }
        }
    }
}
