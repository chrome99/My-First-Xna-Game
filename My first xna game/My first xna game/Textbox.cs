using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Textbox : WindowItem
    {
        public delegate void HandleText(string input);
        private HandleText handleTextFunction;

        private Window box;
        private SpriteFont font;

        public Text input;
        private string inputString = "";
        public string InputString
        {
            get
            {
                return inputString;
            }
            set
            {
                inputString = value;
                input.UpdateTextString(InputString);
            }
        }

        private Text cursor;
        private int cursorIndex = 0;
        private Timer cursorAnimationTimer = new Timer(400f, true);


        private bool enterKeyReleased = false;
        private bool backKeyReleased = false;
        private bool rightKeyReleased = false;
        private bool leftKeyReleased = false;

        public Textbox(Window source, Player player, Vector2 position, Vector2 size, HandleText handleTextFunction)
            : base(source)
        {
            this.handleTextFunction = handleTextFunction;

            box = new Window(player.map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), position, (int)size.X, (int)size.Y);
            font = Game.content.Load<SpriteFont>("Fonts\\Debug1");
            input = new Text(font, Vector2.Zero, Color.Black, InputString, box);
            cursor = new Text(font, Vector2.Zero, Color.Black, "_", box);
        }

        public void Reset()
        {
            InputString = "";
            ResetCursorPosition();
        }

        public void UpdateTextbox(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            box.Update(gameTime);
            UpdateCursorAnimation();

            UpdateInput(newState, oldState);
        }

        public void ResetCursorPosition()
        {
            cursor.position.X = input.bounds.Width + font.Spacing;
            cursorIndex = InputString.Length;
        }

        public void SetCursorPositionToStart()
        {
            cursor.position.X = 0;
            cursorIndex = 0;
        }

        private void CursorGoTo(int index)
        {
            string checkingString = InputString.Remove(index);
            cursor.position.X = font.MeasureString(checkingString).X;
            cursorIndex = checkingString.Length;
        }

        private void FixCursorPosition(string str, bool back = false)
        {
            float newPosition = font.MeasureString(str).X + font.Spacing;
            if (back)
            {
                cursor.position.X -= newPosition;
                cursorIndex -= str.Length;
            }
            else
            {
                cursor.position.X += newPosition;
                cursorIndex += str.Length;
            }

        }

        private void FixCursorPosition(char letter, bool back = false)
        {
            float newPosition = font.MeasureString(letter.ToString()).X + font.Spacing;
            if (back)
            {
                cursor.position.X -= newPosition;
                cursorIndex--;
            }
            else
            {
                cursor.position.X += newPosition;
                cursorIndex++;
            }
        }

        private void UpdateCursorAnimation()
        {
            if (cursorAnimationTimer.result)
            {
                if (cursor.text == "_")
                {
                    cursor.text = "";
                }
                else if (cursor.text == "")
                {
                    cursor.text = "_";
                }
                cursorAnimationTimer.Reset();
            }
        }

        private void FixCursorAnimation()
        {
            cursor.text = "_";
            cursorAnimationTimer.Reset();
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            char newInput;
            if (Game.TryConvertKeyboardInput(newState, oldState, out newInput))
            {
                InputString = InputString.Insert(cursorIndex, newInput.ToString());
                FixCursorPosition(newInput, false);
                FixCursorAnimation();
            }

            //command line
            if (newState.IsKeyDown(Keys.Enter) && enterKeyReleased)
            {
                handleTextFunction(InputString);

                enterKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Enter))
            {
                enterKeyReleased = true;
            }

            if (newState.IsKeyDown(Keys.Back) && backKeyReleased)
            {
                if (InputString.Length != 0 && cursorIndex > 0)
                {
                    char charToRemove = InputString[cursorIndex - 1];
                    InputString = InputString.Remove(cursorIndex - 1, 1);
                    if (InputString.Length != 0)
                    {
                        FixCursorPosition(charToRemove, true);
                    }
                    else
                    {
                        ResetCursorPosition();
                    }

                    FixCursorAnimation();
                }
                
                backKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Back))
            {
                backKeyReleased = true;
            }

            if (newState.IsKeyDown(Keys.Right) && rightKeyReleased)
            {
                if (newState.IsKeyDown(Keys.LeftControl))
                {
                    if (cursorIndex != InputString.Length)
                    {
                        int newIndex = InputString.IndexOf(" ", cursorIndex + 1);
                        if (newIndex == -1)
                        {
                            ResetCursorPosition();
                        }
                        else
                        {
                            CursorGoTo(newIndex);
                        }
                    }
                }
                else
                {
                    if (cursorIndex < InputString.Length)
                    {
                        FixCursorPosition(InputString[cursorIndex]);
                    }
                }
                rightKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Right))
            {
                rightKeyReleased = true;
            }

            if (newState.IsKeyDown(Keys.Left) && leftKeyReleased)
            {
                if (newState.IsKeyDown(Keys.LeftControl))
                {
                    if (cursorIndex != 0)
                    {
                        int newIndex = InputString.LastIndexOf(" ", cursorIndex - 1);
                        if (newIndex == -1)
                        {
                            SetCursorPositionToStart();
                        }
                        else
                        {
                            CursorGoTo(newIndex);
                        }
                    }
                }
                else
                {
                    if (cursorIndex > 0)
                    {
                        FixCursorPosition(InputString[cursorIndex - 1], true);
                    }
                }
                leftKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Left))
            {
                leftKeyReleased = true;
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
