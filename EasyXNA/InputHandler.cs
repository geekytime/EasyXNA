using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EasyXNA
{
    public class InputHandler
    {

        public PlayerIndex PlayerIndex { get; set; }
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;

        public InputHandler(PlayerIndex playerIndex)
        {
            this.PlayerIndex = playerIndex;
            if (playerIndex == PlayerIndex.Two)
            {
                SetPlayerTwoKeys();
            }
        }

        private void SetPlayerTwoKeys()
        {
            upKey = Keys.W;
            downKey = Keys.S;
            leftKey = Keys.A;
            rightKey = Keys.D;

        }


        public void HandleInput(EasyGameComponent component)
        {
            float x = 0;
            float y = 0;

            if (component.AllowVerticalMovement == true)
            {
                if (isDirectionPressed(upKey, Buttons.DPadUp, Buttons.LeftThumbstickUp))
                {
                    y = y - component.Acceleration;
                }
                else if (component.Body.LinearVelocity.Y < 0)
                {
                    y = 0;
                }


                if (isDirectionPressed(downKey, Buttons.DPadDown, Buttons.LeftThumbstickDown))
                {
                    y = y + component.Acceleration;
                }
                else if (component.Body.LinearVelocity.Y > 0)
                {
                    y = 0;
                }

            }

            if (component.AllowHorizontalMovement == true)
            {

                if (isDirectionPressed(leftKey, Buttons.DPadLeft, Buttons.LeftThumbstickLeft))
                {
                    x = x - component.Acceleration;
                }
                else if (component.Body.LinearVelocity.X < 0) //if we're moving left
                {
                    x = 0;
                }

                if (isDirectionPressed(rightKey, Buttons.DPadRight, Buttons.LeftThumbstickRight))
                {
                    x = x + component.Acceleration;
                }
                else if (component.Body.LinearVelocity.X > 0) //if we're moving right
                {
                    x = 0;
                }
            }

            component.Body.LinearVelocity = new Vector2(x, y);

        }

        private bool isDirectionPressed(Keys key, Buttons dpad, Buttons thumbstick)
        {
            if (Keyboard.GetState().IsKeyDown(key))
            {
                return true;
            }
            if (GamePad.GetState(this.PlayerIndex).IsButtonDown(dpad))
            {
                return true;
            }
            if (GamePad.GetState(this.PlayerIndex).IsButtonDown(thumbstick))
            {
                return true;
            }
            return false;
        }
    }
}
