using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EasyXNA
{
    public class InputHandlerComponent : GameComponent
    {
        EasyTopDownGame game;
        Action callback;
        PlayerIndex playerIndex;
        List<Keys> keys;
        List<Buttons> buttons;

        GamePadState lastGamePadState = new GamePadState();
        KeyboardState lastKeyboardState = new KeyboardState();

        public InputHandlerComponent(EasyTopDownGame game, Action callback, PlayerIndex playerIndex, params object[] inputs) : base(game){
            this.game = game;
            this.callback = callback;
            this.playerIndex = playerIndex;
            this.InitializeInputs(inputs);
        }

        private void InitializeInputs(object[] inputs)
        {
            keys = new List<Keys>();
            buttons = new List<Buttons>();
            foreach (object input in inputs)
            {
                if (input is Keys)
                {
                    keys.Add((Keys)input);
                }
                else if (input is Buttons)
                {
                    buttons.Add((Buttons)input);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (buttons.Count > 0)
            {
                bool buttonsPressed = CheckButtons();
                if (buttonsPressed)
                {
                    return;
                }
            }

            if (keys.Count > 0)
            {
                CheckKeys();
            }            
            base.Update(gameTime);
        }

        private bool CheckButtons()
        {
            GamePadState currentGamePadState = GamePad.GetState(playerIndex);            

            foreach (Buttons button in buttons)
            {
                if (currentGamePadState.IsButtonDown(button) && (lastGamePadState.IsButtonDown(button) == false))
                {
                    callback();
                    lastGamePadState = currentGamePadState;
                    return true;
                }
            }

            lastGamePadState = currentGamePadState;
            return false;
        }

        private void CheckKeys()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            foreach (Keys key in keys)
            {
                if (currentKeyboardState.IsKeyDown(key) && (lastKeyboardState.IsKeyDown(key) == false))
                {
                    callback();
                    lastKeyboardState = currentKeyboardState;
                    return;
                }
            }
            lastKeyboardState = currentKeyboardState;
        }


    }

}
