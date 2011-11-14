using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EasyXNA
{
    public class RotatingPlayerComponent : AnimatedRotatingGameComponent
    {
        PlayerIndex playerIndex;
        public RotatingPlayerComponent(EasyTopDownGame game, String sheetName, PlayerIndex playerIndex)
            : base(game, sheetName)
        {
            this.playerIndex = playerIndex;
            base.Acceleration = 6;
            base.MaxVelocity = PlayerGameComponent.DEFAULT_PLAYER_MAX_VELOCITY;
            base.InputHandler = new InputHandler(playerIndex);
            base.currentAnimationFrame = 0;            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
