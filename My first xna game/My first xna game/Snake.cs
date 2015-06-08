using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    class Snake
    {
        private Partical[] particalsArray;

        public List<Vector2> destinationsList = new List<Vector2>();
        public List<bool> newDestinationsList = new List<bool>();

        public Snake(int length, Rectangle startingRect)
        {
            particalsArray = new Partical[length];
            for (int i = 0; i < length; i++)
            {
                particalsArray[i] = new Partical(startingRect, 1, Color.Coral);
            }
        }

        public void Update()
        {
            if (destinationsList.Count == 0) { return; }

            for (int i = 0; i < destinationsList.Count; i++)
            {
                newDestinationsList.Add(false);
            }

            for (int i = 0; i < destinationsList.Count; i++)
            {
                if (!newDestinationsList[i])
                {
                    int doneParticalsCount = 0;
                    for (int counter = 0; counter < particalsArray.Length; counter++)
                    {
                        if (!particalsArray[counter].GoTo(new Vector2(500, 500)))
                        {
                            doneParticalsCount++;
                        }
                    }
                    if (doneParticalsCount == particalsArray.Length)
                    {
                        newDestinationsList[i] = true;
                        return;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Partical partical in particalsArray)
            {
                partical.Draw(spriteBatch);
            }
        }
    }
}
