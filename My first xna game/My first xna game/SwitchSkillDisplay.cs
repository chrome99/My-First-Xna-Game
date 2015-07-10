using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class SwitchSkillDisplay
    {
        public bool alive = false;
        Player player;
        Picture skillIconCenter;
        Picture skillIconLeft;
        Picture skillIconVeryLeft;
        Picture skillIconRight;
        Picture skillIconVeryRight;
        Picture skillIconTransition;
        List<Picture> picturesList;

        public SwitchSkillDisplay(Map map, Player player)
        {
            this.player = player;
            picturesList = new List<Picture>() { skillIconVeryLeft, skillIconLeft, skillIconCenter, skillIconRight, skillIconVeryRight };
            Kill();
        }

        public void Revive()
        {
            if (CreateSkillPictures())
            {
                alive = true;
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
                picturesList[picturesCounter] = new Picture(Item.IconSet, new Vector2(player.position.X + ((picturesCounter - 2) * 50), player.position.Y - 50), null);
                picturesList[picturesCounter].drawingRect = player.skillsList[skillCounter].getRect;
                picturesCounter++;
            }
            return true;
        }

        public void Update(GameTime gameTime, KeyboardState newState, KeyboardState oldState)
        {
            if (!alive) { return; }


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
