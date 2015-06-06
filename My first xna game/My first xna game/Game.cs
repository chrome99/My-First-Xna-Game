using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        /*players can go through portals.
         TODO:
         * jumping
         * defend & attack (sword)
         * combos & skills
         * animations and autotiles
         * round raduis for enemies
         * better AI for enemies
         * enemy type / id (bee, wolf, eater of worlds)
         * 
         * bloom shader
         * shadows
         * effects
         * better knockback
         * core collision
         * options
         * gamemodes
         * trade
         * pvp menu
         * levels (in level up you can upgrade skills or stats)
         * chat
         * command line
         * push and plate sound
         * item data(durabillity, price, weight, and more)
         * 
         * LAN
         * Best IA
         * 
         * :Fighter- bodybuilder, knight, barbarian :Roughe thief, clown: Mage
         * bodybuilder, lost hero, clown
         * special potions
         * building
         * turrets
         * redstone
         
         BUGS:
         * fix msgWindow intializing
         * remove depth
         * hud bug in low health
         */

        // Graphics and controls
        public static GraphicsDeviceManager graphics;
        public static bool endGame = false;
        SpriteBatch spriteBatch;
        KeyboardState oldState;

        public enum Depth { GUIFront, front, windowsSelector, windowsDataFront, windowsData, windows, above, jumping, player, projectiles, below, background }

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
        public static Rectangle worldRect;

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
            worldRect = new Rectangle(0, 0, 1920, 1080);
            scene = new Title(graphics);

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

            //if exit
            if (endGame)
            {
                Exit();
            }

            // Update newState
            KeyboardState newState = Keyboard.GetState();

            // Update
            foreach (Timer timer in timersList)
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
            if (scene != null)
            {
                scene.Draw(spriteBatch);
            }
            //if (spriteBatch.)
            //spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
