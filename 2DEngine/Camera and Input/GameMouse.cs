using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// An enum for the mouse buttons
    /// </summary>
    public enum MouseButton
    {
        kLeftButton,
        kMiddleButton,
        kRightButton,
    }

    /// <summary>
    /// A singleton class to handle the mouse state
    /// </summary>
    public class GameMouse : UIObject
    {
        #region Properties and Fields

        /// <summary>
        /// We wish the origin of the texture to be the top left, so that the click position is the tip of the cursor
        /// </summary>
        protected override Vector2 TextureCentre{ get { return Vector2.Zero; } }

        /// <summary>
        /// A timer to prevent multiple clicks happening in a short space of time
        /// </summary>
        private float ClickDelayTimer { get; set; }

        /// <summary>
        /// The current state of the mouse - query it to obtain position and mouse button states
        /// </summary>
        private MouseState CurrentMouseState { get; set; }

        /// <summary>
        /// The previous state of the mouse - query this and the current state to obtain mouse button click states
        /// </summary>
        private MouseState PreviousMouseState { get; set; }

        /// <summary>
        /// The single static instance of this class.
        /// </summary>
        private static GameMouse instance;
        public static GameMouse Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameMouse();
                }

                return instance;
            }
        }

        #endregion

        /// <summary>
        /// The constructor is private because we wish to have one single static instance of this class
        /// </summary>
        private GameMouse() :
            base(Vector2.Zero, AssetManager.MouseTextureAsset)
        {
        }

        #region Virtual Functions

        public override void LoadContent()
        {
            if (!ShouldLoad) { return; }

            Texture = ScreenManager.Instance.Content.Load<Texture2D>(AssetManager.MouseTextureAsset);

            base.LoadContent();
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            // Check if we should update the mouse
            if (!ShouldUpdate) { return; }

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            LocalPosition = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }

        #endregion

        #region Functions for Querying Mouse Button States

        /// <summary>
        /// Determines whether the inputted button was released this frame and pressed the previous frame.
        /// This gives the effect of clicking, except we return true when the button is released so that
        /// any effects that might take place of of this only do so when the mouse button is released rather than pressed.
        /// </summary>
        /// <param name="mouseButton">The mouse button we wish to query</param>
        /// <returns>Returns true if the mouse button was pressed in the previous frame and released this frame</returns>
        public bool IsClicked(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.kLeftButton:
                    return CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed;

                case MouseButton.kMiddleButton:
                    return CurrentMouseState.MiddleButton == ButtonState.Released && PreviousMouseState.MiddleButton == ButtonState.Pressed;

                case MouseButton.kRightButton:
                    return CurrentMouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed;

                default:
                    return false;

            }
        }

        /// <summary>
        /// Determines whether the inputted mouse button is pressed this frame
        /// </summary>
        /// <param name="mouseButton">The mouse button we wish to query</param>
        /// <returns>Returns true if the mouse button was pressed this frame</returns>
        public bool IsPressed(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.kLeftButton:
                    return CurrentMouseState.LeftButton == ButtonState.Pressed;

                case MouseButton.kMiddleButton:
                    return CurrentMouseState.MiddleButton == ButtonState.Pressed;

                case MouseButton.kRightButton:
                    return CurrentMouseState.RightButton == ButtonState.Pressed;

                default:
                    return false;

            }
        }

        /// <summary>
        /// Returns whether the mouse has been dragged with the inputted button down
        /// </summary>
        /// <param name="mouseButton">The mouse button we wish to query</param>
        /// <returns>Returns true if the inputted button is down and we have moved the mouse since last frame</returns>
        public bool IsDragged(MouseButton mouseButton)
        {
            return IsPressed(mouseButton) && GetDragDelta() != Vector2.Zero;
        }

        /// <summary>
        /// Returns the amount that we have moved the mouse since last frame
        /// </summary>
        /// <returns>The amount that we have moved the mouse since last frame</returns>
        public Vector2 GetDragDelta()
        {
            return new Vector2(CurrentMouseState.X - PreviousMouseState.X, CurrentMouseState.Y - PreviousMouseState.Y);
        }

        #endregion
    }
}
