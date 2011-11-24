using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace EasyXNA
{
    public class FourDirectionPlayerComponent : AnimatedGameComponent
    {
        PlayerIndex playerIndex;
        protected AnimatedGameComponentDirection lastDirection = AnimatedGameComponentDirection.Front;
        protected AnimatedGameComponentDirection direction = AnimatedGameComponentDirection.Front;        
        public PlayerScoreDisplay PlayerDisplayData {get; set;}


        public FourDirectionPlayerComponent(EasyTopDownGame easyGame, String imageName, PlayerIndex playerIndex)
            : base(easyGame, imageName)
        {
            this.playerIndex = playerIndex;
            base.Acceleration = 6;
            base.MaxVelocity = PlayerGameComponent.DEFAULT_PLAYER_MAX_VELOCITY;
            base.InputChecker = new DirectionInputChecker(playerIndex);
            base.currentAnimationFrame = 0;            
        }


        public override void Update(GameTime gameTime)
        {
            Vector2 inputVector = InputChecker.GetInputVector(Acceleration);
            SetVelocityBasedOnInputVector(gameTime, inputVector);


                SetDirectionBasedOnVelocity(gameTime);
            base.Update(gameTime);
        }

        private void SetVelocityBasedOnInputVector(GameTime gameTime, Vector2 inputVector)
        {
            Vector2 current = Body.LinearVelocity;
            SlowDownFastIfDirectionChanges(inputVector);
            ApplyInputVector(inputVector);
            MaintainMaxVelocity();
        }

        private void SlowDownFastIfDirectionChanges(Vector2 inputVector)
        {
            Vector2 currentVelocity = Body.LinearVelocity;

            if (currentVelocity.X > 0 && inputVector.X < 0 || inputVector.X == 0)
            {
                Body.ApplyLinearImpulse(new Vector2(-currentVelocity.X, 0));
            }

            if (currentVelocity.X < 0 && inputVector.X > 0 || inputVector.X == 0)
            {
                Body.ApplyLinearImpulse(new Vector2(-currentVelocity.X, 0));
            }

            if (currentVelocity.Y > 0 && inputVector.Y < 0 || inputVector.Y == 0)
            {
                Body.ApplyLinearImpulse(new Vector2( 0,-currentVelocity.Y));
            }

            if (currentVelocity.Y < 0 && inputVector.Y > 0 || inputVector.Y == 0)
            {
                Body.ApplyLinearImpulse(new Vector2(0, -currentVelocity.Y));
            }

            if (Body.LinearVelocity.Length() > 0 && Body.LinearVelocity.Length() < 1)
            {
                Body.LinearVelocity = Vector2.Zero;
            }
        }

        private void ApplyInputVector(Vector2 inputVector)
        {
            Vector2 xPart = new Vector2(inputVector.X, 0);
            Vector2 yPart = new Vector2(0, inputVector.Y);

            if (Math.Abs(Body.LinearVelocity.X) < MaxVelocity  && xPart.Length() > 0)
            {
                Body.ApplyLinearImpulse(xPart);
            }
            if (Math.Abs(Body.LinearVelocity.Y) < MaxVelocity && yPart.Length() > 0)
            {
                Body.ApplyLinearImpulse(yPart);
            }
        }

        private void MaintainMaxVelocity()
        {
            if (Math.Abs(Body.LinearVelocity.X) > MaxVelocity)
            {
                Vector2 xPart = new Vector2(-Body.LinearVelocity.X, 0);
                Body.ApplyLinearImpulse(xPart);
            }
            if (Math.Abs(Body.LinearVelocity.Y) > MaxVelocity)
            {
                Vector2 yPart = new Vector2(0, -Body.LinearVelocity.Y);
                Body.ApplyLinearImpulse(yPart);
            }            
        }

        private void SetDirectionBasedOnVelocity(GameTime gameTime)
        {
            if (Body.LinearVelocity.Length() > 0)
            {
                ClickAnimationFrame(gameTime);
                lastDirection = direction;
            }

            if (Body.LinearVelocity.X == 0)
            {
                if (Body.LinearVelocity.Y < 0)
                {
                    direction = AnimatedGameComponentDirection.Back;
                }
                else if (Body.LinearVelocity.Y > 0)
                {
                    {
                        direction = AnimatedGameComponentDirection.Front;
                    }
                }
            }
            else if (Body.LinearVelocity.X < 0)
            {
                direction = AnimatedGameComponentDirection.Left;
            }
            else
            {
                direction = AnimatedGameComponentDirection.Right;
            }
        }


        protected override String BuildActiveSpriteSuffix()
        {
            return direction.ToString().ToLower() + "-" + currentAnimationFrame.ToString();
        }

        protected override int ClickAnimationFrame(GameTime gameTime)
        {
            if (direction == lastDirection)
            {
                return base.ClickAnimationFrame(gameTime);
            }
            else
            {
                return 0;
            }

        }

        public Vector2 GetProjectileDirection()
        {
            if (direction == AnimatedGameComponentDirection.Back)
            {
                return ProjectileDirections.Up;
            }
            else if (direction == AnimatedGameComponentDirection.Front)
            {
                return ProjectileDirections.Down;
            }
            else if (direction == AnimatedGameComponentDirection.Left)
            {
                return ProjectileDirections.Left;
            }
            else if (direction == AnimatedGameComponentDirection.Right)
            {
                return ProjectileDirections.Right;
            }
            return Vector2.Zero;
        }
    }
}