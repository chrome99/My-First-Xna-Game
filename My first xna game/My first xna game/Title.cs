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

        private ParticalManager backgroundParticals;
        private ParticalManager cursorParticals;

        private Song music;
        private bool shutUp = false;

        private int title = 0;
        private int titleSpeed = 1;

        private bool keyDownReleased;
        private bool keyUpReleased;
        private bool keyConfirmReleased;

        private Camera camera1;
        private Camera camera2;
        private Camera camera3;
        private Camera camera4;

        public Title(GraphicsDeviceManager graphicsDeviceManager)
            : base(graphicsDeviceManager)
        {
            background = new Picture(Game.content.Load<Texture2D>("Textures\\Pictures\\title"), Vector2.Zero, null);

            newgame = new Text(Game.content.Load<SpriteFont>("Fonts\\medival big"), new Vector2(80f, 550f), Color.Orange, "New Game", null, new Vector2(20, 20));
            loadgame = new Text(Game.content.Load<SpriteFont>("Fonts\\medival big"), new Vector2(80f, 700f), Color.Orange, "Continue", null, new Vector2(20, 20));
            loadgame.opacity = 50;
            quit = new Text(Game.content.Load<SpriteFont>("Fonts\\medival big"), new Vector2(80f, 850f), Color.Orange, "Quit", null, new Vector2(20, 20));

            backgroundParticals = new ParticalManager(ParticalManager.ParticalsMovement.xy, 100, Game.worldRect, new Vector2(6, 6), 0, 1, Color.Yellow, Color.WhiteSmoke, 100, 0);

            Rectangle newRect = newgame.bounds;
            newRect.Y += newRect.Height - 30;
            newRect.Height = 20;
            cursorParticals = new ParticalManager(ParticalManager.ParticalsMovement.xy, 100, newRect, new Vector2(10, 10), 0, 4, Color.Orange, Color.OrangeRed, 50, 25);

            music = Game.content.Load<Song>("Audio\\Themes\\title theme");

            if (!shutUp)
            {
                MediaPlayer.Play(music);
            }
        }
        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            UpdateInput(newState, oldState);

            backgroundParticals.Update();
            cursorParticals.Update();

            switch (title)
            {
                case 0:
                    if (newgame.position.X < 100)
                    {
                        newgame.position.X += titleSpeed;
                        if (newgame.position.X - 100 < titleSpeed)
                        {
                            Rectangle newRect = newgame.bounds;
                            newRect.Y += newRect.Height - 30;
                            newRect.Height = 20;
                            cursorParticals.NewRect(newRect);

                            newgame.color = Color.Gold;
                            loadgame.color = Color.Orange;
                            quit.color = Color.Orange;
                        }
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
                    if (loadgame.position.X < 100)
                    {
                        loadgame.position.X += titleSpeed;
                        if (loadgame.position.X - 100 < titleSpeed)
                        {
                            Rectangle newRect = loadgame.bounds;
                            newRect.Y += newRect.Height - 30;
                            newRect.Height = 10;
                            cursorParticals.NewRect(newRect);

                            newgame.color = Color.Orange;
                            loadgame.color = Color.Gold;
                            quit.color = Color.Orange;
                        }
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
                    if (quit.position.X < 100)
                    {
                        quit.position.X += titleSpeed;
                        if (quit.position.X - 100 < titleSpeed)
                        {
                            Rectangle newRect = quit.bounds;
                            newRect.Y += newRect.Height - 30;
                            newRect.Height = 10;
                            cursorParticals.NewRect(newRect);

                            newgame.color = Color.Orange;
                            loadgame.color = Color.Orange;
                            quit.color = Color.Gold;
                        }
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
                        

                        camera1 = new Camera(graphicsDeviceManager, new Rectangle(0, 0, 960, 540), PlayerCollection.player1, PlayerCollection.player1);
                        camera2 = new Camera(graphicsDeviceManager, new Rectangle(0, 540, 960, 540), PlayerCollection.player2, PlayerCollection.player2);
                        camera3 = new Camera(graphicsDeviceManager, new Rectangle(960, 0, 960, 540), PlayerCollection.player3, PlayerCollection.player3);
                        camera4 = new Camera(graphicsDeviceManager, new Rectangle(960, 540, 960, 540), PlayerCollection.player4, PlayerCollection.player4);

                        //set scene to map
                        Game.scene = new World(graphicsDeviceManager, new List<Camera> { camera1, camera2, camera3, camera4 });
                        break;

                    case 1:
                        Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();

                        //stop music
                        MediaPlayer.Stop();

                        //Load Players Data
                        Game.InitiateLoad();

                        camera1 = new Camera(graphicsDeviceManager, new Rectangle(0, 0, 960, 540), PlayerCollection.player1, PlayerCollection.player1);
                        camera2 = new Camera(graphicsDeviceManager, new Rectangle(0, 540, 960, 540), PlayerCollection.player2, PlayerCollection.player2);
                        camera3 = new Camera(graphicsDeviceManager, new Rectangle(960, 0, 960, 540), PlayerCollection.player3, PlayerCollection.player3);
                        camera4 = new Camera(graphicsDeviceManager, new Rectangle(960, 540, 960, 540), PlayerCollection.player4, PlayerCollection.player4);

                        //set scene to map
                        Game.scene = new World(graphicsDeviceManager, new List<Camera> { camera1, camera2, camera3, camera4 });
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
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            background.DrawWithoutSource(spriteBatch, new Rectangle());//nana

            backgroundParticals.Draw(spriteBatch);
            cursorParticals.Draw(spriteBatch);

            newgame.DrawWithoutSource(spriteBatch, new Rectangle());//nana
            loadgame.DrawWithoutSource(spriteBatch, new Rectangle());//nana
            quit.DrawWithoutSource(spriteBatch, new Rectangle());//nana



            spriteBatch.End();
        }
    }
}