using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace WindowsGame
{
    public class Character : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// The state machine for this character.
        /// </summary>
        protected StateMachine StateMachine { get; set; }

        /// <summary>
        /// Get the current texture based on the state machine active state.
        /// </summary>
        protected override Texture2D Texture
        {
            get
            {
                return StateMachine.ActiveState.Animation.Texture;
            }
        }

        /// <summary>
        /// Get the current texture centre based on the state machine active state.
        /// </summary>
        protected override Vector2 TextureCentre
        {
            get
            {
                return StateMachine.ActiveState.Animation.Centre;
            }
        }

        #endregion

        public Character(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            
        }

        #region Animations

        /// <summary>
        /// Sets up all the states and transitions.
        /// </summary>
        public virtual void SetUpStateMachine()
        {
            State idleState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Idle_000_1x12_Resized", 1, 12));
            State walkState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Walk_000_1x16_Resized", 1, 16, false));
            State runState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Run_000_1x14_Resized", 1, 14));
            State jumpStartState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Jump Start_000_1x10_Resized", 1, 10, false));
            State jumpFallState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Jump Fall_000_1x1_Resized", 1, 1));
            State idleShootState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Shoot_000_1x16_Resized", 1, 16, false));
            State runAndShootState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Run_Shoot_000_1x15_Resized", 1, 15, false));
            State meleeState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Knockback_000_1x16_Resized", 1, 16, false));
            State forwardRollState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Forward Roll_000_1x13_Resized", 1, 13, false));

            State deathState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Death_000_1x15_Resized", 1, 15, false));

            idleState.AddTransition(walkState, GameKeyboard.IsMovementKeyDown);
            idleState.AddTransition(jumpStartState, GameKeyboard.IsJumpKeyPressed);
            idleState.AddTransition(idleShootState, GameMouse.IsShootButtonClicked);
            idleState.AddTransition(meleeState, GameMouse.IsMeleeButtonClicked);
            idleState.AddTransition(forwardRollState, GameKeyboard.IsRollKeyPressed);

            walkState.AddTransition(runState, Transition.SourceAnimationComplete);
            walkState.AddTransition(idleState, GameKeyboard.IsMovementKeyNotDown);
            walkState.AddTransition(meleeState, GameMouse.IsMeleeButtonClicked);
            walkState.AddTransition(forwardRollState, GameKeyboard.IsRollKeyPressed);

            runState.AddTransition(idleState, GameKeyboard.IsMovementKeyNotDown);
            runState.AddTransition(runAndShootState, GameMouse.IsShootButtonClicked);
            runState.AddTransition(meleeState, GameMouse.IsMeleeButtonClicked);
            runState.AddTransition(forwardRollState, GameKeyboard.IsRollKeyPressed);

            jumpStartState.AddTransition(jumpFallState, Transition.SourceAnimationComplete);

            idleShootState.AddTransition(idleState, Transition.SourceAnimationComplete);

            runAndShootState.AddTransition(runState, Transition.SourceAnimationComplete);

            meleeState.AddTransition(idleState, Transition.SourceAnimationComplete);

            forwardRollState.AddTransition(idleState, Transition.SourceAnimationComplete);

            deathState.Animation.OnAnimationComplete += base.Die;

            StateMachine = new StateMachine(this, idleState);
            StateMachine.States.Add(idleState);
            StateMachine.States.Add(walkState);
            StateMachine.States.Add(runState);
            StateMachine.States.Add(jumpStartState);
            StateMachine.States.Add(jumpFallState);
            StateMachine.States.Add(idleShootState);
            StateMachine.States.Add(runAndShootState);
            StateMachine.States.Add(meleeState);
            StateMachine.States.Add(forwardRollState);

            StateMachine.AddGlobalTransition(deathState, DeathTransition);
        }

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Loads all the states.
        /// </summary>
        public override void LoadContent()
        {
            if (!ShouldLoad) { return; }

            SetUpStateMachine();

            StateMachine.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Updates the SpriteEffect of the character to face left or right based on key pressed.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            if (!ShouldHandleInput) { return; }

            base.HandleInput(elapsedGameTime, mousePosition);

            if (GameKeyboard.IsKeyDown(InputMap.MoveLeft))
            {
                SpriteEffect = SpriteEffects.FlipHorizontally;
            }

            if (GameKeyboard.IsKeyDown(InputMap.MoveRight))
            {
                SpriteEffect = SpriteEffects.None;
            }
        }

        /// <summary>
        /// Updates the state machine.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            if (!ShouldUpdate) { return; }

            base.Update(elapsedGameTime);

            StateMachine.Update(elapsedGameTime);
        }

        /// <summary>
        /// Sets the source rectangle from the state machine's active state.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!ShouldDraw) { return; }

            SourceRectangle = StateMachine.ActiveState.Animation.CurrentSourceRectangle;

            base.Draw(spriteBatch);
        }

        public override void Die()
        {
            ShouldHandleInput = false;
        }

        #endregion
    }
}
