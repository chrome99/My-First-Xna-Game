using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class PlayerObject : Sprite
    {
        public Player source;

        public PlayerObject(Player source, Texture2D texture, Vector2 position, Game.Depth depth, float speed = 2f, Rectangle drawingCoordinates = new Rectangle())
            : base(texture, position, depth, speed, drawingCoordinates)
        {
            this.source = source;
        }

        public static PlayerObject SpriteToPlayerObject(Player source, Sprite sprite)
        {
            return new PlayerObject(source, sprite.texture, sprite.position, sprite.depth, sprite.speed, sprite.drawingCoordinates);
        }
    }
}
