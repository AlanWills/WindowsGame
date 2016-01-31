using System;
using Microsoft.Xna.Framework;

namespace _2DEngine
{
    public class RectangleCollider : Collider
    {
        /// <summary>
        /// The rectangle corresponds to the bounds of this collider.
        /// This is done in this way so that we can modify the Location of the Rectangle.
        /// </summary>
        private Rectangle bounds;
        public Rectangle Bounds
        {
            get { return bounds; }
        }

        public RectangleCollider(BaseObject parent) :
            base(parent)
        {
            // Sets up the bounds
            Vector2 parentWorldPos = parent.WorldPosition;
            Vector2 parentSize = parent.Size;
            bounds = new Rectangle(
                (int)parentWorldPos.X, 
                (int)parentWorldPos.Y, 
                (int)parentSize.X, 
                (int)parentSize.Y);
        }

        #region Abstract Collision Functions

        public override bool CheckCollisionWith(Vector2 point)
        {
            bool result = bounds.Contains(point);
            CollidedThisFrame = CollidedThisFrame || result;

            return result;
        }

        #endregion

        #region Collider Update Functions

        /// <summary>
        /// Update the position of the rectangle collider
        /// </summary>
        public override void Update()
        {
            base.Update();

            Vector2 parentWorldPos = Parent.WorldPosition;
            Vector2 parentSize = Parent.Size;

            // Update the bounds location (top left)
            bounds.Location = new Point(
                (int)(parentWorldPos.X - parentSize.X * 0.5f),
                (int)(parentWorldPos.Y - parentSize.Y * 0.5f));
        }

        #endregion
    }
}
