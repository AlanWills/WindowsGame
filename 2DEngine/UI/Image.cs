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
        public Image(Vector2 size, Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, textureAsset, lifeTime)
        {
        }

        #region Virtual Functions

        /// <summary>
        /// Sets up the size to preserve the texture aspect ratio
        /// </summary>
        public override void Initialise()
        {
            // Check to see whether we should Initialise
            if (!ShouldInitialise) { return; }

            // The size must be set to a non zero value and the texture not null
            Debug.Assert(Size != Vector2.Zero);
            Debug.Assert(Texture != null);

            float aspectRatio = Texture.Bounds.Height / Texture.Bounds.Width;
            Debug.Assert(aspectRatio > 0);

            Size = new Vector2(Size.X, Size.X * aspectRatio);

            base.Initialise();
        }

        #endregion
    }
}
