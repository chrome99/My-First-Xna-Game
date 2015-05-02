using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace My_first_xna_game
{
    public class Selector : WindowItem
    {
        public static Rectangle selectorRect = new Rectangle(128, 64, 32, 32);
        public WindowItem currentTarget;
        public int currentTargetNum;
        public Player player;
        private Texture2D texture;
        private List<WindowItem> targets;
        private Vector2 size;
        private int layout;

        // TODO: Bad name. Every Selector has a newRow? Really? Where's the oldRow? :P
        private int newRow;

        // TODO: typo
        private bool subOpactiy = false;
        private bool upKeyReleased = false;
        private bool downKeyReleased = false;
        private bool leftKeyReleased = false;
        private bool rightKeyReleased = false;

        public Selector(Window source, List<WindowItem> targets, Vector2 size, int layout, int newRow = 0)
            : base(source)
        {
            this.targets = targets;
            this.size = size;
            this.layout = layout;
            this.newRow = newRow;

            texture = source.texture;
            opacity = 30f;
            depth = Game.DepthToFloat(Game.Depth.windowsSelector);
        }

        public override Rectangle bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {

            if (targets.Count == 0)
            {
                visible = false;
            }
            if (!visible) { return; }

            currentTarget = targets[currentTargetNum];
            position.X = currentTarget.position.X - layout;
            position.Y = currentTarget.position.Y - layout;

            if (!subOpactiy)
            {
                opacity += 0.5f;
                if (opacity >= 50f)
                {
                    subOpactiy = true;
                }
            }
            if (subOpactiy)
            {
                opacity -= 0.5f;
                if (opacity <= 30f)
                {
                    subOpactiy = false;
                }
            }


            UpdateInput(newState, oldState);
        }

        public bool Clamp()
        {
            int oldTargetNum = currentTargetNum;
            currentTargetNum = (int)MathHelper.Clamp(currentTargetNum, 0, targets.Count - 1);
            if (oldTargetNum == currentTargetNum)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (player == null)
            {
                //use default keys
            }
            //right
            if (newState.IsKeyDown(player.keys.right) && rightKeyReleased)
            {
                if (currentTargetNum < targets.Count - 1)
                {
                    currentTargetNum += 1;
                }

                rightKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.keys.right))
            {
                rightKeyReleased = true;
            }

            //left
            if (newState.IsKeyDown(player.keys.left) && leftKeyReleased)
            {
                if (currentTargetNum > 0)
                {
                    currentTargetNum -= 1;
                }

                leftKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.keys.left))
            {
                leftKeyReleased = true;
            }

            //don't update up and down when (newRow == 0).
            if (newRow == 0) { return; }

            //up
            if (newState.IsKeyDown(player.keys.up) && upKeyReleased)
            {
                currentTargetNum = (int)MathHelper.Clamp(currentTargetNum - newRow, 0, targets.Count - 1);

                upKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.keys.up))
            {
                upKeyReleased = true;
            }

            //down
            if (newState.IsKeyDown(player.keys.down) && downKeyReleased)
            {
                currentTargetNum = (int)MathHelper.Clamp(currentTargetNum + newRow, 0, targets.Count - 1);

                downKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.keys.down))
            {
                downKeyReleased = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenRect)
        {
            if (visible)
            {
                Rectangle newPosition = bounds;
                spriteBatch.Draw(texture, newPosition, selectorRect, Color.White * getOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
            }
        }

    }
}
