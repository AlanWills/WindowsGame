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
        }

        protected override void UpdateBehaviour()
        {
            base.UpdateBehaviour();

            switch (CurrentBehaviour)
            {
                // Add extra cases here

                default:
                    break;
            }
        }

        #endregion

        #region Player Behaviour Changing Functions

        protected override void IdleState()
        {
            base.IdleState();
        }

        #endregion
    }
}
