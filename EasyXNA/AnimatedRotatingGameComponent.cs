using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;

namespace EasyXNA
{
    public class AnimatedRotatingGameComponent : AnimatedGameComponent
    {
        public AnimatedRotatingGameComponent(EasyTopDownGame game, string sheetName)
            : base(game, sheetName)
        {
            
        }

        public override void InitializePhysics()
        {
            base.InitializePhysics();
            Body.FixedRotation = false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {           
            if (Body.LinearVelocity.Length() > 0) 
            {
                TurnSpriteToFaceTravelDirection();
                //Fire the animation frames if we're moving.
                ClickAnimationFrame(gameTime);
            }
            base.Update(gameTime);
        }

        private void TurnSpriteToFaceTravelDirection()
        {
            if (this.Body.LinearVelocity.Length() > 0)
            {
                Body.Rotation =  PhysicsHelper.VectorToRotation(Body.LinearVelocity);
            }
        }


    }
}
