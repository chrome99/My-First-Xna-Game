using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    class Title : Scene
    {
        public static Map map;
        private Picture background;
        private Text newgame;
        private Text loadgame;
        private Text quit;
        private int title = 0;
        private bool keyDownReleased;
        private bool keyUpReleased;

        public Title(GraphicsDeviceManager graphicsDeviceManager)
            : base(graphicsDeviceManager)
        {
            background = new Picture(Game.content.Load<Texture2D>("Textures\\Pictures\\title"), Vector2.Zero, null);
            background.depth = Game.DepthToFloat(Game.Depth.background);
            newgame = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), new Vector2(50f, 300f), Color.BurlyWood, "New Game");
            loadgame = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), new Vector2(50f, 350f), Color.BurlyWood, "Continue");
            loadgame.opacity = 50;
            quit = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), new Vector2(50f, 400f), Color.BurlyWood, "Quit");
        }
        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            UpdateInput(newState, oldState);
            switch (title)
            {
                case 0:
                    if (newgame.position.X < 80)
                    {
                        newgame.position.X += 2;
                    }

                    if (loadgame.position.X > 50)
                    {
                        loadgame.position.X -= 2;
                    }

                    if (quit.position.X > 50)
                    {
                        quit.position.X -= 2;
                    }
                    break;

                case 1:
                    if (loadgame.position.X < 80)
                    {
                        loadgame.position.X += 2;
                    }

                    if (newgame.position.X > 50)
                    {
                        newgame.position.X -= 2;
                    }

                    if (quit.position.X > 50)
                    {
                        quit.position.X -= 2;
                    }
                    break;

                case 2:
                    if (quit.position.X < 80)
                    {
                        quit.position.X += 2;
                    }

                    if (newgame.position.X > 50)
                    {
                        newgame.position.X -= 2;
                    }

                    if (loadgame.position.X > 50)
                    {
                        loadgame.position.X -= 2;
                    }
                    break;
            }
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(Keys.S) && keyDownReleased)
            {
                if (title < 2)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                    title++;
                }
                keyDownReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.S))
            {
                keyDownReleased = true;
            }

            if (newState.IsKeyDown(Keys.W) && keyUpReleased)
            {
                if (title > 0)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\select").Play();
                    title--;
                }

                keyUpReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.W))
            {
                keyUpReleased = true;
            }

            if (newState.IsKeyDown(Keys.Enter))
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                switch (title)
                {
                    case 0:
                        //build map
                        TileMap tileMap = new TileMap("Maps\\classic.tmx");
                        map = new Map(tileMap);
                        map.AddObjectCollection(new ObjectCollection1(map));
                        Camera camera1 = new Camera(new Rectangle(0, 0, 960, 540), map, map.player1, map.player1);
                        Camera camera2 = new Camera(new Rectangle(0, 540, 960, 540), map, map.player2, map.player2);
                        Camera camera3 = new Camera(new Rectangle(960, 0, 960, 540), map, map.player3, map.player3);
                        Camera camera4 = new Camera(new Rectangle(960, 540, 960, 540), map, map.player4, map.player4);

                        //set scene to map
                        Game.scene = new World(graphicsDeviceManager, new List<Camera> { camera1 , camera2, camera3, camera4 });
                        break;

                    case 1:

                        break;

                    case 2:
                        Game.endGame = true;
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            background.Draw(spriteBatch, new Rectangle());
            newgame.Draw(spriteBatch, new Rectangle());
            loadgame.Draw(spriteBatch, new Rectangle());
            quit.Draw(spriteBatch, new Rectangle());

            spriteBatch.End();
        }
    }
}
