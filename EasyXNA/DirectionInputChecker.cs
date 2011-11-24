using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EasyXNA
{
    public class DirectionInputChecker
    {

        public PlayerIndex PlayerIndex { get; set; }
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;

        public DirectionInputChecker(PlayerIndex playerIndex)
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


        public Vector2 GetInputVector(float acceleration)
        {
            float x = 0;
            float y = 0;


            if (isDirectionPressed(upKey, Buttons.DPadUp, Buttons.LeftThumbstickUp))
            {
                y = y - acceleration;
            }

            if (isDirectionPressed(downKey, Buttons.DPadDown, Buttons.LeftThumbstickDown))
            {
                y = y + acceleration;
            }

            if (isDirectionPressed(leftKey, Buttons.DPadLeft, Buttons.LeftThumbstickLeft))
            {
                x = x - acceleration;
            }

            if (isDirectionPressed(rightKey, Buttons.DPadRight, Buttons.LeftThumbstickRight))
            {
                x = x + acceleration;
            }


            return new Vector2(x, y);
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
