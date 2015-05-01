using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        /*
         TODO:
         * npc's
         * trade
         * pvp
         * pickup items
         * skills
         * equipment
         * levels (in level up you can upgrade skills or stats)
         * map editor
         * round raduis, and test what happens with two targets at the same time
         * better AI for enemies
         * real HUD
         * little fighter world(set speed for X and for Y (or divide X by 2 for Y), set spritesheets, set maps(background, camera that only scrolls right and left, tiles drawing, player fence / limit))
         * enemy type / id (bee, wolf, eater of worlds)
         * better debug (and hide it when the player is beneth it)
         * core collision
         * title screen and menus
         * change the "using"
         * fix size in childs of gameobject
         * ini file with configuration.
         
         BUGS:
         * screen buffer for each camera
         * fix debug and inventory position for every player
         * moving things can get out of the map
         * fix multiple updates, and check for collision only within the camera.
         * knockback when you can't realy knock back get's weird.
         * small one: running status gets weird when the player colides with objects. it seems like the problem is in MovementManager.
         * unreachable: boxs can't get through the tree.
         * sometimes the enemy dosent attack you even if you get into its range -  youl need to get out and then he will attack you.
         */

        // Graphics and controls
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        public enum Depth { front, windowsSelector, windowsData, windows, above, player, projectiles, below, background }

        public static float DepthToFloat(Depth type)
        {
            return (float)type / 1000;
        }

        // public static Rectangle worldRect;
        public static Scene scene;

        // Timer list
        public static List<Timer> timersList = new List<Timer>();

        // Class lists
        private List<GameObject> tagList = new List<GameObject>();

        // TODO: Is this needed?
        public static ContentManager content;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            Content.RootDirectory = "Content";
       } 

        protected override void Initialize()
        {
            content = Content;
            scene = new Title();

            // Initialize keyboard (old) state
            oldState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Update newState
            KeyboardState newState = Keyboard.GetState();

            // Update
            foreach(Timer timer in timersList)
            {
                timer.Update(gameTime);
            }
            if (scene != null)
            {
                scene.Update(newState, oldState, gameTime);
            }
            

            // Update oldState
            oldState = newState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aqua);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if (scene != null)
            {
                scene.Draw(spriteBatch);
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }

        
        
        void Quit()
        {
            this.Exit();
        }
    }
}
