using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        /// The health of this object.  If below zero, it will be killed and cleaned up.
        /// </summary>
        protected float Health { get; set; }

        #endregion

        public GameObject(Vector2 localPosition, string dataAsset) :
            base(localPosition, "")
        {
            Health = 100;
            DataAsset = dataAsset;
        }

        #region Virtual Functions

        public override void LoadContent()
        {
            // Check to see if we should load
            if (!ShouldLoad) { return; }

            // Load the data here
            // Set the texture asset - if no data, then set the texture asset to be the data asset

            base.LoadContent();
        }

        /// <summary>
        /// Calls Die on the object if it has insufficient health.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            if (!ShouldUpdate) { return; }

            base.Update(elapsedGameTime);

            // Die if we have insufficient health
            if (Health <= 0) { Die(); }
        }

        #endregion

        #region Utility Functions

        public bool DeathTransition(State sourceState, State destinationState)
        {
            return Health <= 0;
        }

        #endregion
    }
}
