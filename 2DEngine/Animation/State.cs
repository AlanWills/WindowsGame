using System.Collections.Generic;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class that represents a certain animation and a list of transitions to other possible states.
    /// </summary>
    public class State
    {
        /// <summary>
        /// The animation associated with this state.
        /// </summary>
        public Animation Animation { get; set; }

        /// <summary>
        /// A list of transitions to other states
        /// </summary>
        public List<Transition> Transitions { get; set; }

        #region State Transition Functions

        /// <summary>
        /// Iterates through the transitions and checks each condition.  If a condition is true, we return.
        /// </summary>
        /// <returns>Returns the state we transition to if a transition condition is met.  Otherwise, returns this state.</returns>
        public State CheckTransitions()
        {
            foreach (Transition transition in Transitions)
            {
                if (transition.CheckTransitionCondition())
                {
                    return transition.DestinationState;
                }
            }

            return this;
        }

        #endregion
    }
}