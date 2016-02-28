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
        /// </summary>
        private SpriteFont spriteFont;
        protected SpriteFont SpriteFont
        {
            get
            {
                if (spriteFont == null)
                {
                    spriteFont = AssetManager.GetSpriteFont(AssetManager.DefaultSpriteFontAsset);
                }

                return spriteFont;
            }
            set { spriteFont = value; }
        }

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

        /// <summary>
        /// Updates the lifetime of this UIObject
        /// </summary>
        /// <param name="elapsedGameTime"></param>
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
    }
}
