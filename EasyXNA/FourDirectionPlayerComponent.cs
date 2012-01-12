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
            UpdateAnimationFrame(gameTime);
            SetDirectionBasedOnInputVector(inputVector);
            base.Update(gameTime);
        }

        private void UpdateAnimationFrame(GameTime gameTime)
        {
            if (Body.LinearVelocity.Length() > .1)
            {
                ClickAnimationFrame(gameTime);
                lastDirection = direction;
            }
        }

        private void SetVelocityBasedOnInputVector(GameTime gameTime, Vector2 inputVector)
        {
            Body.ApplyLinearImpulse(inputVector);
        }

        private void SetDirectionBasedOnInputVector(Vector2 inputVector)
        {
            if (inputVector.Y < 0 && YIsGreaterThanX(inputVector))
            {
                direction = AnimatedGameComponentDirection.Back;
                return;
            }

            if (inputVector.Y > 0 && YIsGreaterThanX(inputVector))
            {
                direction = AnimatedGameComponentDirection.Front;
                return;
            }

            if (inputVector.X < 0)
            {
                direction = AnimatedGameComponentDirection.Left;
            }
            else if (inputVector.X > 0)
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