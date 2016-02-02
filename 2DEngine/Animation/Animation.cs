using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _2DEngine
{
    public class Animation
    {
        #region Animation Properties

        /// <summary>
        /// The string path for the texture asset
        /// </summary>
        private string TextureAsset { get; set; }

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
        /// The starting frame of the animation
        /// </summary>
        private int DefaultFrame { get; set; }

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

        private float currentTimeOnFrame = 0;

        #endregion

        public Animation(string textureAsset, int framesInX, int framesInY, float timePerFrame, bool isPlaying = true, bool continual = true, int defaultFrame = 0)
        {
            TextureAsset = textureAsset;
            Frames = new Point(framesInX, framesInY);
            TimePerFrame = timePerFrame;
            IsPlaying = isPlaying;
            Continual = continual;
            DefaultFrame = defaultFrame;
            CurrentFrame = defaultFrame;
        }

        #region LoadContent and Update functions
        
        /// <summary>
        /// Loads the texture and finalises all the information the animation needs
        /// </summary>
        public void LoadContent()
        {
            Texture = AssetManager.GetTexture(TextureAsset);

            Debug.Assert(Texture != null);

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
                CurrentFrame = DefaultFrame;
            }
        }

        #endregion

        #region Utility Functions

        private void CalculateSourceRectangle()
        {
            int currentRow = CurrentFrame / Frames.X;
            int currentColumn = CurrentFrame % Frames.X;

            Debug.Assert(currentColumn < Frames.X);
            Debug.Assert(currentRow < Frames.Y);

            CurrentSourceRectangle = new Rectangle(currentColumn * FrameDimensions.X, currentRow * FrameDimensions.Y, FrameDimensions.X, FrameDimensions.Y);
        }

        #endregion
    }
}
