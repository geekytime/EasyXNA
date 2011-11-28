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
            Acceleration = 20f;
            base.MaxVelocity = 2f;
            base.InputChecker = new DirectionInputChecker(playerIndex);
            base.currentAnimationFrame = 0;
            Body.LinearDamping = 30;
            Body.Mass = 3;
            Body.Restitution = 0f;
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
            //SlowDownFastIfDirectionChanges(inputVector);
            ApplyInputVector(inputVector);
            //MaintainMaxVelocity();
        }

        private void SlowDownFastIfDirectionChanges(Vector2 inputVector)
        {
            Vector2 currentVelocity = Body.LinearVelocity;

            if ( inputVector.X == 0 || (currentVelocity.X > 0 && inputVector.X < 0))
            {
                Body.ApplyLinearImpulse(new Vector2(-currentVelocity.X, 0));
            }

            if ( inputVector.X == 0 || (currentVelocity.X < 0 && inputVector.X > 0) )
            {
                Body.ApplyLinearImpulse(new Vector2(-currentVelocity.X, 0));
            }

            if (inputVector.Y == 0 || (currentVelocity.Y > 0 && inputVector.Y < 0))
            {
                Body.ApplyLinearImpulse(new Vector2( 0,-currentVelocity.Y));
            }

            if (inputVector.Y == 0|| (currentVelocity.Y < 0 && inputVector.Y > 0) )
            {
                Body.ApplyLinearImpulse(new Vector2(0, -currentVelocity.Y));
            }

        }

        private void ApplyInputVector(Vector2 inputVector)
        {            
            Body.ApplyLinearImpulse(inputVector);
        }

        private void MaintainMaxVelocity()
        {
            if (Body.LinearVelocity.X > MaxVelocity)
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


            if (Body.LinearVelocity.Y < 0 && YIsGreaterThanX(Body.LinearVelocity))
            {
                direction = AnimatedGameComponentDirection.Back;
                return;
            }

            if (Body.LinearVelocity.Y > 0 && YIsGreaterThanX(Body.LinearVelocity))
            {
                direction = AnimatedGameComponentDirection.Front;
                return;
            }


            if (Body.LinearVelocity.X < 0)
            {
                direction = AnimatedGameComponentDirection.Left;
            }
            else if (Body.LinearVelocity.X > 0)
            {
                direction = AnimatedGameComponentDirection.Right;
            }
        }

        protected bool YIsGreaterThanX(Vector2 vector2)
        {
            if (Math.Abs(vector2.Y) > Math.Abs(vector2.X))
            {
                return true;
            }
            return false;
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