using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    class World : Scene
    {
        public List<Camera> cameraList;

        public World(Camera camera)
        {
            cameraList.Add(camera);
        }

        public World(List<Camera> cameraList)
        {
            this.cameraList = cameraList;
        }

        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            foreach (Camera camera in cameraList)
            {
                camera.Update(newState, oldState, gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Camera camera in cameraList)
            {
                camera.Draw(spriteBatch);
            }
        }
    }
}
