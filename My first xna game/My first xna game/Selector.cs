using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


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
        public bool active = true;
        public int itemsInRow;

        // TODO: Remove this
        private bool opacityMaxed = false;
        private bool upKeyReleased = false;
        private bool downKeyReleased = false;
        private bool leftKeyReleased = false;
        private bool rightKeyReleased = false;

        public Selector(Window source, List<WindowItem> targets, Vector2 size, int layout, int itemsInRow = 0)
            : base(source)
        {
            this.targets = targets;
            this.size = size;
            this.layout = layout;
            this.itemsInRow = itemsInRow;

            texture = source.texture;
            opacity = 30f;
            depth = Game.DepthToFloat(Game.Depth.windowsSelector);
        }

        public void Reset()
        {
            currentTargetNum = 0;
            UpdateCurrentTarget();
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

            UpdateCurrentTarget();

            if (!opacityMaxed)
            {
                opacity += 0.5f;
                if (opacity >= 50f)
                {
                    opacityMaxed = true;
                }
            }
            if (opacityMaxed)
            {
                opacity -= 0.5f;
                if (opacity <= 30f)
                {
                    opacityMaxed = false;
                }
            }

            if (active)
            {
                UpdateInput(newState, oldState);
            }
        }

        private void UpdateCurrentTarget()
        {
            currentTarget = targets[currentTargetNum];
            position.X = currentTarget.position.X - layout;
            position.Y = currentTarget.position.Y - layout;
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
            if (newState.IsKeyDown(player.kbKeys.mvRight) && rightKeyReleased)
            {
                if (currentTargetNum < targets.Count - 1)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                    currentTargetNum += 1;
                }

                rightKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.mvRight))
            {
                rightKeyReleased = true;
            }

            //left
            if (newState.IsKeyDown(player.kbKeys.mvLeft) && leftKeyReleased)
            {
                if (currentTargetNum > 0)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                    currentTargetNum -= 1;
                }

                leftKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.mvLeft))
            {
                leftKeyReleased = true;
            }

            //don't update up and down when (newRow == 0).
            if (itemsInRow == 0) { return; }

            //up
            if (newState.IsKeyDown(player.kbKeys.mvUp) && upKeyReleased)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                currentTargetNum = (int)MathHelper.Clamp(currentTargetNum - itemsInRow, 0, targets.Count - 1);

                upKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.mvUp))
            {
                upKeyReleased = true;
            }

            //down
            if (newState.IsKeyDown(player.kbKeys.mvDown) && downKeyReleased)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                currentTargetNum = (int)MathHelper.Clamp(currentTargetNum + itemsInRow, 0, targets.Count - 1);

                downKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.mvDown))
            {
                downKeyReleased = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (visible)
            {
                Vector2 newPositionVec;
                if (source == null)
                {
                    newPositionVec = position;
                }
                else
                {
                    newPositionVec = position + source.position + source.thickness;
                }
                newPositionVec.X = newPositionVec.X - offsetRect.X;
                newPositionVec.Y = newPositionVec.Y - offsetRect.Y;
                Rectangle newPositionRect = bounds;
                newPositionRect.X = (int)newPositionVec.X;
                newPositionRect.Y = (int)newPositionVec.Y;

                spriteBatch.Draw(texture, newPositionRect, selectorRect, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
            }
        }

    }
}
