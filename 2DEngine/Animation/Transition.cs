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
    public delegate bool TransitionEventHandler();

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
        /// The state that we would transition to if the transition condition was met.
        /// </summary>
        public State DestinationState { get; private set; }

        #endregion

        public Transition(State destinationState)
        {
            DestinationState = destinationState;
        }

        #region

        /// <summary>
        /// This function calls the event(s) on the transition event handler and returns whether this transition condition is fulfilled.
        /// If true, the state machine has moved to a new state.
        /// </summary>
        /// <returns>Returns true if transition condition is fulfilled.</returns>
        public bool CheckTransitionCondition()
        {
            // The transition event should not be null
            Debug.Assert(TransitionEvent != null);

            return TransitionEvent();
        }

        #endregion
    }
}
