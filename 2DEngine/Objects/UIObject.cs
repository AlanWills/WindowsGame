using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// This class is a base class for any UI objects in the game.  
    /// It is marked abstract because we do not want to create an instance of this class - it is too generic
    /// </summary>
    public abstract class UIObject : BaseObject
    {
        #region Properties and Fields

        /// <summary>
        /// A float for this class which can be used to kill the object after a certain amount of time has passed.
        /// </summary>
        private float LifeTime { get; set; }

        /// <summary>
        /// The current amount of time this object has been alive
        /// </summary>
        private float CurrentLifeTime { get; set; }

        /// <summary>
        /// An object which can be used to store values.
        /// Useful for buttons etc.
        /// </summary>
        public object StoredObject { get; set; }

        /// <summary>
        /// A class specific SpriteFont which can be used to draw text.
        /// Must be created by this class.
        /// </summary>
        protected SpriteFont SpriteFont { get; set; }

        #endregion

        public UIObject(Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            this(Vector2.Zero, localPosition, textureAsset, lifeTime)
        {
        }

        public UIObject(Vector2 size, Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, textureAsset)
        {
            LifeTime = lifeTime;
            CurrentLifeTime = 0;
        }

        #region Virtual Functions

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            CurrentLifeTime += elapsedGameTime;
            if (CurrentLifeTime > LifeTime)
            {
                // The UIObject has been alive longer than it's lifetime so it dies
                // and will be cleared up by whatever manager is in charge of it
                Die();
            }
        }

        #endregion

        #region Utility Functions

        protected void SetupSpriteFont(string spriteFontAsset = AssetManager.DefaultSpriteFontAsset)
        {
            SpriteFont = AssetManager.GetSpriteFont(spriteFontAsset);
            DebugUtils.AssertNotNull(SpriteFont);
        }

        #endregion
    }
}
