using _2DEngineData;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace _2DEngine
{
    public class GameObject : BaseObject
    {
        #region Properties and Fields

        /// <summary>
        /// A string used to store the data asset for this game object.
        /// </summary>
        protected string DataAsset { get; private set; }

        /// <summary>
        /// The data associated with this game object.  Not all game objects will have data, but most should.
        /// If not loaded when needed, then is loaded automatically.
        /// </summary>
        private GameObjectData data;
        protected GameObjectData Data
        {
            get
            {
                if (data == null)
                {
                    data = LoadGameObjectData();
                }

                return data;
            }
        }

        /// <summary>
        /// A potentially null physics body used to apply physics to this GameObject
        /// </summary>
        public PhysicsBody PhysicsBody { get; private set; }

        /// <summary>
        /// The health of this object.  If below zero, it will be killed and cleaned up.
        /// </summary>
        protected float Health { get; private set; }

        #endregion

        public GameObject(Vector2 localPosition, string dataAsset) :
            base(localPosition, "")
        {
            Health = 100;
            DataAsset = dataAsset;
        }

        #region Virtual Functions

        /// <summary>
        /// A function for specifying a custom data class to load for this game object.
        /// Override completely to specify a new data type in the AssetManager.GetData function.
        /// </summary>
        /// <returns></returns>
        protected virtual GameObjectData LoadGameObjectData()
        {
            if (Data != null) { return Data; }

            return AssetManager.GetData<GameObjectData>(DataAsset);
        }

        public override void LoadContent()
        {
            // Check to see if we should load
            CheckShouldLoad();

            // Load the data here if we have a non-empty data asset.
            if (!string.IsNullOrEmpty(DataAsset))
            {
                DebugUtils.AssertNotNull(Data);

                // Texture asset can be empty - for example, with animations they will handle themselves.
                TextureAsset = Data.TextureAsset;
            }
            else
            {
                // If we have got in here, the data asset was not specified and so our texture asset was manually set.
                // Should check that this is true.
                Debug.Assert(!string.IsNullOrEmpty(TextureAsset));
            }

            // This will handle the loading if not done so already.
            base.LoadContent();
        }

        /// <summary>
        /// Calls Die on the object if it has insufficient health.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (PhysicsBody != null) { PhysicsBody.Update(elapsedGameTime); }

            // Die if we have insufficient health
            if (Health <= 0) { Die(); }
        }

        /// <summary>
        /// A function for altering the health of this object.  
        /// Health cannot be modified in any other, because we may wish to have extra behaviour when we lose health (i.e. changing behaviour/animation state).
        /// </summary>
        /// <param name="damage">The amount to subtract from this object's health.</param>
        public virtual void Damage(float damage)
        {
            Health -= damage;
        }

        #endregion

        #region Utility Functions

        protected void AddPhysicsBody()
        {
            PhysicsBody = new PhysicsBody(this);
        }

        public bool DeathTransition(State sourceState, State destinationState)
        {
            return Health <= 0;
        }

        #endregion
    }
}
