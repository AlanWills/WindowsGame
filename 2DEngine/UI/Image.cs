using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class for scaling textures but preserving their aspect ratio.
    /// With a UIObject, inputting a size scalesthe image exactly, destroying the aspect ratio.
    /// This class takes the inputted size, sets the largest dimension of the texture to that size
    /// and sets the other dimension to the appopriate size to preserve the aspect ratio of the texture.
    /// </summary>
    public class Image : UIObject
    {
        /// <summary>
        /// A float that can be used to represent how much we should scale the texture, rather than manually inputting a size.
        /// Doesn't have to be set, in which case it defaults to 1.
        /// </summary>
        private float Scale { get; set; }

        public Image(Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(localPosition, textureAsset, lifeTime)
        {
            Scale = 1;
        }

        public Image(float scale, Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(Vector2.Zero, localPosition, textureAsset, lifeTime)
        {
            Scale = scale;
        }

        public Image(Vector2 size, Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, textureAsset, lifeTime)
        {
            Scale = 1;
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up the size to preserve the texture aspect ratio
        /// </summary>
        public override void Initialise()
        {
            // Check to see whether we should Initialise
            CheckShouldInitialise();

            // Texture cannot be null
            Debug.Assert(Texture != null);

            float aspectRatio = (float)Texture.Bounds.Height / (float)Texture.Bounds.Width;
            Debug.Assert(aspectRatio > 0);

            if (aspectRatio < 1)
            {
                Size = new Vector2(Size.X, Size.X * aspectRatio);
            }
            else
            {
                Size = new Vector2(Size.Y / aspectRatio, Size.Y);
            }

            base.Initialise();

            // Multiply by the scale after the base.Initialise call.  
            // This is because, we still may have Size = Vector2.Zero before the call, but afterwards it will not be
            Size *= Scale;
        }

        #endregion
    }
}
