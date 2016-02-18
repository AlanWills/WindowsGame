using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// The base class for any UI or game objects in our game.
    /// Marked as abstract, because we should not be able to create an instance of this class.
    /// Instead we should create a UIObject or GameObject
    /// </summary>
    public abstract class BaseObject : Component
    {
        #region Properties and Fields

        /// <summary>
        /// A string to store the texture asset for this object
        /// </summary>
        public string TextureAsset { get; protected set; }

        /// <summary>
        /// The texture for this object - override if we have different textures that need to be drawn at different times
        /// </summary>
        private Texture2D texture;
        protected virtual Texture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    Debug.Assert(!string.IsNullOrEmpty(TextureAsset));
                    texture = AssetManager.GetSprite(TextureAsset);
                }

                return texture;
            }
            set { texture = value; }
        }

        /// <summary>
        /// This is a cached vector that will only be set once.  Used in the draw method to indicate the dimensions of the texture.
        /// Will be set when the texture is loaded ONLY.
        /// </summary>
        private Vector2 TextureDimensions { get; set; }

        /// <summary>
        /// This is a cached vector that will only be set once.  Used in the draw method to indicate the centre of the texture.
        /// Will be set when the texture is loaded ONLY.
        /// </summary>
        protected virtual Vector2 TextureCentre { get; private set; }

        /// <summary>
        /// A source rectangle used to specify a sub section of the Texture2D to draw.
        /// Useful for animations and bars and by default set to (0, 0, texture width, texture height).
        /// </summary>
        public Rectangle SourceRectangle { get; set; }

        /// <summary>
        /// An object which we can parent this object off of.  Positions and rotations are then relative to this object
        /// </summary>
        public BaseObject Parent { get; set; }

        /// <summary>
        /// The local offset from the parent.
        /// Cannot do LocalPosition.X = x, because Vector2 is a struct.
        /// Instead, do LocalPosition = new Vector2(x, LocalPosition.Y).
        /// </summary>
        public Vector2 LocalPosition { get; set; }

        /// <summary>
        /// The local rotation from the parent's rotation - this value is bound between -PI and PI
        /// </summary>
        private float localRotation;
        public float LocalRotation
        {
            get { return localRotation; }
            set
            {
                // Wrap the angle between -PI and PI
                localRotation = MathHelper.WrapAngle(value);
            }
        }

        /// <summary>
        /// The world space position of the object
        /// </summary>
        public Vector2 WorldPosition
        {
            get
            {
                if (Parent == null)
                {
                    return LocalPosition;
                }

                // This syntax is for optimisation
                return Vector2.Add(Parent.WorldPosition, Vector2.Transform(LocalPosition, Matrix.CreateRotationZ(WorldRotation)));
            }
        }

        /// <summary>
        /// The world space rotation, calculated recursively using the parent's WorldRotation.
        /// This value will be between -PI and PI
        /// </summary>
        public float WorldRotation
        {
            get
            {
                // If we have no parent, return the local rotation
                if (Parent == null)
                {
                    return LocalRotation;
                }

                // Wrap the angle between -PI and PI
                return MathHelper.WrapAngle(Parent.WorldRotation + localRotation);
            }
        }

        /// <summary>
        /// The size of this object.  By default this will be the size of the Texture.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// The colour of the object - by default this is set to white, so that white in the png will appear transparent
        /// </summary>
        public Color Colour { get; set; }

        /// <summary>
        /// The opacity of the object - between 0 and 1.  A value of 0 makes the texture completely transparent, and 1 completely opaque
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// A property that can be used to reverse an image - useful for animations or sprites that are facing just one way.
        /// By default, this is SpriteEffects.None.
        /// </summary>
        protected SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// A bool to indicate whether we should add a collider during initialisation.
        /// Some objects (like text) do not need a collider - this is an optimisation step.
        /// </summary>
        public bool UsesCollider { get; set; }

        /// <summary>
        /// The collider associated with this object.  Also, is responsible for mouse interactions.
        /// </summary>
        public Collider Collider { get; private set; }

        #endregion

        public BaseObject(Vector2 localPosition, string textureAsset) :
            base()
        {
            LocalPosition = localPosition;
            TextureAsset = textureAsset;
            Colour = Color.White;
            Opacity = 1;
            UsesCollider = true;
            SpriteEffect = SpriteEffects.None;
        }

        public BaseObject(Vector2 size, Vector2 localPosition, string textureAsset) :
            this(localPosition, textureAsset)
        {
            Size = size;
        }

        #region Virtual Functions

        /// <summary>
        /// Check that the texture has been loaded by doing a get call
        /// </summary>
        public override void LoadContent()
        {
            // Check to see whether we should load
            CheckShouldLoad();

            DebugUtils.AssertNotNull(Texture);

            base.LoadContent();
        }

        /// <summary>
        /// Set up the size if it has not been set already.
        /// Adds the collider if it should.
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            if (Texture != null)
            {
                TextureDimensions = new Vector2(Texture.Bounds.Width, Texture.Bounds.Height);
                TextureCentre = new Vector2(Texture.Bounds.Center.X, Texture.Bounds.Center.Y);

                // Set the source rectangle to the default size of the texture
                SourceRectangle = new Rectangle(
                     0, 0,
                     (int)TextureDimensions.X,
                     (int)TextureDimensions.Y);
            }

            // If our size is zero (i.e. uninitialised) we use the texture's size (if it is not null)
            if (Size == Vector2.Zero && Texture != null)
            {
                Size = new Vector2(Texture.Bounds.Width, Texture.Bounds.Height);
            }

            // Adds the collider if the flag is true
            if (UsesCollider) { AddCollider(); }

            base.Initialise();
        }

        /// <summary>
        /// By default adds a RectangleCollider for this object if it's bool UsesCollider is set to true.
        /// Can be overridden to add custom colliders instead.
        /// </summary>
        protected virtual void AddCollider()
        {
            Collider = new RectangleCollider(this);
        }

        /// <summary>
        /// A function which updates the collider per frame.
        /// Can be overridden to provide custom behaviour - i.e. for objects which use an animation.
        /// </summary>
        /// <param name="position">The position we wish the collider to be centred at</param>
        /// <param name="size">The dimensions of the collider</param>
        public virtual void UpdateCollider(ref Vector2 position, ref Vector2 size)
        {
            position = WorldPosition;
            size = Size;
        }

        /// <summary>
        /// Update the collider's mouse state variables
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (UsesCollider)
            {
                DebugUtils.AssertNotNull(Collider);
                Collider.HandleInput(mousePosition);
            }
        }

        /// <summary>
        /// Updates the object's collider if it has one
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (UsesCollider)
            {
                // Update the collider position and state variables
                DebugUtils.AssertNotNull(Collider);
                Collider.Update();
            }
        }

        /// <summary>
        /// Draws the object's texture.
        /// If we wish to create an object, but not draw it, change it's ShouldDraw property
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // If we are drawing this object, it should have a valid texture
            // If we wish to create an object but not draw it, simply change it's ShouldDraw property
            DebugUtils.AssertNotNull(Texture);
            spriteBatch.Draw(
                Texture,
                WorldPosition,
                null,
                SourceRectangle,
                TextureCentre,
                WorldRotation,
                Vector2.Divide(Size, TextureDimensions),
                Colour * Opacity,
                SpriteEffect,
                0);
        }

        #endregion
    }
}
