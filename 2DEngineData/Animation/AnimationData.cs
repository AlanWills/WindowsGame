using Microsoft.Xna.Framework;

namespace _2DEngineData
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
        /// The number of frames in X.
        /// </summary>
        public int TextureFramesX { get; set; }

        /// <summary>
        /// The number of frames in Y.
        /// </summary>
        public int TextureFramesY { get; set; }

        /// <summary>
        /// A flag to indicate whether the animation should repeat or not.
        /// </summary>
        public bool Continual { get; set; }
    }
}