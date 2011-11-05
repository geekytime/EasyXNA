using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyXNA;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class WanderingEnemyGameComponent : EasyGameComponent
    {
        int nextChangeInTotalSeconds = 0;

        public int DecisionCycleMaxLength { get; set; }
        public int DecisionCycleMinLength { get; set; }
        Random random;
        Vector2 currentDirection;

        public WanderingEnemyGameComponent(EasyTopDownGame easyGame, String imageName)
            : base(easyGame, imageName)
        {
            DecisionCycleMinLength = 0;
            DecisionCycleMaxLength = 5;
            Acceleration = 2;
            currentDirection = Vector2.Zero;            
            Body.FixedRotation = true;
        }

        public override void Initialize()
        {
            random = new Random((int)DateTime.Now.Ticks);
            base.Initialize();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {            
            
            base.Update(gameTime);
            if (gameTime.TotalGameTime.TotalSeconds > nextChangeInTotalSeconds)
            {
                currentDirection = getRandomDirection();
                nextChangeInTotalSeconds = (int)Math.Round(gameTime.TotalGameTime.TotalSeconds) + getRandomSeconds();
                Body.LinearVelocity = currentDirection;
            }
            
        }

        private Vector2 getRandomDirection()
        {            
            float x = (float)random.NextDouble() * 10 - 5;
            float y = (float)random.NextDouble() * 10 - 5;
            Vector2 randomVector = new Vector2(x, y);
            randomVector.Normalize();
            return randomVector * Acceleration;
        }

        private int getRandomSeconds()
        {
            return random.Next(DecisionCycleMinLength, DecisionCycleMaxLength);
        }

    }
}
