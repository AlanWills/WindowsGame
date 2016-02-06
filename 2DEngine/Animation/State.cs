using System.Collections.Generic;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class that represents a certain animation and a list of transitions to other possible states.
    /// </summary>
    public class State
    {
        #region Properties and Fields

        /// <summary>
        /// The animation associated with this state.
        /// </summary>
        public Animation Animation { get; set; }

        /// <summary>
        /// A list of transitions to other states
        /// </summary>
        public List<Transition> Transitions { get; set; }

        #endregion

        public State(Animation animation)
        {
            Animation = animation;

            Transitions = new List<Transition>();
        }

        #region State Update Functions

        public void Update(float elapsedGameTime)
        {
            Animation.Update(elapsedGameTime);
        }

        #endregion

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
                    Animation.Reset();
                    transition.DestinationState.Animation.IsPlaying = true;

                    return transition.DestinationState;
                }
            }

            return this;
        }

        #endregion

        #region Transition Utility Functions

        /// <summary>
        /// Adds a transition between this and the inputted state with the inputted transition condition.
        /// </summary>
        /// <param name="destinationState">The state we will transition to.</param>
        /// <param name="transitionEvent">The condition that must be satisfied for a transition to occur.</param>
        public void AddTransition(State destinationState, TransitionEventHandler transitionEvent)
        {
            // For now, don't support transitions to yourself.  This just makes no sense really.
            Debug.Assert(destinationState != this);

            Transitions.Add(new Transition(this, destinationState, transitionEvent));
        }

        #endregion
    }
}