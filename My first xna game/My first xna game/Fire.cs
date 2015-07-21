using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Fire
    {
        private ParticalManager flame;
        private Timer timer;
        private Rectangle hole;
        private bool toggleHole = false;

        public Fire(Rectangle rect)
        {
            flame = new ParticalManager(ParticalManager.ParticalsMovement.xy, (rect.Width * rect.Height) / 75, rect, new Vector2(1, 1), 20, 10, Color.Red, Color.OrangeRed, 50, 25);
            hole = new Rectangle(0, 0, 350, 350);
            timer = new Timer(3000f);
        }

        public void Update()
        {
            flame.Update();

            if (timer.result)
            {
                if (toggleHole)
                {
                    flame.RemoveHole(hole);
                    toggleHole = false;
                }
                else
                {
                    flame.AddHole(hole);
                    toggleHole = true;
                }
                timer.Reset();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            flame.Draw(spriteBatch);
        }
    }
}
