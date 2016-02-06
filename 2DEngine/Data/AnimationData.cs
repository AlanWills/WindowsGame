using Microsoft.Xna.Framework;

namespace _2DEngine
{
    /// <summary>
    /// A class to hold all the information necessary for an animation.
    /// </summary>
    public class AnimationData : BaseData
    {
        /// <summary>
        /// The path for the animation texture.
        /// </summary>
        public string AnimationTextureAsset { get; set; }

        /// <summary>
        /// The number of frames in X and Y.
        /// </summary>
        public Point TextureFrames { get; set; }

        /// <summary>
        /// A flag to indicate whether the animation should repeat or not.
        /// </summary>
        public bool Continual { get; set; }
    }
}