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
        public virtual void SetUpAnimations()
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

        #endregion
    }
}
