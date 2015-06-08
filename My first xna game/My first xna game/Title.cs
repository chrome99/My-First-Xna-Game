using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace My_first_xna_game
{
    class Title : Scene
    {
        private Picture background;
        private Text newgame;
        private Text loadgame;
        private Text quit;

        private ParticalManager particalManager;

        private Song music;
        private bool shutUp = false;

        private int title = 0;
        private int titleSpeed = 3;

        private bool keyDownReleased;
        private bool keyUpReleased;
        private bool keyConfirmReleased;

        public Title(GraphicsDeviceManager graphicsDeviceManager)
            : base(graphicsDeviceManager)
        {
            background = new Picture(Game.content.Load<Texture2D>("Textures\\Pictures\\title"), Vector2.Zero, null);
            background.depth = Game.DepthToFloat(Game.Depth.background);

            newgame = new Text(Game.content.Load<SpriteFont>("Fonts\\medival big"), new Vector2(80f, 550f), Color.Gold, "New Game", null, new Vector2(20, 20));
            loadgame = new Text(Game.content.Load<SpriteFont>("Fonts\\medival big"), new Vector2(80f, 700f), Color.Gold, "Continue", null, new Vector2(20, 20));
            loadgame.opacity = 50;
            quit = new Text(Game.content.Load<SpriteFont>("Fonts\\medival big"), new Vector2(80f, 850f), Color.Gold, "Quit", null, new Vector2(20, 20));

            particalManager = new ParticalManager(100, new Rectangle(0, 0, 1000, 1000));

            music = Game.content.Load<Song>("Audio\\Themes\\title theme");

            if (!shutUp)
            {
                MediaPlayer.Play(music);
            }
        }
        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            UpdateInput(newState, oldState);

            particalManager.Update();

            switch (title)
            {
                case 0:
                    if (newgame.position.X < 150)
                    {
                        newgame.position.X += titleSpeed;
                    }

                    if (loadgame.position.X > 80)
                    {
                        loadgame.position.X -= titleSpeed;
                    }

                    if (quit.position.X > 80)
                    {
                        quit.position.X -= titleSpeed;
                    }
                    break;

                case 1:
                    if (loadgame.position.X < 150)
                    {
                        loadgame.position.X += titleSpeed;
                    }

                    if (newgame.position.X > 80)
                    {
                        newgame.position.X -= titleSpeed;
                    }

                    if (quit.position.X > 80)
                    {
                        quit.position.X -= titleSpeed;
                    }
                    break;

                case 2:
                    if (quit.position.X < 150)
                    {
                        quit.position.X += titleSpeed;
                    }

                    if (newgame.position.X > 80)
                    {
                        newgame.position.X -= titleSpeed;
                    }

                    if (loadgame.position.X > 80)
                    {
                        loadgame.position.X -= titleSpeed;
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

            if (newState.IsKeyDown(Keys.Enter) && keyConfirmReleased)
            {
                switch (title)
                {
                    case 0:
                        Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();

                        //stop music
                        MediaPlayer.Stop();

                        //build map
                        MapCollection.map.AddObject(PlayerCollection.player1);
                        MapCollection.map2.AddObject(PlayerCollection.player2);
                        MapCollection.map.AddObject(PlayerCollection.player3);
                        MapCollection.map.AddObject(PlayerCollection.player4);

                        Camera camera1 = new Camera(graphicsDeviceManager, new Rectangle(0, 0, 960, 540), PlayerCollection.player1, PlayerCollection.player1);
                        Camera camera2 = new Camera(graphicsDeviceManager, new Rectangle(0, 540, 960, 540), PlayerCollection.player2, PlayerCollection.player2);
                        Camera camera3 = new Camera(graphicsDeviceManager, new Rectangle(960, 0, 960, 540), PlayerCollection.player3, PlayerCollection.player3);
                        Camera camera4 = new Camera(graphicsDeviceManager, new Rectangle(960, 540, 960, 540), PlayerCollection.player4, PlayerCollection.player4);

                        //set scene to map
                        Game.scene = new World(graphicsDeviceManager, new List<Camera> { camera1, camera2, camera3, camera4 });
                        break;

                    case 1:
                        Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                        break;

                    case 2:
                        Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();

                        Game.endGame = true;
                        break;
                }
                keyConfirmReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Enter))
            {
                keyConfirmReleased = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            background.Draw(spriteBatch, new Rectangle());
            newgame.Draw(spriteBatch, new Rectangle());
            loadgame.Draw(spriteBatch, new Rectangle());
            quit.Draw(spriteBatch, new Rectangle());

            particalManager.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}