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
    public class Actor : Spritesheet
    {
        public float walkingSpeed = 2f;
        public float runningSpeed = 4f;
        public MovementManager.MovingState movingState;
        public MovementManager.Auto autoMovement;
        public MovementManager.Direction direction = MovementManager.Direction.down;
        public bool enableMovement = true;
        private Timer movementTimer = new Timer(10f);
        private Timer walkingTimer = new Timer(1000f);
        public int timesMoved = 0;
        private Random random = new Random();


        public Actor(Texture2D texture, Vector2 position, MovementManager.Auto autoMovement = MovementManager.Auto.off)
            : base(texture, position, Game.Depth.player, 0)
        {
            this.autoMovement = autoMovement;
        }

        protected override void UpdateActor()
        {
            UpdateHostile();

            //update speed and animation according to actor status
            switch (movingState)
            {
                case MovementManager.MovingState.running:
                    speed = runningSpeed;
                    interval = 125f;
                    break;

                case MovementManager.MovingState.walking:
                    speed = walkingSpeed;
                    interval = 200f;
                    break;

                case MovementManager.MovingState.standing:
                    speed = 0;
                    StopAnimation();
                    break;
            }

            //update auto movement
            switch (autoMovement)
            {
                case MovementManager.Auto.random:
                    if (movementTimer.result && walkingTimer.result)
                    {
                        movingState = MovementManager.MovingState.walking;
                        movementManager.MoveActor(this, direction, (int)speed);
                        timesMoved++;
                        movementTimer.counter = 0f;
                    }


                    if (timesMoved >= random.Next(10, 100) * random.Next(1, 5))
                    {
                        movingState = MovementManager.MovingState.standing;
                        direction = MovementManager.RandomDirection;
                        walkingTimer.counter = 0f;
                        timesMoved = 0;
                    }

                    break;
            }

        }

        protected virtual void UpdateHostile() { }

        public void push(GameObject gameObject)
        {
            movementManager.MoveToDirection(gameObject, this.speed, this.direction);
        }

    }
}
