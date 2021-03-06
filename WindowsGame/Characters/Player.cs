﻿using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

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
    public class Player : ArmedCharacter
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
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kMelee, false);
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kForwardRoll, false);
            StateMachine.AddTransition((uint)PlayerBehaviours.kWalk, (uint)PlayerBehaviours.kJumpStart, false);

            // Transitions from kRun
            StateMachine.AddTransition((uint)PlayerBehaviours.kRun, (uint)PlayerBehaviours.kRunShoot);
            StateMachine.AddTransition((uint)PlayerBehaviours.kRun, (uint)PlayerBehaviours.kMelee, false);
            StateMachine.AddTransition((uint)PlayerBehaviours.kRun, (uint)PlayerBehaviours.kForwardRoll, false);
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

        /// <summary>
        /// Update all the player behaviours
        /// </summary>
        protected override void UpdateBehaviour()
        {
            base.UpdateBehaviour();

            switch (CurrentBehaviour)
            {
                case (uint)PlayerBehaviours.kWalk:
                    WalkState();
                    break;

                case (uint)PlayerBehaviours.kRun:
                    RunState();
                    break;

                case (uint)PlayerBehaviours.kIdleShoot:
                    IdleShootState();
                    break;

                case (uint)PlayerBehaviours.kWalkShoot:
                    WalkShootState();
                    break;

                case (uint)PlayerBehaviours.kRunShoot:
                    RunShootState();
                    break;

                case (uint)PlayerBehaviours.kMelee:
                    MeleeState();
                    break;

                case (uint)PlayerBehaviours.kForwardRoll:
                    ForwardRollState();
                    break;

                case (uint)PlayerBehaviours.kJumpStart:
                    JumpStartState();
                    break;

                case (uint)PlayerBehaviours.kJumpFall:
                    JumpFallState();
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Player Behaviour Changing Functions

        /// <summary>
        /// Handle behaviour changes from kIdle
        /// </summary>
        protected override void IdleState()
        {
            base.IdleState();

            DebugUtils.AssertNotNull(PhysicsBody);
            DebugUtils.AssertNotNull(CharacterData);

            bool isMoveLeftDown = GameKeyboard.IsKeyDown(InputMap.MoveLeft);
            bool isMoveRightDown = GameKeyboard.IsKeyDown(InputMap.MoveRight);

            // This should always be checked
            if (isMoveLeftDown || isMoveRightDown)
            {
                PhysicsBody.Direction = isMoveRightDown ? PhysicsConstants.RightDirection : PhysicsConstants.LeftDirection;

                if (GameKeyboard.IsKeyDown(InputMap.Run))
                {
                    CurrentBehaviour = (uint)PlayerBehaviours.kRun;

                    PhysicsBody.LinearVelocity = new Vector2(CharacterData.RunSpeed, PhysicsBody.LinearVelocity.Y);
                }
                else
                {
                    CurrentBehaviour = (uint)PlayerBehaviours.kWalk;

                    PhysicsBody.LinearVelocity = new Vector2(CharacterData.WalkSpeed, PhysicsBody.LinearVelocity.Y);
                }
            }
            else
            {
                PhysicsBody.FullLinearStop(Dimensions.kX);
            }

            // The other states should always be checked no matter the movement input
            if (Weapon.CurrentWeaponState == WeaponState.kFiring && Weapon.PreviousWeaponState == WeaponState.kReady)
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kIdleShoot;

                PhysicsBody.FullLinearStop(Dimensions.kX);
            }
            else
            {
                CheckMeleeRollJumpFall();
            }
        }

        /// <summary>
        /// Handle behaviour changes from kWalk
        /// </summary>
        private void WalkState()
        {
            bool isMoveLeftDown = GameKeyboard.IsKeyDown(InputMap.MoveLeft);
            bool isMoveRightDown = GameKeyboard.IsKeyDown(InputMap.MoveRight);

            // This should always be checked
            if (!isMoveLeftDown && !isMoveRightDown)
            {
                CurrentBehaviour = (uint)CharacterBehaviours.kIdle;

                PhysicsBody.FullLinearStop(Dimensions.kX);
            }
            else if (isMoveLeftDown || isMoveRightDown)
            {
                PhysicsBody.Direction = isMoveRightDown ? PhysicsConstants.RightDirection : PhysicsConstants.LeftDirection;

                if (GameKeyboard.IsKeyDown(InputMap.Run))
                {
                    CurrentBehaviour = (uint)PlayerBehaviours.kRun;

                    PhysicsBody.LinearVelocity = new Vector2(CharacterData.RunSpeed, PhysicsBody.LinearVelocity.Y);
                }
                else
                {
                    // We are already walking so no need to set the CurrentBehaviour, but update the linear velocity
                    PhysicsBody.LinearVelocity = new Vector2(CharacterData.WalkSpeed, PhysicsBody.LinearVelocity.Y);
                }
            }

            // The other states should always be checked no matter the movement input
            if (Weapon.CurrentWeaponState == WeaponState.kFiring && Weapon.PreviousWeaponState == WeaponState.kReady)
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kWalkShoot;
                PhysicsBody.LinearVelocity = new Vector2(CharacterData.WalkSpeed, PhysicsBody.LinearVelocity.Y);
            }
            else 
            {
                CheckMeleeRollJumpFall();
            }
        }

        /// <summary>
        /// Handle behaviour changes from kRun
        /// </summary>
        private void RunState()
        {
            bool isMoveLeftDown = GameKeyboard.IsKeyDown(InputMap.MoveLeft);
            bool isMoveRightDown = GameKeyboard.IsKeyDown(InputMap.MoveRight);

            // This should always be checked
            if (!isMoveLeftDown && !isMoveRightDown)
            {
                CurrentBehaviour = (uint)CharacterBehaviours.kIdle;

                PhysicsBody.FullLinearStop(Dimensions.kX);
            }
            else if (isMoveLeftDown || isMoveRightDown)
            {
                PhysicsBody.Direction = isMoveRightDown ? PhysicsConstants.RightDirection : PhysicsConstants.LeftDirection;

                if (!GameKeyboard.IsKeyDown(InputMap.Run))
                {
                    CurrentBehaviour = (uint)PlayerBehaviours.kWalk;

                    PhysicsBody.LinearVelocity = new Vector2(CharacterData.WalkSpeed, PhysicsBody.LinearVelocity.Y);
                }
                else
                {
                    // We are already running so no need to set the CurrentBehaviour, but update the linear velocity
                    PhysicsBody.LinearVelocity = new Vector2(CharacterData.RunSpeed, PhysicsBody.LinearVelocity.Y);
                }
            }

            // The other states should always be checked no matter the movement input
            if (Weapon.CurrentWeaponState == WeaponState.kFiring && Weapon.PreviousWeaponState == WeaponState.kReady)
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kRunShoot;
                PhysicsBody.LinearVelocity = new Vector2(CharacterData.RunSpeed, PhysicsBody.LinearVelocity.Y);
            }
            else
            {
                CheckMeleeRollJumpFall();
            }
        }

        /// <summary>
        /// Handle behaviour changes from kIdleShoot
        /// </summary>
        private void IdleShootState()
        {
            ToStateWhenFinished((uint)PlayerBehaviours.kIdleShoot, (uint)CharacterBehaviours.kIdle);
        }

        /// <summary>
        /// Handle behaviour changes from kWalkShoot
        /// </summary>
        private void WalkShootState()
        {
            ToStateWhenFinished((uint)PlayerBehaviours.kWalkShoot, (uint)PlayerBehaviours.kWalk);
        }

        /// <summary>
        /// Handle behaviour changes from kRunShoot
        /// </summary>
        private void RunShootState()
        {
            ToStateWhenFinished((uint)PlayerBehaviours.kRunShoot, (uint)PlayerBehaviours.kRun);
        }

        /// <summary>
        /// Handle behaviour changes from kMelee
        /// </summary>
        private void MeleeState()
        {
            ToStateWhenFinished((uint)PlayerBehaviours.kMelee, (uint)CharacterBehaviours.kIdle);
        }

        /// <summary>
        /// Handle behaviour changes from kForwardRoll
        /// </summary>
        private void ForwardRollState()
        {
            ToStateWhenFinished((uint)PlayerBehaviours.kForwardRoll,(uint)CharacterBehaviours.kIdle);
        }

        /// <summary>
        /// Handle behaviour changes from kJumpStart
        /// </summary>
        private void JumpStartState()
        {
            DebugUtils.AssertNotNull(PhysicsBody);

            if (PhysicsBody.LinearVelocity.Y < 0)
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kJumpFall;
            }
        }

        /// <summary>
        /// Handle behaviour changes from kJumpFall
        /// </summary>
        private void JumpFallState()
        {
            DebugUtils.AssertNotNull(PhysicsBody);

            if ((Collider.CollidedThisFrame || Collider.CollidedLastFrame))
            {
                CurrentBehaviour = (uint)CharacterBehaviours.kIdle;
            }
        }

        /// <summary>
        /// A utility function to check melee, roll, jump and fall which are the same for most states
        /// </summary>
        private void CheckMeleeRollJumpFall()
        {
            // Only melee if we are not reloading or firing
            if (GameMouse.Instance.IsClicked(InputMap.Melee) && Weapon.CurrentWeaponState == WeaponState.kReady)
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kMelee;

                PhysicsBody.FullLinearStop(Dimensions.kX);
            }
            else if (GameKeyboard.IsKeyPressed(InputMap.ForwardRoll))
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kForwardRoll;
                PhysicsBody.LinearVelocity = new Vector2(CharacterData.WalkSpeed, PhysicsBody.LinearVelocity.Y);
            }
            else if (GameKeyboard.IsKeyPressed(InputMap.Jump))
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kJumpStart;
                PhysicsBody.LinearVelocity = new Vector2(PhysicsBody.LinearVelocity.X, CharacterData.JumpHeight);
            }
            else if (PhysicsBody.LinearVelocity.Y < -PhysicsConstants.Gravity)
            {
                CurrentBehaviour = (uint)PlayerBehaviours.kJumpFall;
            }
        }

        /// <summary>
        /// A simple function to wrap up checking if the current animation is finished and going to the destination state if so.
        /// </summary>
        /// <param name="currentBehaviour">The current behaviour - used for debug checking.</param>
        /// <param name="destinationStateID">The destination behaviour to transition to when the animation is finished.</param>
        private void ToStateWhenFinished(uint currentBehaviour, uint destinationStateID)
        {
            StateMachine.CheckActiveStateHasID(currentBehaviour);
            if (StateMachine.CurrentAnimationFinished())
            {
                CurrentBehaviour = destinationStateID;
            }
        }

        #endregion
    }
}
