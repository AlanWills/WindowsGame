using _2DEngine;
using _2DEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsGame
{
    public enum AnimatedCharacterBehaviours
    {
        kWalk = CharacterBehaviours.kNumBehaviours,

        kNumBehaviours
    }

    /// <summary>
    /// A class for a character that will be fully animated with associated behaviour states.
    /// </summary>
    public class AnimatedCharacter : Character
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

        public AnimatedCharacter(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
        }


        #region Animations

        /// <summary>
        /// Sets up all the states and transitions.
        /// </summary>
        public virtual void SetUpStateMachine()
        {
            AnimatedCharacterData data = Data.As<AnimatedCharacterData>();
            Debug.Assert(data != null);

            // Create a temporary data structure to store all our loaded animations in.
            Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

            foreach (string animationDataAsset in data.AnimationInfo)
            {
                Animation animation = new Animation(data.FolderPath + animationDataAsset + ".xml");
                animations.Add(animationDataAsset, animation);
            }

            Debug.Assert(animations["Idle"] != null);
            State idleState = new State(animations["Idle"]);

            Debug.Assert(animations["Walk"] != null);
            State walkState = new State(animations["Walk"]);

            Debug.Assert(animations["Run"] != null);
            State runState = new State(animations["Run"]);

            Debug.Assert(animations["Jump Start"] != null);
            State jumpStartState = new State(animations["Jump Start"]);

            Debug.Assert(animations["Jump Fall"] != null);
            State jumpFallState = new State(animations["Jump Fall"]);

            Debug.Assert(animations["Idle Shoot"] != null);
            State idleShootState = new State(animations["Idle Shoot"]);

            Debug.Assert(animations["Walk Shoot"] != null);
            State walkAndShootState = new State(animations["Walk Shoot"]);

            Debug.Assert(animations["Run Shoot"] != null);
            State runAndShootState = new State(animations["Run Shoot"]);

            Debug.Assert(animations["Melee"] != null);
            State meleeState = new State(animations["Melee"]);

            Debug.Assert(animations["Forward Roll"] != null);
            State forwardRollState = new State(animations["Forward Roll"]);

            Debug.Assert(animations["Death"] != null);
            State deathState = new State(animations["Death"]);

            idleState.AddTransition(walkState, GameKeyboard.IsWalkKeyDown);
            idleState.AddTransition(runState, GameKeyboard.IsWalkAndRunKeyDown);
            idleState.AddTransition(jumpStartState, GameKeyboard.IsJumpKeyPressed);
            idleState.AddTransition(idleShootState, GameMouse.IsShootButtonClicked);
            idleState.AddTransition(meleeState, GameMouse.IsMeleeButtonClicked);
            idleState.AddTransition(forwardRollState, GameKeyboard.IsRollKeyPressed);

            walkState.AddTransition(runState, GameKeyboard.IsRunKeyDown);
            walkState.AddTransition(idleState, GameKeyboard.IsWalkKeyNotDown);
            walkState.AddTransition(jumpStartState, GameKeyboard.IsJumpKeyPressed);
            walkState.AddTransition(walkAndShootState, GameMouse.IsShootButtonClicked);
            walkState.AddTransition(meleeState, GameMouse.IsMeleeButtonClicked);
            walkState.AddTransition(forwardRollState, GameKeyboard.IsRollKeyPressed);

            runState.AddTransition(idleState, GameKeyboard.IsWalkAndRunKeyNotDown);
            runState.AddTransition(walkState, GameKeyboard.IsRunKeyNotDown);
            runState.AddTransition(jumpStartState, GameKeyboard.IsJumpKeyPressed);
            runState.AddTransition(runAndShootState, GameMouse.IsShootButtonClicked);
            runState.AddTransition(meleeState, GameMouse.IsMeleeButtonClicked);
            runState.AddTransition(forwardRollState, GameKeyboard.IsRollKeyPressed);

            jumpStartState.AddTransition(jumpFallState, Transition.SourceAnimationComplete);

            idleShootState.AddTransition(idleState, Transition.SourceAnimationComplete);

            walkAndShootState.AddTransition(walkState, Transition.SourceAnimationComplete);

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
            StateMachine.States.Add(walkAndShootState);
            StateMachine.States.Add(runAndShootState);
            StateMachine.States.Add(meleeState);
            StateMachine.States.Add(forwardRollState);

            StateMachine.AddGlobalTransition(deathState, DeathTransition);
        }

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Loads the animated character data.
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.GetData<AnimatedCharacterData>(DataAsset);
        }

        /// <summary>
        /// Loads all the states.
        /// </summary>
        public override void LoadContent()
        {
            if (!ShouldLoad) { return; }

            base.LoadContent();

            SetUpStateMachine();
            StateMachine.LoadContent();
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

        /// <summary>
        /// When this object dies, it shouldn't die until the death animation is finished.
        /// </summary>
        public override void Die()
        {
            ShouldHandleInput = false;
        }

        #endregion
    }
}
