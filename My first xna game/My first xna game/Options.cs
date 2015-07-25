using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Options
    {
        public bool alive = false;
        private Window window;
        private Selector selector;
        private Player player;
        private SpriteFont font;

        private InputConfig inputConfig;

        private Textbox resolutionTextBoxWidth;
        private Textbox resolutionTextBoxHeight;
        private int resolutionWidthSave;

        public Options(Map map, Player player)
        {
            this.player = player;

            font = Game.content.Load<SpriteFont>("Fonts\\medival1");
            window = new Window(map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), new Vector2(0, 0), 700, 700);
            inputConfig = new InputConfig(map, player);

            int textLayout = 50;

            Text resolutionText = new Text(font, new Vector2(textLayout, 100), Color.Black, "Resolution", window);
            resolutionTextBoxWidth = new Textbox(window, player, new Vector2(500, 100), font.MeasureString("8888"), HandleResolutionWidthInput);
            resolutionTextBoxWidth.Active = false;
            resolutionTextBoxHeight = new Textbox(window, player, new Vector2(700, 100), font.MeasureString("8888"), HandleResolutionHeightInput);
            resolutionTextBoxHeight.Active = false;

            Text configInputText = new Text(font, new Vector2(textLayout, 200), Color.Black, "Config Input", window);

            Vector2 biggestItemSize = new Vector2(window.itemsList[0].bounds.Width, window.itemsList[0].bounds.Height);
            foreach (WindowItem item in window.itemsList)
            {
                if (item.bounds.Width > biggestItemSize.X)
                {
                    biggestItemSize.X = item.bounds.Width;
                }
                if (item.bounds.Height > biggestItemSize.Y)
                {
                    biggestItemSize.Y = item.bounds.Height;
                }
            }

            selector = new Selector(player, new List<WindowItem>() { resolutionText, configInputText }, HandleItemChoice, biggestItemSize, 0, 1, window);
        }

        public void Kill()
        {
            alive = false;
        }

        public void Revive()
        {
            selector.confirmKeyReleased = false;
            selector.currentTargetNum = 0;
            alive = true;
        }

        private void HandleItemChoice()
        {
            switch (selector.currentTargetNum)
            {
                case 0: //resolution
                    selector.active = false;
                    resolutionTextBoxWidth.Active = true;
                    break;

                case 1: //config
                    inputConfig.Revive();
                    window.Kill();
                    break;
            }
        }

        private void HandleResolutionWidthInput(string input)
        {
            if (Int32.TryParse(input, out resolutionWidthSave))
            {
                resolutionTextBoxWidth.Active = false;
                resolutionTextBoxHeight.Active = true;
            }
            else
            {
                resolutionTextBoxWidth.input.ChangeColorEffect(Color.Red, 1000f);
            }
        }

        private void HandleResolutionHeightInput(string input)
        {
            int resolutionHeight;
            if (Int32.TryParse(input, out resolutionHeight))
            {
                Game.ChangeResolution(resolutionWidthSave, resolutionHeight);
                resolutionTextBoxHeight.Active = false;
                selector.active = true;
            }
            else
            {
                resolutionTextBoxHeight.input.ChangeColorEffect(Color.Red, 1000f);
            }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }

            window.Update(gameTime);
            window.UpdateSelectorAndTextBox(newState, oldState, gameTime);
            inputConfig.Update(gameTime, newState, oldState);
            resolutionTextBoxWidth.input.UpdateTextChangeColor();
            resolutionTextBoxHeight.input.UpdateTextChangeColor();
        }

        public bool HandleMenuButtonPress()
        {
            if (inputConfig.alive)
            {
                inputConfig.Kill();
                window.Revive();
            }
            else if (resolutionTextBoxWidth.Active)
            {
                resolutionTextBoxWidth.Active = false;
                selector.active = true;
            }
            else if (resolutionTextBoxHeight.Active)
            {
                resolutionTextBoxWidth.Active = true;
                resolutionTextBoxHeight.Active = false;
            }
            else
            {
                Kill();
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!alive) { return; }

            window.Draw(spriteBatch, new Rectangle());
            inputConfig.Draw(spriteBatch);
        }
    }

    class InputConfig
    {
        public bool alive
        {
            get; private set;
        }
        private Choice choice;
        private Player player;

        private bool getKey = false;
        private FirmKey setToKey = null;

        public InputConfig(Map map, Player player)
        {
            this.player = player;

            SpriteFont font = Game.content.Load<SpriteFont>("Fonts\\medival1");

            Text attackText = new Text(font, Vector2.Zero, Color.Black, "Attack");
            Text jumpText = new Text(font, Vector2.Zero, Color.Black, "Jump");
            Text defendText = new Text(font, Vector2.Zero, Color.Black, "Defend");
            Text useSkillText = new Text(font, Vector2.Zero, Color.Black, "Use Skill");

            Text mvUpText = new Text(font, Vector2.Zero, Color.Black, "Move Up");
            Text mvDownText = new Text(font, Vector2.Zero, Color.Black, "Move Down");
            Text mvRightText = new Text(font, Vector2.Zero, Color.Black, "Move Right");
            Text mvLeftText = new Text(font, Vector2.Zero, Color.Black, "Move Left");

            choice = new Choice(map, new Rectangle(50, 50, 50, 50), player,
                new List<WindowItem>() { attackText, jumpText, defendText, useSkillText, mvUpText, mvDownText, mvRightText, mvLeftText }, HandleButtonChoice, Choice.Arrangement.column);
        }

        public void Kill()
        {
            alive = false;
        }

        public void Revive()
        {
            alive = true;
            choice.ResetSelector();
        }

        private void HandleButtonChoice()
        {
            choice.Active = false;
            choice.SelectorVisible = false;
            getKey = true;
            switch (choice.currentTargetNum)
            {
                case 0: //attack
                    setToKey = player.kbKeys.attack;
                    break;

                case 1: //jump
                    setToKey = player.kbKeys.jump;
                    break;

                case 2: //defend
                    setToKey = player.kbKeys.defend;
                    break;

                case 3: //use skill
                    setToKey = player.kbKeys.useSkill;
                    break;

                case 4: //mv up
                    setToKey = player.kbKeys.mvUp;
                    break;

                case 5: //mv down
                    setToKey = player.kbKeys.mvDown;
                    break;

                case 6: //mv right
                    setToKey = player.kbKeys.mvRight;
                    break;

                case 7: //mv left
                    setToKey = player.kbKeys.mvLeft;
                    break;
            }
        }

        public void Update(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (!alive) { return; }

            choice.Update(newState, oldState, gameTime);

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (getKey)
            {
                Keys[] allPressedKeys = newState.GetPressedKeys();
                if (allPressedKeys.Length > 0)
                {
                    Keys key = allPressedKeys[0];
                    if (!oldState.IsKeyDown(key))
                    {
                        player.kbKeys.list.Find(x => x == setToKey).key = key;
                        getKey = false;
                        choice.Active = true;
                        choice.SelectorVisible = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!alive) { return; }

            choice.Draw(spriteBatch, new Rectangle());
        }
    }
}