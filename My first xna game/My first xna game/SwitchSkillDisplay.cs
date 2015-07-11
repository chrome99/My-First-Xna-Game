using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class SwitchSkillDisplay
    {
        public bool alive = false;
        public bool active = false;
        Player player;
        List<Picture> picturesList;

        public SwitchSkillDisplay(Map map, Player player)
        {
            this.player = player;
            picturesList = new List<Picture>();

            for (int i = 0; i < 5; i++)
            {
                picturesList.Add(new Picture(Item.IconSet, Vector2.Zero, null));
            }

            Kill();
        }

        public void Revive()
        {
            if (CreateSkillPictures())
            {
                alive = true;
            }
        }

        public void Dispose()
        {
            foreach (Picture picture in picturesList)
            {
                picture.Fade(0);
            }
        }

        public void Kill()
        {
            alive = false;
        }

        private bool CreateSkillPictures()
        {
            if (player.currentSkill == null) { return false; }

            int currentSkill = player.skillsList.IndexOf(player.currentSkill);
            int picturesCounter = 0;
            for (int skillCounter = currentSkill - 2; skillCounter < currentSkill + 2 + 1; skillCounter++)
            {
                Picture picture = picturesList[picturesCounter];
                picture.position = new Vector2(player.position.X + ((picturesCounter - 2) * 50), player.position.Y - 50);
                picture.drawingRect = player.skillsList[skillCounter].getRect;
                picture.opacity = 1;
                picture.FadeBack(100);

                picturesCounter++;
            }
            return true;
        }

        public void Update(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (!alive) { return; }

            if (picturesList[0].opacity == 0)
            {
                Kill();
            }

            for (int i = 0; i < picturesList.Count; i++)
            {
                picturesList[i].position = new Vector2(player.position.X + ((i - 2) * 50), player.position.Y - 50);
                picturesList[i].UpdateFade();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }

            foreach (Picture picture in picturesList)
            {
                picture.DrawWithoutSource(spriteBatch, offsetRect);
            }
        }
    }
}
