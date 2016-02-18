using Microsoft.Xna.Framework;

namespace _2DEngine
{
    /// <summary>
    /// A class which deals with drawing lights to the scene.
    /// Is marked abstact because a specific type of light should be implemented - Point, Directional, Ambient
    /// </summary>
    public abstract class Light : BaseObject
    {
        #region Properties and Fields

        /// <summary>
        /// A float to indicate how long we should 
        /// </summary>
        private float LifeTime { get; set; }

        private float currentLifeTimer = 0;

        #endregion

        public Light(Vector2 localPosition, Color colour, string lightTextureAsset, float lifeTime = float.MaxValue) :
            this(Vector2.Zero, localPosition, colour, lightTextureAsset)
        {
        }

        public Light(Vector2 size, Vector2 localPosition, Color colour, string lightTextureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, lightTextureAsset)
        {
            Colour = colour;
            LifeTime = lifeTime;
        }

        #region Virtual Methods

        /// <summary>
        /// Updates this light's life time
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            currentLifeTimer += elapsedGameTime;

            if (currentLifeTimer > LifeTime)
            {
                Die();
            }
        }

        #endregion
    }
}
