using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
