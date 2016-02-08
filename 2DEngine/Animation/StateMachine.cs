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
        private State[] States { get; set; }

        /// <summary>
        /// A list of all the current global states in our state machine.  Global states can be accessed from any state, i.e. Death.
        /// These are represented as transitions, because they are really the thing we are interested in.
        /// </summary>
        private List<State> GlobalStates { get; set; }

        /// <summary>
        /// The current state we are in.  The transitions from the active state and global transitions will be checked every frame.
        /// This is for optimization, as there is no need to check all off the states if we cannot reach them.
        /// </summary>
        public State ActiveState { get; private set; }

        /// <summary>
        /// The starting state for the state machine obtained from the Character's data xml file.
        /// </summary>
        public uint StartingState { private get; set; }

        /// <summary>
        /// A flag to determine whether we should do a full load.
        /// </summary>
        private bool ShouldLoad { get; set; }

        // A field just used for validating addition of states in the correct order.
        private uint currentAddedStates = 0;

        #endregion

        public StateMachine(Character character, uint numAnimations)
        {
            character.BehaviourChanged += HandleBehaviourChange;

            States = new State[numAnimations];
            GlobalStates = new List<State>();
            ShouldLoad = true;
        }

        #region State Machine Update Functions

        /// <summary>
        /// Calls LoadContent on every State in this state machine.
        /// </summary>
        public void LoadContent()
        {
            if (!ShouldLoad) { return; }

            foreach (State state in States)
            {
                state.Animation.LoadContent();
            }

            Debug.Assert(States[StartingState] != null);
            ActiveState = States[StartingState];
            
            ShouldLoad = false;
        }

        /// <summary>
        /// Checks the transitions from our ActiveState and sets the new state if there was a change.
        /// Then, checks the global transitions and if there was a successful transition, sets the new state to be this state and returns.
        /// I.e. normal states can overridden on successful transition, but global states cannot.
        /// </summary>
        public void Update(float elapsedGameTime)
        {
            Debug.Assert(ActiveState != null);

            ActiveState.Update(elapsedGameTime);
        }

        #endregion

        #region Adding States Functions

        /// <summary>
        /// Adds a state to this state machine.  This function takes care of global states and ID checks.
        /// </summary>
        /// <param name="state"></param>
        public void AddState(State state)
        {
            // This is a check to make sure that we are adding the states in the order they are declared in the enum.
            // If this doesn't occur, the states will be mixed up and all hell will break loose.
            Debug.Assert(state.StateID == currentAddedStates);

            States[currentAddedStates] = state;
            currentAddedStates++;

            // If our state is global, add it to our list
            if (state.IsGlobal)
            {
                GlobalStates.Add(state);
            }
        }

        #endregion

        #region Behaviour Change Handling

        /// <summary>
        /// Checks the current active state's transitions against the new behaviour state.
        /// Performs no checking if the new state and current ActiveState have the same ID.
        /// </summary>
        /// <param name="newBehaviourState"></param>
        private void HandleBehaviourChange(uint newBehaviourState)
        {
            Debug.Assert(ActiveState != null);

            // If we have not changed state then just return.
            if (newBehaviourState == ActiveState.StateID) { return; }

            // Check the transitions of the current active state
            if (ActiveState.CheckTransitions(newBehaviourState))
            {
                SetNewActiveState(newBehaviourState);
            }

            // Check the global states
            foreach (State state in GlobalStates)
            {
                if (state != ActiveState && state.StateID == newBehaviourState)
                {
                    SetNewActiveState(newBehaviourState);

                    break;
                }
            }

            // The new state we have moved to should not be playing already
            Debug.Assert(ActiveState.Animation.IsPlaying == false);

            ActiveState.Animation.IsPlaying = true;
        }

        /// <summary>
        /// A helper function which performs clean up and checks when transitioning to a new state.
        /// </summary>
        /// <param name="newBehaviourState"></param>
        private void SetNewActiveState(uint newBehaviourState)
        {
            // Check to make sure we have transitioned to a state that exists in our state machine.
            Debug.Assert(States[newBehaviourState] != null);

            // Reset the old states's animation
            ActiveState.Animation.Reset();

            // Set the new state
            ActiveState = States[newBehaviourState];
        }

        #endregion
    }
}
