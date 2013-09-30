using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;

namespace Pong
{
    class InputHelper
    {
        KeyboardState previousKeyboardState, currentKeyboardState;

        public InputHelper()
        {

        }

        public bool KeyPressed(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k) && !previousKeyboardState.IsKeyDown(k);
        }

        public bool KeyReleased(Keys k)
        {
            return !currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyDown(k);
        }

        public bool KeyDown(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k);
        }

        public bool KeyUp(Keys k)
        {
            return !KeyDown(k);
        }

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }
    }
}
