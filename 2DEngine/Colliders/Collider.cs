using Microsoft.Xna.Framework;
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
        /// A flag to indicate whether our mouse is over the object
        /// </summary>
        public bool IsMouseOver { get; private set; }

        #endregion

        public Collider(BaseObject parent)
        {
            Parent = parent;
        }

        #region Abstract Collision Functions

        /// <summary>
        /// Checks collision with inputted point and updates the CollidedThisFrame bool
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>Returns true if a collision occurred</returns>
        public abstract bool CheckCollisionWith(Vector2 point);

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
            IsMouseOver = CheckCollisionWith(mousePosition);

            // If the mouse is over this and the mouse is clicked the object is clicked
            IsClicked = IsMouseOver && GameMouse.Instance.IsClicked(MouseButton.kLeftButton);
        }

        /// <summary>
        /// Updates collider positions and collision bools
        /// </summary>
        public virtual void Update()
        {
            
        }

        #endregion
    }
}
