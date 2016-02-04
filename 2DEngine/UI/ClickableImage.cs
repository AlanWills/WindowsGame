using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// An enum to describe the state of a clickable object
    /// </summary>
    public enum ClickState
    {
        kIdle,
        kHighlighted,
        kPressed,
        kDisabled,
    }

    /// <summary>
    /// A class to represent an image which executes a function when clicked
    /// </summary>
    public class ClickableImage : Image
    {
        #region Properties and Fields

        /// <summary>
        /// This represents a function which we want to execute when this object is clicked.
        /// To do this, construct a function of the form:
        /// public void Function(object sender, EventArgs e) { ... }
        /// 
        /// Then do ClickEvent += Function;
        /// 
        /// In fact, more than one function can be set up in this way.
        /// </summary>
        public EventHandler ClickEvent;

        /// <summary>
        /// A variable to mark the current status of the clickable object
        /// </summary>
        protected ClickState ClickState { get; private set; }

        /// <summary>
        /// A timer to prevent multiply clicks happening in quick succession.
        /// </summary>
        private float CurrentClickTimer { get; set; }

        private const float clickResetTime = 0.05f;

        #endregion

        public ClickableImage(Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            this(1, localPosition, textureAsset, lifeTime)
        {
        }

        public ClickableImage(float scale, Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(scale, localPosition, textureAsset, lifeTime)
        {
            // Set so that we can immediately click the button
            CurrentClickTimer = clickResetTime;
            ClickState = ClickState.kIdle;
        }

        public ClickableImage(Vector2 size, Vector2 localPosition, string textureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, textureAsset, lifeTime)
        {
            // Set so that we can immediately click the button
            CurrentClickTimer = clickResetTime;
            ClickState = ClickState.kIdle;
        }

        #region Virtual Functions

        /// <summary>
        /// Update the click state and call the click event if this object is clicked
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            // Check to see if we should handle input
            if (!ShouldHandleInput) { return; }

            base.HandleInput(elapsedGameTime, mousePosition);

            if (Collider.IsMouseOver && ClickState != ClickState.kPressed)
            {
                ClickState = ClickState.kHighlighted;
            }

            if (Collider.IsClicked)
            {
                // If we have clicked it and it is not already clicked, fire the event
                if (ClickState != ClickState.kPressed)
                {
                    // The event should be set up if we have created this class
                    Debug.Assert(ClickEvent != null);

                    ClickEvent(this, EventArgs.Empty);
                    ClickState = ClickState.kPressed;
                    CurrentClickTimer = 0;
                }
            }
        }

        /// <summary>
        /// Updates the current click timer for this object to see if it can be clicked again
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            // Check to see if we should update
            if (!ShouldUpdate) { return; }

            base.Update(elapsedGameTime);

            CurrentClickTimer += elapsedGameTime;
            if (CurrentClickTimer >= clickResetTime)
            {
                ClickState = ClickState.kIdle;
            }
        }

        #endregion
    }
}
