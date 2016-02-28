using Microsoft.Xna.Framework;

namespace _2DEngine
{
    /// <summary>
    /// A class used in the level design screen.
    /// Basically an Image except it will be deleted when right clicked on
    /// </summary>
    public class LevelDesignObject : Image
    {
        public LevelDesignObject(Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            this(Vector2.Zero, localPosition, textureAsset, lifeTime)
        {

        }

        public LevelDesignObject(Vector2 size, Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, textureAsset, lifeTime)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Checks to see if this object is right clicked, and if so, kills the object.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            DebugUtils.AssertNotNull(Collider);

            if (GameMouse.Instance.IsClicked(MouseButton.kRightButton) && Collider.CheckIntersects(GameMouse.Instance.InGamePosition))
            {
                Die();
            }
        }

        #endregion
    }
}
