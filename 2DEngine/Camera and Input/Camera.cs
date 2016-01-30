using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DEngine
{
    /// <summary>
    /// An enum to describe the different behaviours that our camera can perform
    /// </summary>
    public enum CameraMode
    {
        kFree,      // The camera will move anywhere based on certain input - mouse at edge of screen, or keyboard
        kFixed,     // The camera will not move at all - useful for menu screens
        kFollow,    // The camera will follow an object - this requires the object to follow to be passed in
    }

    /// <summary>
    /// A singleton class used as an in game camera.
    /// This will also allow us to not render objects if they are not contained within the camera viewport
    /// </summary>
    public class Camera
    {
        #region Properties and Fields

        /// <summary>
        /// A private variable used to determine what behaviour the camera should perform.
        /// This should remain private and instead, to change the camera mode, the appropriate functions 'SetFree', 'SetFixed', 'SetFollow'
        /// should be called.  This is because extra parameters are required for each mode.
        /// </summary>
        private CameraMode CameraMode { get; set; }

        /// <summary>
        /// The position of the Camera - corresponds to where the top left of the screen is.  Again we should not be able to set this outside of this class.
        /// Ways to change the camera position are determined by the CameraMode, and also by a parameter passed in when changing mode ONLY.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// A value to determine how fast the camera should move in Free mode.  This should be freely available for alteration.
        /// </summary>
        public float PanSpeed { get; set; }

        /// <summary>
        /// A value only to be altered and seen by this class, used to determine the zoom of the camera.
        /// This value can be changed in Free or Follow mode by keyboard input ONLY.
        /// </summary>
        private float Zoom { get; set; }

        /// <summary>
        /// This represents the current transformation of the camera calculated from the position and zoom.
        /// It is used as a parameter to SpriteBatch.Begin() to give the impression of a camera when drawing objects
        /// </summary>
        public Matrix TransformationMatrix
        {
            get
            {
                // This could be done with usual operators - *, + etc., but this is an optimisation
                return Matrix.Multiply(Matrix.CreateTranslation(Position.X, Position.Y, 0), Matrix.CreateScale(Zoom));
            }
        }

        /// <summary>
        /// Corresponds to the window of our game - will have coordinates (0, 0, screen width, screen height).
        /// This will be used in determining whether an object is visible or not.
        /// It will need to be set up once, or when our screen size changes.
        /// </summary>
        public Rectangle ViewportRectangle
        {
            get;
            private set;
        }

        /// <summary>
        /// We will only have once Camera so we will make a static instance that can be accessed anywhere in our code via Camera.Instance.
        /// </summary>
        private static Camera singleton;
        public static Camera Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new Camera();
                }

                return singleton;
            }
        }

        #endregion

        /// <summary>
        /// Make the Constructor private so that we just use the static Instance instead.
        /// </summary>
        private Camera()
        {
            CameraMode = CameraMode.kFixed;
            Position = Vector2.Zero;
            PanSpeed = 300;
            Zoom = 1;
            ViewportRectangle = new Rectangle(0, 0, ScreenManager.Instance.Viewport.Width, ScreenManager.Instance.Viewport.Height);
        }

        #region Camera Position and Zoom Update Functions

        /// <summary>
        /// Updates the camera position based on what mode we are in - should be called every frame
        /// </summary>
        /// <param name="elapsedSeconds"></param>
        public void Update(float elapsedSeconds)
        {
            // Only the follow mode requires no user input to update
            if (CameraMode != CameraMode.kFollow) { return; }

            // TODO - Follow mode not supported yet
        }

        /// <summary>
        /// Updates the camera position based on what mode we are in and any appropriate user input - should be called every frame
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public void HandleInput(float elapsedGameTime)
        {
            // If we are in fixed camera mode, we should not update anything
            if (CameraMode == CameraMode.kFixed) { return; }

            // In Free or Follow mode, we can alter the zoom

            // We will be updating the position using keyboard input from now on - this only applies to Free mode
            if (CameraMode == CameraMode.kFollow) { return; }

            // Update the camera position using keyboard input
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// A function used to position the camera so that the inputted position is in the centre of the screen.
        /// Since the camera position corresponds to the top left, this involves removed half the screen dimensions from the inputted position.
        /// </summary>
        /// <param name="focusPosition">The position that we wish to have in the centre of the screen</param>
        public void FocusOnPosition(Vector2 focusPosition)
        {
            Position = focusPosition - new Vector2(ViewportRectangle.Width, ViewportRectangle.Height) * 0.5f;
        }

        /// <summary>
        /// A function for converting a position on the screen into game space, using the zoom and position of the camera
        /// </summary>
        /// <param name="screenPosition">The position on the screen between (0, 0) and (screen width, screen height)</param>
        /// <returns>The game space coordinates corresponding to the inputted screen position</returns>
        public Vector2 ScreenToGameCoords(Vector2 screenPosition)
        {
            // This could be done using ordinary mathematical operators - +, / etc. but this is an optimisation
            return Vector2.Divide(Vector2.Add(Position, screenPosition), Zoom);
        }

        /// <summary>
        /// A function for converting a position in game space into screen space, using the zoom and position of the camera
        /// </summary>
        /// <param name="gamePosition">The position in game space.</param>
        /// <returns>The screen space coordinates corresponding to the inputted game position</returns>
        public Vector2 GameToScreenCoords(Vector2 gamePosition)
        {
            // This could be done using ordinary mathematical operators - +, / etc. but this is an optimisation
            return Vector2.Subtract(Vector2.Multiply(gamePosition, Zoom), Position);
        }

        #endregion

        #region Changing Camera Mode Functions

        /// <summary>
        /// Sets the CameraMode to free
        /// </summary>
        public void SetFree()
        {
            CameraMode = CameraMode.kFree;
        }

        /// <summary>
        /// Sets the CameraMode to free and the camera's position to the inputted value.
        /// </summary>
        /// <param name="resetPosition">The new value of the camera's position</param>
        public void SetFree(Vector2 resetPosition)
        {
            CameraMode = CameraMode.kFree;
            Position = resetPosition;
        }

        /// <summary>
        /// Sets the CameraMode to fixed
        /// </summary>
        public void SetFixed()
        {
            CameraMode = CameraMode.kFixed;
        }

        /// <summary>
        /// Sets the CameraMode to Fixed and the camera's position to the inputted value
        /// </summary>
        /// <param name="resetPosition">The new value of the camera's position</param>
        public void SetFixed(Vector2 resetPosition)
        {
            CameraMode = CameraMode.kFixed;
            Position = resetPosition;
        }

        // TODO Implement follow when we have objects

        #endregion
    }
}
