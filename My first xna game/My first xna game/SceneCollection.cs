using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace My_first_xna_game
{
    class SceneCollection
    {
        public static World world;
        public static Title title;

        static SceneCollection()
        {
            Camera camera1 = new Camera(Game.graphics, new Rectangle(0, 0, 960, 540), PlayerCollection.player1, PlayerCollection.player1);
            Camera camera2 = new Camera(Game.graphics, new Rectangle(0, 540, 960, 540), PlayerCollection.player2, PlayerCollection.player2);
            Camera camera3 = new Camera(Game.graphics, new Rectangle(960, 0, 960, 540), PlayerCollection.player3, PlayerCollection.player3);
            Camera camera4 = new Camera(Game.graphics, new Rectangle(960, 540, 960, 540), PlayerCollection.player4, PlayerCollection.player4);

            title = new Title(Game.graphics);

            world = new World(Game.graphics, new List<Camera> { camera1, camera2, camera3, camera4 });
        }
    }
}
