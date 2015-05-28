using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class HostileHUD
    {
        private Hostile hostile;
        private List<Heart> heartsList = new List<Heart>();

        private float getHeartsNum(bool maxHealth, bool quarters)
        {
            int stat;
            if (maxHealth)
            {
                stat = hostile.stats.maxHealth;
            }
            else
            {
                stat = hostile.stats.health;
            }

            float result = stat / 10 + 1;
            if (quarters)
            {
                result *= 4;
            }
            return result;
        }

        public HostileHUD(Hostile hostile)
        {
            //intialize player
            this.hostile = hostile;

            //intialize hearts list
            for (int counter = 0; counter < getHeartsNum(true, false); counter++)
            {
                Picture emptyHeartPic = new Picture(Game.content.Load<Texture2D>("Textures\\Sprites\\heart 0"), new Vector2(5, 5), null);
                Picture fullHeartPic = new Picture(Game.content.Load<Texture2D>("Textures\\Sprites\\heart 4"), new Vector2(5, 5), null);
                emptyHeartPic.position.X += Tile.size * counter;
                fullHeartPic.position.X += Tile.size * counter;
                fullHeartPic.depth = Game.DepthToFloat(Game.Depth.windowsDataFront);
                heartsList.Add(new Heart { fullHeart = fullHeartPic, emptyHeart = emptyHeartPic, value = 4 });
            }
        }

        public void UpdateHearts(int damage)
        {
            if (damage == 0) { return; } //prevent divide by 0 exepation
            float amountOfQuartersToRemove = hostile.stats.maxHealth / heartsList.Count / 4 / damage;
            //reverse loop
            for (int counter = 0; counter < amountOfQuartersToRemove; counter++)
            {
                for (int i = heartsList.Count; i-- > 0; )
                {
                    if (heartsList[i].value != 0)
                    {
                        heartsList[i].value--;
                        if (heartsList[i].value == 0)
                        {
                            if (i == 0) //last one and equal zero
                            {
                                heartsList[i].value++;
                                break;
                            }
                            else
                            {
                                heartsList[i].fullHeart.visible = false;
                            }
                        }
                        else
                        {
                            heartsList[i].fullHeart.texture = Game.content.Load<Texture2D>("Textures\\Sprites\\heart " + heartsList[i].value);
                        }
                        break;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Heart heart in heartsList)
            {
                heart.fullHeart.Draw(spriteBatch, new Rectangle());
                heart.emptyHeart.Draw(spriteBatch, new Rectangle());
            }
        }
    }

    class Heart
    {
        public Picture fullHeart;
        public Picture emptyHeart;
        public int value;
    }
}
