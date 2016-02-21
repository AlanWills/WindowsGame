﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        /// A cached list of keys that have just been pressed to save us having to recalculate in the same frame for multiple calls of GetPressedKeys
        /// </summary>
        private static Keys[] PressedKeys { get; set; }

        private static bool calculatedPressedKeys = false;

        /// <summary>
        /// Updates the keyboard states
        /// </summary>
        public static void Update()
        {
            calculatedPressedKeys = false;

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
        /// Returns all the keys which are currently down
        /// </summary>
        /// <returns></returns>
        public static Keys[] GetKeysDown()
        {
            return CurrentKeyboardState.GetPressedKeys();
        }

        /// <summary>
        /// Returns all the keys which were not down last frame, but are up now
        /// </summary>
        /// <returns></returns>
        public static Keys[] GetPressedKeys()
        {
            if (!calculatedPressedKeys)
            {
                // Get all the keys in thisFrame that weren't in last frame
                PressedKeys = Array.FindAll(CurrentKeyboardState.GetPressedKeys(), x => Array.IndexOf(PreviousKeyboardState.GetPressedKeys(), x) == -1);
                calculatedPressedKeys = true;
            }

            return PressedKeys;
        }

        #endregion
    }
}