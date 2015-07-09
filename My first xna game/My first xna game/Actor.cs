using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace My_first_xna_game
{
    public class Actor : Spritesheet
    {
        public Pack pack;

        public float runningAcceleration = 0f;
        public float runningAccelerationSpeed = 0.01f;
        public int runningAccelerationMax = 0;

        private MovementManager.MovingState movingState;
        public MovementManager.MovingState MovingState
        {
            get { return movingState; }
            set
            {
                if (value != MovementManager.MovingState.running)
                {
                    runningAcceleration = 0;
                }
                movingState = value;
            }
        }

        public MovementManager.Auto autoMovement;
        public MovementManager.Direction direction = MovementManager.Direction.down;

        // TODO: Bad name. enableMovement is a name for a function. bool should be more like 'moving'
        public bool enableMovement = true;
        private Timer movementTimer = new Timer(10f);
        private Timer walkingTimer = new Timer(1000f);
        public int timesMoved = 0;
        private Random random = new Random();

        public Actor(Texture2D texture, Vector2 position, MovementManager.Auto autoMovement = MovementManager.Auto.off)
            : base(texture, position)
        {
            this.autoMovement = autoMovement;
        }

        protected override void UpdateActor()
        {
            UpdateHostile();

            //update speed and animation according to actor status
            switch (MovingState)
            {
                case MovementManager.MovingState.running:
                    if (runningAcceleration < runningAccelerationMax)
                    {
                        runningAcceleration += runningAccelerationSpeed;
                    }
                    speed = runningSpeed + runningAcceleration;
                    interval = 125f;
                    break;

                case MovementManager.MovingState.walking:
                    speed = walkingSpeed;
                    interval = 200f;
                    break;

                case MovementManager.MovingState.standing:
                    StopAnimation();
                    break;
            }

            //update auto movement
            switch (autoMovement)
            {
                case MovementManager.Auto.random:
                    if (movementTimer.result && walkingTimer.result)
                    {
                        MovingState = MovementManager.MovingState.walking;
                        movementManager.MoveActor(this, direction, (int)speed);
                        timesMoved++;
                        movementTimer.counter = 0f;
                    }


                    if (timesMoved >= random.Next(10, 100) * random.Next(1, 5))
                    {
                        MovingState = MovementManager.MovingState.standing;
                        direction = MovementManager.RandomDirection;
                        walkingTimer.counter = 0f;
                        timesMoved = 0;
                    }

                    break;
            }

        }

        protected virtual void UpdateHostile() { }

        protected override void move(MovementManager.Direction direction, int speed)
        {
            movementManager.MoveActor(this, direction, speed);
        }

        public void push(GameObject gameObject)
        {
            movementManager.MoveToDirection(gameObject, this.direction, (int)this.speed);
        }

    }
}
