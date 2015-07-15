using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace My_first_xna_game
{
    public class Selector : WindowItem
    {
        public delegate void HandleItemChoice();

        public HandleItemChoice handleItemChoiceFunction;
        public static Rectangle selectorRect = new Rectangle(128, 64, 32, 32);
        public WindowItem currentTarget;
        public int currentTargetNum;
        private Player player;
        private Texture2D texture;
        private List<WindowItem> targets;
        private Vector2 size;
        private int layout;
        public bool active = true;
        public int itemsInRow;

        private bool customSize;

        // TODO: Remove this
        private bool opacityMaxed = false;

        public bool confirmKeyReleased = false;
        private bool upKeyReleased = false;
        private bool downKeyReleased = false;
        private bool leftKeyReleased = false;
        private bool rightKeyReleased = false;

        public Selector(Player player, List<WindowItem> targets, HandleItemChoice handleItemChoiceFunction, Vector2 size, int layout, int itemsInRow = 0, Window source = null)
            : base(source, true)
        {
            this.handleItemChoiceFunction = handleItemChoiceFunction;
            this.size = size;
            this.layout = layout;
            this.itemsInRow = itemsInRow;
            this.targets = targets;
            this.player = player;

            customSize = size == new Vector2();

            if (source == null)
            {
                texture = Game.content.Load<Texture2D>("Textures\\Windows\\windowskin");
            }
            else
            {
                texture = source.texture;
            }
            opacity = 30f;
            WindowDepth = Game.WindowDepth.windowsSelector;
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

        public void UpdateSelector(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
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
            if (customSize)
            {
                size.X = currentTarget.bounds.Width + layout * 2;
                size.Y = currentTarget.bounds.Height + layout * 2;
            }
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

            //confirm
            if (newState.IsKeyDown(player.kbKeys.attack) && confirmKeyReleased)
            {
                handleItemChoiceFunction();
                confirmKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack))
            {
                confirmKeyReleased = true;
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

        public override void Draw(SpriteBatch spriteBatch, Vector2 windowPosition)
        {
            if (visible)
            {
                Vector2 newPositionVec = GetDrawingPosition(windowPosition);
                Rectangle newPositionRect = bounds;
                newPositionRect.X = (int)newPositionVec.X;
                newPositionRect.Y = (int)newPositionVec.Y;

                spriteBatch.Draw(texture, newPositionRect, selectorRect, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
            }
        }

        public override void DrawWithoutSource(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            Vector2 newPositionVec = GetDrawingPositionWithoutSource(offsetRect);
            Rectangle newPositionRect = bounds;
            newPositionRect.X = (int)newPositionVec.X;
            newPositionRect.Y = (int)newPositionVec.Y;
            spriteBatch.Draw(texture, newPositionRect, selectorRect, Color.White * drawingOpacity, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

    }
}
