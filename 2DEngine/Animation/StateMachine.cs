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

        public StateMachine(GameObject parentGameObject, State startingState)
        {
            ParentGameObject = parentGameObject;
            ActiveState = startingState;

            States = new List<State>();
            GlobalTransitions = new List<Transition>();
        }

        #region State Machine Update Functions

        /// <summary>
        /// Calls LoadContent on every State in this state machine.
        /// </summary>
        public void LoadContent()
        {
            foreach (State state in States)
            {
                state.Animation.LoadContent();
            }

            foreach (Transition globalState in GlobalTransitions)
            {
                globalState.DestinationState.Animation.LoadContent();
            }

            ActiveState.Animation.IsPlaying = true;
        }

        /// <summary>
        /// Checks the transitions from our ActiveState and sets the new state if there was a change.
        /// Then, checks the global transitions and if there was a successful transition, sets the new state to be this state and returns.
        /// I.e. normal states can overridden on successful transition, but global states cannot.
        /// </summary>
        public void Update(float elapsedGameTime)
        {
            Debug.Assert(ActiveState != null);
            Debug.Assert(States.Count > 0);

            ActiveState.Update(elapsedGameTime);

            ActiveState = ActiveState.CheckTransitions();

            // Check to make sure we have transition to a state that exists in our state machine.
            Debug.Assert(States.Exists(x => x == ActiveState) || GlobalTransitions.Exists(x => x.DestinationState == ActiveState));
            
            foreach (Transition transition in GlobalTransitions)
            {
                if (transition.DestinationState != ActiveState && transition.CheckTransitionCondition())
                {
                    ActiveState.Animation.Reset();

                    ActiveState = transition.DestinationState;
                    ActiveState.Animation.IsPlaying = true;
                    return;
                }
            }
        }

        #endregion

        #region Global Transitions

        public void AddGlobalTransition(State destinationState, TransitionEventHandler transitionEvent)
        {
            GlobalTransitions.Add(new Transition(null, destinationState, transitionEvent));
        }

        #endregion
    }
}
