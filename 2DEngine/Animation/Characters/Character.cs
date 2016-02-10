using _2DEngineData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace _2DEngine
{
    public enum CharacterBehaviours
    {
        kIdle,
        kDeath,

        kNumBehaviours
    }

    public delegate void BehaviourChangeHandler(uint newState);

    public class Character : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// An event handler used with the StateMachine.
        /// Call EmitBehaviourChanged() when a character changes behaviour to update the State Machine for that character.
        /// </summary>
        public event BehaviourChangeHandler BehaviourChanged;

        /// <summary>
        /// A property to indicate the current behaviour that this character is in.
        /// Handles behaviour changing event after the value is set.
        /// </summary>
        private uint currentBehaviour;
        protected uint CurrentBehaviour
        {
            get { return currentBehaviour; }
            set
            {
                currentBehaviour = value;

                // Behaviour Changed should not be null.
                // If it is, it means we have not set up our State Machine to hook into this event.
                Debug.Assert(BehaviourChanged != null);
                BehaviourChanged(currentBehaviour);
            }
        }

        /// <summary>
        /// The animations that this character has access to.  Defined in it's xml CharacterData.
        /// </summary>
        protected Dictionary<string, Animation> Animations { get; private set; }

        /// <summary>
        /// This should be set from the Behaviour Enum from any derived class to be the total number of behaviours declared.
        /// Therefore, if we have declared two more in a derived class, this should be set to 3 to count the behaviour we declare in this class.
        /// </summary>
        protected uint NumBehaviours { private get; set; }

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
                Debug.Assert(StateMachine != null);
                Debug.Assert(StateMachine.ActiveState != null);
                Debug.Assert(StateMachine.ActiveState.Animation != null);

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
                Debug.Assert(StateMachine != null);
                Debug.Assert(StateMachine.ActiveState != null);
                Debug.Assert(StateMachine.ActiveState.Animation != null);

                return StateMachine.ActiveState.Animation.Centre;
            }
        }

        #endregion

        public Character(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            Animations = new Dictionary<string, Animation>();
            NumBehaviours = (uint)CharacterBehaviours.kNumBehaviours;
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up all the states and transitions.
        /// </summary>
        protected virtual void SetUpAnimations()
        {
            Debug.Assert(Data != null);
            CharacterData data = Data.As<CharacterData>();
            Debug.Assert(data != null);

            // Checks that we have declared at most the same number of enum behaviours as we have animations.
            // If NumAnimations is larger than data.AnimationInfo.Count, it means we have not loaded enough animations for all our behaviours.
            Debug.Assert(data.AnimationInfo.Count >= NumBehaviours);

            StateMachine = new StateMachine(this, NumBehaviours);

            foreach (string animationDataAsset in data.AnimationInfo)
            {
                Animation animation = new Animation(data.FolderPath + animationDataAsset + ".xml");

                Debug.Assert(!Animations.ContainsKey(animationDataAsset));
                Animations.Add(animationDataAsset, animation);
            }

            CreateState("Idle", (uint)CharacterBehaviours.kIdle);
            CreateState("Death", (uint)CharacterBehaviours.kDeath);

            StateMachine.StartingState = (uint)CharacterBehaviours.kIdle;
        }

        /// <summary>
        /// Loads the character data.
        /// </summary>
        /// <returns></returns>
        protected override GameObjectData LoadGameObjectData()
        {
            return AssetManager.GetData<CharacterData>(DataAsset);
        }

        /// <summary>
        /// Loads all the states.
        /// </summary>
        public override void LoadContent()
        {
            if (!ShouldLoad) { return; }

            // Do not need to rely on loading character data - this will happen when needed.
            SetUpAnimations();
            StateMachine.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Starts the state machine playing the ActiveState animation
        /// </summary>
        public override void Initialise()
        {
            if (!ShouldInitialise) { return; }

            StateMachine.ActiveState.Animation.IsPlaying = true;

            base.Initialise();
        }

        /// <summary>
        /// Checks the current behaviour to see if it can move to a new behaviour.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            // Check to see if we should handle input
            if (!ShouldHandleInput) { return; }

            base.HandleInput(elapsedGameTime, mousePosition);


        }

        /// <summary>
        /// Runs through all the behaviours in our Behaviour enum and checks their appropriate function for whether the behaviour state can change.
        /// Can be overridden to check inherited class' new behaviours.
        /// </summary>
        protected virtual void UpdateBehaviour()
        {
            switch (CurrentBehaviour)
            {
                case (uint)CharacterBehaviours.kIdle:
                    IdleState();
                    break;

                case (uint)CharacterBehaviours.kDeath:
                    DeathState();
                    break;

                default:
                    break;
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

        #endregion

        #region Utility Functions

        /// <summary>
        /// A utility function which performs some validity checks before it creates a state for our state machine.
        /// It then adds it to the state machine.  This must be called in the same order as the enum IDs are declared.
        /// </summary>
        /// <param name="name">The name of the asset in our Animations dictionary - will be the name of the XML, e.g. "Walk".</param>
        /// <param name="id">The desired ID for this state.</param>
        /// <returns>Returns the state we have just created.</returns>
        protected State CreateState(string name, uint id)
        {
            Debug.Assert(Animations[name] != null);

            State state = new State(id, Animations[name], Animations[name].IsGlobal);
            StateMachine.AddState(state);

            return state;
        }

        /// <summary>
        /// Obtains a state from the state machine.  Checks for valid ID and null state.
        /// </summary>
        /// <param name="id">The ID of the state we wish to obtain.</param>
        /// <returns>The state we requested from our state machine.</returns>
        public State GetState(uint id)
        {
            Debug.Assert(id < NumBehaviours);
            return StateMachine.GetState(id);
        }

        #endregion

        #region Character Behaviour Changing Functions

        /// <summary>
        /// The function that will be called in the kIdle state to see if we can transition to a new behaviour state.
        /// </summary>
        protected virtual void IdleState()
        {
            // Check health -> go to death if insufficient health
            // Dont' check is alive as this is still true so that we can play the death animation.
        }

        /// <summary>
        /// The function that will be called in the kDeath state to see if we can transition to a new behaviour state.
        /// </summary>
        protected virtual void DeathState()
        {
            // Probably should be empty
            // Maybe add a check to see if the animation is complete to then actually kill the object?
        }

        #endregion
    }
}