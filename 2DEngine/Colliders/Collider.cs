using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A base class for all colliders.  Contains the functions that inherited colliders must provide behaviour for.
    /// Also takes care of Mouse interactions
    /// </summary>
    public abstract class Collider
    {
        #region Properties and Fields

        /// <summary>
        /// We need to update the collider's position every frame
        /// </summary>
        protected BaseObject Parent { get; private set; }

        /// <summary>
        /// A flag to indicate whether we have collided with an object this frame.
        /// Does not include mouse collisions.
        /// </summary>
        public bool CollidedThisFrame { get; protected set; }

        /// <summary>
        /// A flag to indicate whether we have collided with an object last frame.
        /// Does not include mouse collisions.
        /// </summary>
        public bool CollidedLastFrame { get; protected set; }

        /// <summary>
        /// A flag to indicate whether we have been clicked on
        /// </summary>
        public bool IsClicked { get; private set; }

        /// <summary>
        /// A flag to indicate whether we have been pressed on
        /// </summary>
        public bool IsPressed { get; private set; }

        /// <summary>
        /// A flag to indicate whether our mouse is over the object
        /// </summary>
        public bool IsMouseOver { get; private set; }

        /// <summary>
        /// The size of the collider.
        /// </summary>
        public Vector2 Size { get; protected set; }

        #endregion

        public Collider(BaseObject parent)
        {
            Parent = parent;
        }

        #region Abstract Collision Functions

        /// <summary>
        /// Calls the appropriate check function against the type of the inputted collider.
        /// </summary>
        /// <param name="collider">The collider to check against</param>
        /// <returns>Returns true if a collision occurred</returns>
        public virtual bool CheckCollisionWith(Collider collider)
        {
            RectangleCollider rectangleCollider = collider as RectangleCollider;
            if (rectangleCollider != null)
            {
                return CheckCollisionWith(rectangleCollider);
            }

            Debug.Fail("Checking against an unknown collider.");
            return false;
        }

        /// <summary>
        /// Check collision with inputted rectangle collider and updates the CollidedThisFrame bool
        /// </summary>
        /// <param name="rectangleCollider">The rectangle collider to check against</param>
        /// <returns>Returns true if a collision occurred</returns>
        public abstract bool CheckCollisionWith(RectangleCollider rectangleCollider);

        /// <summary>
        /// Checks collision with inputted point.  Does not update CollidedThisFrame bool
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>Returns true if a collision occurred</returns>
        public abstract bool CheckIntersects(Vector2 point);

        /// <summary>
        /// Check whether this collider intersects the inputted rectangle.  Does not update CollidedThisFrame bool.
        /// </summary>
        /// <param name="rectangle">The inputted rectangle to test against</param>
        /// <returns>Returns true if the collider intersects the rectangle</returns>
        public abstract bool CheckIntersects(Rectangle rectangle);

        #endregion

        #region Collider Update and Handle Input Functions

        /// <summary>
        /// Checks collisions with the mouse and updates the appropriate bools
        /// </summary>
        /// <param name="mousePosition"></param>
        public void HandleInput(Vector2 mousePosition)
        {
            CollidedLastFrame = CollidedThisFrame;
            CollidedThisFrame = false;

            // If the mouse position and this have collided the mouse is over it
            IsMouseOver = CheckIntersects(mousePosition);

            // If the mouse is over this and the left mouse button is clicked, the object is clicked
            IsClicked = IsMouseOver && GameMouse.Instance.IsClicked(MouseButton.kLeftButton);

            // If the mouse is over this and the left mouse button is down, the object is pressed
            if (GameMouse.Instance.IsDown(MouseButton.kLeftButton))
            {
                IsPressed = IsPressed || IsMouseOver;
            }
            else
            {
                IsPressed = false;
            }
        }

        /// <summary>
        /// Updates collider positions and collision bools
        /// </summary>
        public virtual void Update() { }

        #endregion
    }
}
