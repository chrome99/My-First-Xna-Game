using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    public class Timer
    {
        public float max = 1f;
        public float counter = 0f;
        public bool result = false;
        public bool timerSwitch = false;
        private float i;

        public Timer(float max, bool timerSwitch = true)
        {
            this.max = max;
            this.timerSwitch = timerSwitch;

            Game.timersList.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            if (timerSwitch && counter < max)
            {
                counter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                i = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (counter >= max)
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
    }
}
