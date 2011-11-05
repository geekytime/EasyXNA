using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class AnimatedRotatingEnemyComponent : AnimatedRotatingGameComponent
    {
        double currentActionCompletionTime = 0;

        public AnimatedRotatingEnemyComponent(EasyTopDownGame game, string sheetName)
            : base(game, sheetName)
        {
            this.Acceleration = 4;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (TimeForNewAction(gameTime) == true)
            {
                ChooseRandomDirection();
                ChooseRandomDuration(gameTime);
            }            
            base.Update(gameTime);         
        }

        private void ChooseRandomDirection()
        {
            Vector2 directionUnitVector = PhysicsHelper.GetRandomDirectionUnitVector();
            Vector2 speedVector = directionUnitVector * Acceleration;
            this.Body.LinearVelocity = speedVector;
        }

        private void ChooseRandomDuration(GameTime gameTime)
        {
            double duration = RandomHelper.DoubleInRange(.5, 5);
            this.currentActionCompletionTime = gameTime.TotalGameTime.TotalSeconds + duration;
        }

        private bool TimeForNewAction(GameTime gameTime)
        {
            if (Body.LinearVelocity.Length() < Acceleration *.8)
            {
                return true;
            }

            if (gameTime.TotalGameTime.TotalSeconds > currentActionCompletionTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
