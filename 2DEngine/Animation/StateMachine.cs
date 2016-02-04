using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class that represents a connected set of animation states.
    /// Also contains global states which can be accessed to from anywhere (a good example of this, is Death).
    /// </summary>
    public class StateMachine
    {
        #region Properties and Fields

        /// <summary>
        /// A list of all the current states in our state machine.
        /// </summary>
        public List<State> States { get; set; }

        /// <summary>
        /// A list of all the current global states in our state machine.  Global states can be accessed from any state, i.e. Death.
        /// These are represented as transitions, because they are really the thing we are interested in.
        /// </summary>
        private List<Transition> GlobalTransitions { get; set; }

        /// <summary>
        /// The current state we are in.  The transitions from the active state and global transitions will be checked every frame.
        /// This is for optimization, as there is no need to check all off the states if we cannot reach them.
        /// </summary>
        public State ActiveState { get; private set; }

        /// <summary>
        /// The game object that is associated with this state machine.
        /// </summary>
        public GameObject ParentGameObject { get; private set; }

        #endregion

        public StateMachine(GameObject parentGameObject)
        {
            ParentGameObject = parentGameObject;
        }

        #region State Machine Update Functions

        /// <summary>
        /// Checks the transitions from our ActiveState and sets the new state if there was a change.
        /// Then, checks the global transitions and if there was a successful transition, sets the new state to be this state and returns.
        /// I.e. normal states can overridden on successful transition, but global states cannot.
        /// </summary>
        public void Update()
        {
            Debug.Assert(ActiveState != null);
            Debug.Assert(States.Count > 0);

            ActiveState = ActiveState.CheckTransitions();
            
            foreach (Transition state in GlobalTransitions)
            {
                if (state.CheckTransitionCondition())
                {
                    ActiveState = state.DestinationState;
                    return;
                }
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Adds a global state to our state machine.
        /// </summary>
        /// <param name="state">The global state.</param>
        /// <param name="transitionEvent">The condition that must be fulfilled to transition to this state.</param>
        public void AddGlobalState(State state, TransitionEventHandler transitionEvent)
        {
            Transition transition = new Transition(state);
            transition.TransitionEvent += transitionEvent;

            GlobalTransitions.Add(transition);
        }

        #endregion
    }
}
