using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    public class Timer
    {
        public float max = 1f;
        public float counter = 0f;
        public bool result = false;
        private bool timerSwitch = false;

        public bool Counting
        {
            get { return timerSwitch && counter < max; }
        }

        public Timer(float max, bool timerSwitch = true)
        {
            this.max = max;
            this.timerSwitch = timerSwitch;

            Game.timersList.Add(this);
        }

        public void Active()
        {
            timerSwitch = true;
        }

        public void Reset(bool startCounting = true)
        {
            counter = 0f;
            timerSwitch = startCounting;
        }

        public void Update(GameTime gameTime)
        {
            if (timerSwitch && counter < max)
            {
                counter += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
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
