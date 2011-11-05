using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace EasyXNA
{
    public class AnimatedPlayer4DirectionGameComponent : AnimatedGameComponent
    {
        PlayerIndex playerIndex;
        protected AnimatedGameComponentDirection lastDirection = AnimatedGameComponentDirection.Front;
        protected AnimatedGameComponentDirection direction = AnimatedGameComponentDirection.Front;
        public PlayerDisplayData PlayerDisplayData { get { return (PlayerDisplayData) DisplayData; } }

        public AnimatedPlayer4DirectionGameComponent(EasyTopDownGame easyGame, String imageName, PlayerIndex playerIndex)
            : base(easyGame, imageName)
        {
            this.playerIndex = playerIndex;
            base.Acceleration = 6;
            base.MaxVelocity = PlayerGameComponent.DEFAULT_PLAYER_MAX_VELOCITY;
            base.InputHandler = new InputHandler(playerIndex);
            base.currentAnimationFrame = 0;
            base.DisplayData = new PlayerDisplayData(game, playerIndex);
        }


        public override void Update(GameTime gameTime)
        {
            InputHandler.HandleInput(this);           
 
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
            base.Update(gameTime);
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
    }
}