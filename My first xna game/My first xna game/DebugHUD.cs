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
        private Text text;
        private Player player;

        public DebugHUD(Text text, Player player)
        {
            this.text = text;
            this.player = player;

            text.WindowDepth = Game.WindowDepth.GUIFront;
        }

        public void Update()
        {
            if (!alive) { return; }
            text.UpdateTextString(
                "Health: " + player.stats.strength + " / " + player.stats.maxHealth +
                "\nX:" + player.position.X / Tile.size +
                "\nY: " + player.position.Y / Tile.size +
                "\nMovement: " + player.MovingState +
                "\nDirection: " + player.direction
                );
        }

        public void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(player.kbKeys.opDebug) && keyReleased)
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
            else if (!oldState.IsKeyDown(player.kbKeys.opDebug))
            {
                keyReleased = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                //draw text
                text.DrawWithoutSource(spriteBatch, new Rectangle());
            }
        }
    }
}