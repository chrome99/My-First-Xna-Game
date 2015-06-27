﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class ChooseSkill
    {
        public Window window;
        private Selector selector;
        public bool alive = false;

        private Player player;
        private SkillBranch currentBranch;

        private bool confirmKeyReleased = false;

        public ChooseSkill(Player player)
        {
            this.player = player;

            currentBranch = player.skillTree.skillBranches[player.stats.level - 1];
            window = new Window(player.map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), new Vector2(20), 920, 500);
            Text skillText1 = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, currentBranch.skill1.color, currentBranch.skill1.name, null, new Vector2(2, 5));
            Text skillText2 = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, currentBranch.skill2.color, currentBranch.skill2.name, null, new Vector2(2, 5));
            Picture skillPicture1 = new Picture(currentBranch.skill1.choosePicture, Vector2.Zero, null);
            Picture skillPicture2 = new Picture(currentBranch.skill2.choosePicture, Vector2.Zero, null);

            skillPicture1.position.X += 100;
            skillPicture1.position.Y += 60;

            skillPicture2.position.X = window.bounds.Width - skillPicture2.bounds.Width - 100;
            skillPicture2.position.Y += 60;



            skillText1.position.X = skillPicture1.position.X + skillPicture1.bounds.Width / 2 - skillText1.bounds.Width / 2;
            skillText1.position.Y = skillPicture1.position.Y + skillPicture1.bounds.Height;

            skillText2.position.X = skillPicture2.position.X + skillPicture2.bounds.Width / 2 - skillText2.bounds.Width / 2;
            skillText2.position.Y = skillPicture2.position.Y + skillPicture2.bounds.Height;

            window.AddItem(skillPicture1);
            window.AddItem(skillPicture2);
            window.AddItem(skillText1);
            window.AddItem(skillText2);

            selector = new Selector(window, new List<WindowItem>() { skillText1, skillText2 }, new Vector2(), 10, 2);
            selector.player = player;
        }

        public void Update(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (!alive) { return; }
            window.Update(gameTime);
            selector.Update(newState, oldState, gameTime);

            UpdateInput(newState, oldState);
        }

        public void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(player.kbKeys.attack) && confirmKeyReleased)
            {
                if (selector.currentTargetNum == 1)
                {
                    player.LearnSkill(currentBranch.skill2);
                    player.AddStats(currentBranch.changeStats2);
                    alive = false;
                }
                else
                {
                    player.LearnSkill(currentBranch.skill1);
                    player.AddStats(currentBranch.changeStats1);
                    alive = false;
                }
                confirmKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack))
            {
                confirmKeyReleased = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!alive) { return; }
            window.Draw(spriteBatch, new Rectangle());
            selector.Draw(spriteBatch, new Rectangle());
        }
    }
}
