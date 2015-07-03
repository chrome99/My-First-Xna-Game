using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class DebugHUD
    {
        public Vector2 position = new Vector2(0, 30);
        private bool keyReleased = false;

        // TODO: Can a Debug HUD be alive or dead?
        private bool alive = false;
        private string text = "";
        private SpriteFont font;
        private Color color;
        private Player player;
        private Keys key;

        public DebugHUD(SpriteFont font, Color color, Player player, Keys key)
        {
            this.font = font;
            this.color = color;
            this.player = player;
            this.key = key;
        }

        public void Update()
        {
            if (!alive) { return; }
            text =
                "Health: " + player.stats.health + " / " + player.stats.maxHealth +
                "\nX:" + player.position.X / Tile.size +
                "\nY: " + player.position.Y / Tile.size +
                "\nMovement: " + player.MovingState +
                "\nDirection: " + player.direction;
        }

        public void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(key) && keyReleased)
            {
                if (alive)
                {
                    alive = false;
                }
                else
                {
                    alive = true;
                }
                keyReleased = false;
            }
            else if (!oldState.IsKeyDown(key))
            {
                keyReleased = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                //draw window
                spriteBatch.DrawString(font, text, position, color
                , 0f, new Vector2(3f, 3f), 1.0f, SpriteEffects.None, Game.DepthToFloat(Game.Depth.front));
            }
        }
    }
}