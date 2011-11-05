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
        public static int DEFAULT_PLAYER_ACCELERATION = 4;
        public static int DEFAULT_PLAYER_MAX_VELOCITY = 4;

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
            base.InputHandler = new InputHandler(playerIndex);
            base.Body.FixedRotation = true;
            base.Body.IsStatic = false;
        }

        public override void Update(GameTime gameTime)
        {
            InputHandler.HandleInput(this);
            base.Update(gameTime);
        }

 
    }
}
