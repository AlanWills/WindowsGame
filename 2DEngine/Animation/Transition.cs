using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class used for basic transition conditions
    /// </summary>
    public class TransitionArgs : EventArgs
    {
        
    }

    /// <summary>
    /// An event handler description for our transition event.
    /// Possibly will take custom args, but haven't worked that out yet.
    /// </summary>
    public delegate bool TransitionEventHandler(State source, State destination);

    /// <summary>
    /// A class representing a condition between two animation states.
    /// </summary>
    public class Transition
    {
        #region Properties and Fields

        /// <summary>
        /// An event handler that 
        /// </summary>
        public event TransitionEventHandler TransitionEvent;

        /// <summary>
        /// The state that we are transitioning from.  Can be nullptr if it is from a global transition.
        /// </summary>
        public State SourceState { get; private set; }

        /// <summary>
        /// The state that we would transition to if the transition condition was met.
        /// </summary>
        public State DestinationState { get; private set; }

        #endregion

        public Transition(State sourceState, State destinationState, TransitionEventHandler transitionEvent)
        {
            SourceState = sourceState;
            DestinationState = destinationState;
            TransitionEvent += transitionEvent;
        }

        #region Utility Functions

        /// <summary>
        /// This function calls the event(s) on the transition event handler and returns whether this transition condition is fulfilled.
        /// If true, the state machine has moved to a new state.
        /// </summary>
        /// <returns>Returns true if transition condition is fulfilled.</returns>
        public bool CheckTransitionCondition()
        {
            // The transition event should not be null
            Debug.Assert(TransitionEvent != null);
            Debug.Assert(DestinationState != null);

            return TransitionEvent(SourceState, DestinationState);
        }

        #endregion

        #region State Transition Events - some generic, common events

        /// <summary>
        /// Transition if the left or right movement keys are down.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool IsMovementKeyDown(State source, State destination)
        {
            return GameKeyboard.IsKeyDown(InputMap.MoveLeft) || GameKeyboard.IsKeyDown(InputMap.MoveRight);
        }

        public static bool IsMovementKeyNotDown(State source, State destination)
        {
            return !IsMovementKeyDown(source, destination);
        }

        /// <summary>
        /// Transition if the source animation has finished playing.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static bool AnimationComplete(State source, State destination)
        {
            // If we are using this condition, the animation cannot be continual.
            Debug.Assert(source.Animation.Continual == false);

            return !source.Animation.IsPlaying;
        }

        #endregion
    }
}
