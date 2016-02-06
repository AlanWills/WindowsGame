using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A delegate used for events after animations have completed.
    /// </summary>
    public delegate void OnAnimationCompleteHandler();

    public class Animation
    {
        #region Animation Properties

        /// <summary>
        /// The string path for the data asset
        /// </summary>
        private string DataAsset { get; set; }

        /// <summary>
        /// The texture associated with this animation
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// The pixel dimensions of one frame of the animation
        /// </summary>
        private Point FrameDimensions { get; set; }

        /// <summary>
        /// The centre we should pass to the object that is using this animation.
        /// Set only once at loading.
        /// </summary>
        public Vector2 Centre { get; private set; }

        /// <summary>
        /// The number of frames in the sprite sheet
        /// </summary>
        private Point Frames { get; set; }

        /// <summary>
        /// The current frame of the animation
        /// </summary>
        private int CurrentFrame { get; set; }

        /// <summary>
        /// The time each frame is displayed before moving on
        /// </summary>
        public float TimePerFrame { get; set; }

        /// <summary>
        /// A flag to indicate whether the animation is playing
        /// </summary>
        public bool IsPlaying { get; set; }

        /// <summary>
        /// A flag to indicate whether this animation should keep playing or just play once
        /// </summary>
        public bool Continual { get; set; }

        /// <summary>
        /// A flag to indicate whether the animation is completed
        /// </summary>
        private bool Finished { get; set; }

        /// <summary>
        /// The source rectangle to use in the spritebatch Draw function calculated for this animation using the current frame and frame dimensions
        /// </summary>
        public Rectangle CurrentSourceRectangle { get; private set; }

        /// <summary>
        /// Can only be used by non continual animations.
        /// Used to perform a function after the animation has completed.
        /// </summary>
        public event OnAnimationCompleteHandler OnAnimationComplete;

        private float currentTimeOnFrame = 0;
        private const float defaultTimePerFrame = 0.05f;

        #endregion

        public Animation(string dataAsset)
        {
            DataAsset = dataAsset;
            IsPlaying = false;
            CurrentFrame = 0;
        }

        #region LoadContent and Update functions
        
        /// <summary>
        /// Loads the texture and finalises all the information the animation needs
        /// </summary>
        public void LoadContent()
        {
            AnimationData data = AssetManager.GetData<AnimationData>(DataAsset);
            Debug.Assert(data != null);

            Texture = AssetManager.GetTexture(data.AnimationTextureAsset);
            Debug.Assert(Texture != null);

            Frames = data.TextureFrames;
            Continual = data.Continual;

            TimePerFrame = defaultTimePerFrame;
            FrameDimensions = new Point(Texture.Width / Frames.X, Texture.Height / Frames.Y);
            Centre = new Vector2(FrameDimensions.X * 0.5f, FrameDimensions.Y * 0.5f);

            CalculateSourceRectangle();
        }

        /// <summary>
        /// Updates the time on the current animation frame and the animation frame if needs be
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public void Update(float elapsedGameTime)
        {
            if (IsPlaying)
            {
                currentTimeOnFrame += elapsedGameTime;

                if (currentTimeOnFrame >= TimePerFrame)
                {
                    // We have changed frame so recalculate the source rectangle
                    CurrentFrame++;

                    // If the animation should only play once, we remove it once it reaches the last frame
                    if (!Continual)
                    {
                        if (CurrentFrame == Frames.X * Frames.Y - 1)
                        {
                            Finished = true;
                            IsPlaying = false;

                            if (OnAnimationComplete != null)
                            {
                                OnAnimationComplete();
                            }

                            return;
                        }
                    }
                    else
                    {
                        CurrentFrame %= Frames.X * Frames.Y;
                    }

                    CalculateSourceRectangle();

                    currentTimeOnFrame = 0;
                }
            }
            else
            {
                CurrentFrame = 0;
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Calculates the source rectangle from the sprite sheet we should use based on the dimensions and current frame.
        /// </summary>
        private void CalculateSourceRectangle()
        {
            int currentRow = CurrentFrame / Frames.X;
            int currentColumn = CurrentFrame % Frames.X;

            Debug.Assert(currentColumn < Frames.X);
            Debug.Assert(currentRow < Frames.Y);

            CurrentSourceRectangle = new Rectangle(currentColumn * FrameDimensions.X, currentRow * FrameDimensions.Y, FrameDimensions.X, FrameDimensions.Y);
        }

        /// <summary>
        /// Resets the animation to make it ready to play again.
        /// </summary>
        public void Reset()
        {
            currentTimeOnFrame = 0;
            IsPlaying = false;
            CurrentFrame = 0;
            Finished = false;

            CalculateSourceRectangle();
        }

        #endregion
    }
}
