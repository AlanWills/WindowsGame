using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A simple class which contains a HUD.
    /// Really a base class for more custom gameplay screens
    /// </summary>
    public class GameplayScreen : BaseScreen
    {
        // Can't work out whether this should be static or not.
        // How many gameplay screens will we have?
        // protected HUD HUD {get;set;}

        /// <summary>
        /// The objects we will check collisions with our against GameObjects and Backgrounds
        /// </summary>
        private List<GameObject> CollisionObjects { get; set; }

        public GameplayScreen(string screenDataAsset) :
            base(screenDataAsset)
        {
            Lights.ShouldDraw.Value = true;
            CollisionObjects = new List<GameObject>();
        }

        #region Virtual Functions

        /// <summary>
        /// Updates the Camera to be free moving
        /// </summary>
        public override void Initialise()
        {
            base.Initialise();

            Camera.SetFree(Vector2.Zero);
        }

        /// <summary>
        /// Handles collisions of CollisionObjects
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            CollisionObjects.RemoveAll(x => !x.IsAlive.Value);

            foreach (GameObject collisionObject in CollisionObjects)
            {
                foreach (UIObject backgroundObject in EnvironmentObjects)
                {
                    if (backgroundObject.UsesCollider && collisionObject.Collider.CheckCollisionWith(backgroundObject.Collider))
                    {
                        float angle = MathUtils.AngleBetweenPoints(backgroundObject.WorldPosition, collisionObject.WorldPosition);

                        Vector2 collisionObjectWorldPosition = collisionObject.WorldPosition;
                        Vector2 backgroundObjectWorldPosition = backgroundObject.WorldPosition;
                        Vector2 collisionObjectHalfSize = collisionObject.Collider.Size * 0.5f;
                        Vector2 backgroundObjectHalfSize = backgroundObject.Collider.Size * 0.5f;

                        Vector2 correction = Vector2.Zero;

                        if (MathUtils.CheckCollisionFromAbove(angle))
                        {
                            // Collided from the top or bottom so stop Y velocity and acceleration
                            //collisionObject.PhysicsBody.FullLinearStop(Dimensions.kY);
                            // Diff between bottom of object and top of collider
                            correction.Y += (backgroundObjectWorldPosition.Y - backgroundObjectHalfSize.Y) - (collisionObjectWorldPosition.Y + collisionObjectHalfSize.Y);
                        }
                        else if (MathUtils.CheckCollisionFromBelow(angle))
                        {
                            //collisionObject.PhysicsBody.FullLinearStop(Dimensions.kY);
                            correction.Y += (backgroundObjectWorldPosition.Y + backgroundObjectHalfSize.Y) - (collisionObjectWorldPosition.Y - collisionObjectHalfSize.Y);
                        }

                        if (MathUtils.CheckCollisionFromLeft(angle))
                        {
                            //collisionObject.PhysicsBody.FullLinearStop(Dimensions.kX);
                            correction.X += (backgroundObjectWorldPosition.X - backgroundObjectHalfSize.X) - (collisionObjectWorldPosition.X + collisionObjectHalfSize.X);
                        }
                        else if (MathUtils.CheckCollisionFromRight(angle))
                        {
                            //collisionObject.PhysicsBody.FullLinearStop(Dimensions.kX);
                            correction.X += (backgroundObjectWorldPosition.X + backgroundObjectHalfSize.X) - (collisionObjectWorldPosition.X - collisionObjectHalfSize.X);
                        }

                        // Stops small oscillations when in contact with surface - FORGET THIS FOR NOW, BUT LATER WE WILL NEED TO CHANGE INTERSECTION CODE TOO
                        /*if (-PhysicsConstants.IntersectionDelta <= correction.X && correction.X <= PhysicsConstants.IntersectionDelta)
                        {
                            correction.X = 0;
                        }
                        if (-PhysicsConstants.IntersectionDelta <= correction.Y && correction.Y <= PhysicsConstants.IntersectionDelta)
                        {
                            correction.Y = 0;
                        }*/

                        collisionObject.LocalPosition += correction;
                    }
                }

                foreach (GameObject gameObject in GameObjects)
                {
                    DebugUtils.AssertNotNull(gameObject.Collider);
                    if (collisionObject != gameObject && collisionObject.Collider.CheckCollisionWith(gameObject.Collider))
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// A function used to indicate that a GameObject should check collisions with the BackgroundObjects and other GameObjects
        /// </summary>
        /// <param name="collisionObject"></param>
        protected void AddCollisionObject(GameObject collisionObject)
        {
            Debug.Assert(collisionObject.UsesCollider);
            DebugUtils.AssertNotNull(collisionObject.PhysicsBody);

            CollisionObjects.Add(collisionObject);
        }

        #endregion
    }
}
