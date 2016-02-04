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
        private float Health { get; set; }

        #endregion

        public GameObject(Vector2 localPosition, string dataAsset) :
            base(localPosition, "")
        {
            Health = 1;
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

        #endregion
    }
}
