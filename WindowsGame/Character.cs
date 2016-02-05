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
            State walkingState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Walk_000_1x16_Resized", 1, 16, false));
            State runningState = new State(new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Run_000_1x14_Resized", 1, 14));

            idleState.Transitions.Add(new Transition(idleState, walkingState, Transition.IsMovementKeyDown));

            walkingState.Transitions.Add(new Transition(walkingState, runningState, Transition.AnimationComplete));
            walkingState.Transitions.Add(new Transition(walkingState, idleState, Transition.IsMovementKeyNotDown));

            runningState.Transitions.Add(new Transition(runningState, idleState, Transition.IsMovementKeyNotDown));

            StateMachine = new StateMachine(this, idleState);
            StateMachine.States.Add(idleState);
            StateMachine.States.Add(walkingState);
            StateMachine.States.Add(runningState);
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
            base.Update(elapsedGameTime);

            if (!ShouldUpdate) { return; }

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

        #endregion
    }
}
