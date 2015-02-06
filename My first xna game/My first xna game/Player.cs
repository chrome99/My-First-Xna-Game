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
    public class Player : Hostile
    {
        public Pack pack;
        public PlayerKeys keys;
        //public Camera cameraCenter;
        private bool playerMoving = false;
        private bool playerRunning = false;
        private int relesedKeysCount;
        private bool fireballkeyReleased = false;
        private bool menuKeyReleased = false;
        public struct PlayerKeys
        {
            public Keys up;
            public Keys down;
            public Keys left;
            public Keys right;
            public Keys attack;
            public Keys run;
            public Keys menu;
        }

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position, MovementManager.Auto.off)
        {

            pack = new Pack();
        }

        public void runningSwitch()
        {
            if (enableRunning)
            {
                enableRunning = false;
            }
            else
            {
                enableRunning = true;
            }
        }

        protected override void UpdatePlayer()
        {
            
        }

        public void UpdateInput(KeyboardState newState, KeyboardState oldState, ContentManager Content, Map map)
        {
            if (!alive) { return; }
            relesedKeysCount = 0;

            //update menu
            if (newState.IsKeyDown(keys.menu) && menuKeyReleased)
            {
                Game.scene = new Inventory(map, this);

                menuKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(keys.menu))
            {
                menuKeyReleased = true;
            }

            // -Update player speed state
            if (newState.IsKeyDown(keys.run) && enableRunning)
            {
                playerRunning = true;
            }
            else // reset player speed speed state
            {
                playerRunning = false;
            }

            // -Update player through debug button
            if (newState.IsKeyDown(Keys.LeftControl))
            {
                through = true;
            }
            else // reset player speed speed state
            {
                through = false;
            }


            //if perssed attack
            if (newState.IsKeyDown(keys.attack) && fireballkeyReleased)
            {
                Projectile projectile = new Projectile(map ,Content.Load<Texture2D>("wolf"), 6, this, 50);
                projectile.through = true;

                fireballkeyReleased = false;
            }
            else if (!oldState.IsKeyDown(keys.attack))
            {
                fireballkeyReleased = true;
            }
            // if pressed left
            if (newState.IsKeyDown(keys.left))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.left, (int)speed);
                
            }
            else
            {
                relesedKeysCount++;
            }

            // if pressed right
            if (newState.IsKeyDown(keys.right))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.right, (int)speed);
            }
            else
            {
                relesedKeysCount++;
            }

            // if pressed up
            if (newState.IsKeyDown(keys.up))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.up, (int)speed);
            }
            else
            {
                relesedKeysCount++;
            }

            // if pressed down
            if (newState.IsKeyDown(keys.down))
            {
                playerMoving = movementManager.MoveActor(this, MovementManager.Direction.down, (int)speed);
            }
            else
            {
                relesedKeysCount++;
            }

            //if all four arrows relesed
            if (relesedKeysCount >= 4)
            {
                playerMoving = false;
            }

            //-Update player movement status

            if (playerMoving && playerRunning)
            {
                movingState = MovementManager.MovingState.running;

            }
            else if (playerMoving)
            {
                movingState = MovementManager.MovingState.walking;
            }
            else
            {
                movingState = MovementManager.MovingState.standing;
            }
        }
    }
}
