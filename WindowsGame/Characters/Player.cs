using _2DEngine;
using Microsoft.Xna.Framework;

namespace WindowsGame
{
    /// <summary>
    /// An enum for all the extra behaviours our player character has on top of a plain character.
    /// </summary>
    public enum PlayerBehaviours
    { 
        kWalk = CharacterBehaviours.kNumBehaviours,
        kRun,
        kIdleShoot,
        kWalkShoot,
        kRunShoot,
        kMelee,
        kForwardRoll,
        kJumpStart,
        kJumpFall,

        kNumBehaviours
    }

    /// <summary>
    /// A class representing the player's character.
    /// Set's up the animations for our player and handles input for movement etc.
    /// </summary>
    public class Player : Character
    {
        public Player(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            NumBehaviours = (uint)PlayerBehaviours.kNumBehaviours;
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up all the animations for our player's character.
        /// Behaviours that have no transitions have been handled by an earlier reciprocal transition.
        /// </summary>
        protected override void SetUpAnimations()
        {
            base.SetUpAnimations();

            CreateState("Walk", (uint)PlayerBehaviours.kWalk);
            CreateState("Run", (uint)PlayerBehaviours.kRun);
            CreateState("Idle Shoot", (uint)PlayerBehaviours.kIdleShoot);
            CreateState("Walk Shoot", (uint)PlayerBehaviours.kWalkShoot);
            CreateState("Run Shoot", (uint)PlayerBehaviours.kRunShoot);
            CreateState("Melee", (uint)PlayerBehaviours.kMelee);
            CreateState("Forward Roll", (uint)PlayerBehaviours.kForwardRoll);
            CreateState("Jump Start", (uint)PlayerBehaviours.kJumpStart);
            CreateState("Jump Fall", (uint)PlayerBehaviours.kJumpFall);

            // Transitions from kIdle
            StateMachine.AddTransition((uint)CharacterBehaviours.kIdle, (uint)PlayerBehaviours.kWalk);
            StateMachine.AddTransition((uint)CharacterBehaviours.kIdle, (uint)PlayerBehaviours.kRun);
            StateMachine.AddTransition((uint)CharacterBehaviours.kIdle, (uint)PlayerBehaviours.kIdleShoot);
            StateMachine.AddTransition((uint)CharacterBehaviours.kIdle, (uint)PlayerBehaviours.kMelee);
            StateMachine.AddTransition((uint)CharacterBehaviours.kIdle, (uint)PlayerBehaviours.kForwardRoll);
            StateMachine.AddTransition((uint)CharacterBehaviours.kIdle, (uint)PlayerBehaviours.kJumpStart, false);

            // Transitions from kWalk
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kRun);
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kWalkShoot);
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kMelee);
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kForwardRoll);
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kJumpStart, false);

            // Transitions from kRun
            StateMachine.AddTransition((uint)PlayerBehaviours.kRun, (uint)PlayerBehaviours.kRunShoot);
            StateMachine.AddTransition((uint)PlayerBehaviours.kRun, (uint)PlayerBehaviours.kMelee);
            StateMachine.AddTransition((uint)PlayerBehaviours.kRun, (uint)PlayerBehaviours.kForwardRoll);
            StateMachine.AddTransition((uint)PlayerBehaviours.kRun, (uint)PlayerBehaviours.kJumpStart, false);

            // Transitions from kIdleShoot
            // None

            // Transitions from kWalkShoot
            // None

            // Transitions from kRunShoot
            // None

            // Transitions from kMelee
            // None

            // Transitions from kForwardRoll
            // None

            // Transitions from kJumpStart
            StateMachine.AddTransition((uint)PlayerBehaviours.kJumpStart, (uint)PlayerBehaviours.kJumpFall, false);

            // Transitions from kJumpFall
            StateMachine.AddTransition((uint)PlayerBehaviours.kJumpFall, (uint)CharacterBehaviours.kIdle, false);

            /*

            kMelee,
        kForwardRoll,
        kJumpStart,
        kJumpFall
            jumpStartState.AddTransition(jumpFallState, Transition.SourceAnimationComplete);

            idleShootState.AddTransition(idleState, Transition.SourceAnimationComplete);

            walkAndShootState.AddTransition(walkState, Transition.SourceAnimationComplete);

            runAndShootState.AddTransition(runState, Transition.SourceAnimationComplete);

            meleeState.AddTransition(idleState, Transition.SourceAnimationComplete);

            forwardRollState.AddTransition(idleState, Transition.SourceAnimationComplete);

            deathState.Animation.OnAnimationComplete += base.Die;

            StateMachine.States.Add(idleState);
            StateMachine.States.Add(walkState);
            StateMachine.States.Add(runState);
            StateMachine.States.Add(jumpStartState);
            StateMachine.States.Add(jumpFallState);
            StateMachine.States.Add(idleShootState);
            StateMachine.States.Add(walkAndShootState);
            StateMachine.States.Add(runAndShootState);
            StateMachine.States.Add(meleeState);
            StateMachine.States.Add(forwardRollState);

            StateMachine.AddGlobalTransition(deathState, DeathTransition);*/

        }

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            // Check to see if we should handle input
            if (!ShouldHandleInput) { return; }

            base.HandleInput(elapsedGameTime, mousePosition);

            // Switch on behaviour
            // For each behaviour, write a function which tests the conditions which would move it to another behaviour
            switch (CurrentBehaviour)
            {
                default:
                    break;
            }
        }

        #endregion
    }
}
