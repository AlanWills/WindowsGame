using Microsoft.Xna.Framework.Input;

namespace _2DEngine
{
    /// <summary>
    /// A static class used to query input from the Keyboard.
    /// </summary>
    public static class GameKeyboard
    {
        /// <summary>
        /// The current state of the keyboard - we can use this to work out what keys are down etc.
        /// </summary>
        private static KeyboardState CurrentKeyboardState { get; set; }

        /// <summary>
        /// The previous state of the keyboard - we can use this, and the current state, to work out what keys were pressed this frame
        /// </summary>
        private static KeyboardState PreviousKeyboardState { get; set; }

        /// <summary>
        /// Updates the keyboard states
        /// </summary>
        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        #region Utility Functions for Querying Key States

        /// <summary>
        /// Queries the current keyboard state to work out whether the inputted key is down
        /// </summary>
        /// <param name="key">The key we wish to query</param>
        /// <returns>Returns true if the key is down this frame</returns>
        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Queries the current and previous keyboard states to work out whether a key has been pressed
        /// </summary>
        /// <param name="key">The key we wish to query</param>
        /// <returns>Returns true if the key is down this frame and up the previous frame</returns>
        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        #endregion
    }
}