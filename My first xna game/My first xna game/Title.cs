using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Title : Scene
    {
        //scene title
        public static Map map;
        Picture background;
        Text newgame;
        Text loadgame;
        Text quit;
        int title = 0;
        bool keyDownReleased;
        bool keyUpReleased;

        public Title(ContentManager Content)
        {
            background = new Picture(Content.Load<Texture2D>("title"), Vector2.Zero, null);
            background.depth = Game.DepthToFloat(Game.Depth.background);
            newgame = new Text(Content.Load<SpriteFont>("medival1"), new Vector2(50f, 300f), Color.BurlyWood, "New Game");
            loadgame = new Text(Content.Load<SpriteFont>("medival1"), new Vector2(50f, 350f), Color.BurlyWood, "Continue");
            loadgame.opacity = 50;
            quit = new Text(Content.Load<SpriteFont>("medival1"), new Vector2(50f, 400f), Color.BurlyWood, "Quit");
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
                switch (title)
                {
                    case 0:
                        //build map
                        TileMap tileMap = new TileMap(Game.content.Load<Texture2D>("Grassland"), TileMap.MapDesign.trees, 50, 50);
                        map = new Map(tileMap, 1);
                        map.AddObjectInstance(new ObjectInstance1(map));
                        Camera camera1 = new Camera(new Rectangle(0, 0, 320, 480), map, 1);
                        Camera camera2 = new Camera(new Rectangle(320, 0, 320, 480), map, 0);

                        //set scene to map
                        Game.scene = new World(new List<Camera> { camera1, camera2 });
                        break;

                    case 1:

                        break;

                    case 2:
                        //game.Quit();
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            newgame.Draw(spriteBatch);
            loadgame.Draw(spriteBatch);
            quit.Draw(spriteBatch);
        }
    }
}
