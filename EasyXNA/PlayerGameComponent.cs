using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EasyXNA
{
    public class PlayerGameComponent : EasyGameComponent
    {
        public static float DEFAULT_PLAYER_ACCELERATION = 20f;
        public static int DEFAULT_PLAYER_MAX_VELOCITY = 8;

        PlayerIndex playerIndex;
        PlayerIndex PlayerIndex
        {
            get { return this.playerIndex; }
        }
       
        public PlayerGameComponent(EasyTopDownGame easyGame, String imageName, PlayerIndex playerIndex)
            : base(easyGame, imageName)
        {
            this.playerIndex = playerIndex;
            base.Acceleration = DEFAULT_PLAYER_ACCELERATION;
            base.MaxVelocity = DEFAULT_PLAYER_MAX_VELOCITY;
            base.InputChecker = new DirectionInputChecker(playerIndex);
            base.Body.FixedRotation = true;
            base.Body.IsStatic = false;
            Body.LinearDamping = 30;
            Body.Mass = 3;
            Body.Restitution = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 inputVector = InputChecker.GetInputVector(Acceleration);
            Vector2 adjustedVector = ApplyMovementRules(inputVector);

            Body.ApplyLinearImpulse(adjustedVector);
            base.Update(gameTime);
        }

        protected Vector2 ApplyMovementRules(Vector2 inputVector)
        {
            Vector2 adjustedVector = new Vector2(0, 0) ;
            if (AllowHorizontalMovement == false)
            {
                adjustedVector = new Vector2(0, inputVector.Y);
            }
            if (AllowVerticalMovement == false)
            {
                adjustedVector = new Vector2(inputVector.X, 0);
            }
            return adjustedVector;
        }
    }
}
