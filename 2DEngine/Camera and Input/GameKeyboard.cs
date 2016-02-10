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

        /// <summary>
        /// Transition if the InputMap movement keys are down.
        /// </summary>
        /// <returns></returns>
        public static bool IsWalkKeyDown()
        {
            return IsKeyDown(InputMap.MoveLeft) || IsKeyDown(InputMap.MoveRight);
        }

        /// <summary>
        /// Transition if the InputMap run key is down.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsRunKeyDown()
        {
            return IsKeyDown(InputMap.Run);
        }

        /// <summary>
        /// Transition if the InputMap movement and run keys are down.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsWalkAndRunKeyDown()
        {
            return IsWalkKeyDown() && IsRunKeyDown();
        }

        /// <summary>
        /// Transitions if the InputMap movements keys are not down.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsWalkKeyNotDown()
        {
            return !IsWalkKeyDown();
        }

        /// <summary>
        /// Transitions if the InputMap run key is not down.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsRunKeyNotDown()
        {
            return !IsRunKeyDown();
        }

        /// <summary>
        /// Transitions if the InputMap movement and run keys are not down.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsWalkAndRunKeyNotDown()
        {
            return !IsWalkAndRunKeyDown();
        }

        /// <summary>
        /// Transitions if the InputMap jump key is pressed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsJumpKeyPressed()
        {
            return IsKeyPressed(InputMap.Jump);
        }

        /// <summary>
        /// Transitions if the InputMap roll key is pressed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsRollKeyPressed()
        {
            return IsKeyPressed(InputMap.ForwardRoll);
        }

        #endregion
    }
}