using Microsoft.Xna.Framework;

namespace _2DEngine
{
    public enum Dimensions
    {
        kX = 1,
        kY = 2,
        kXAndY = 4,
    }

    /// <summary>
    /// A class which uses physics equations to update a GameObject's position
    /// </summary>
    public class PhysicsBody
    {
        #region Properties and Fields

        /// <summary>
        /// The game object we will be applying physics too
        /// </summary>
        private GameObject GameObject { get; set; }

        /// <summary>
        /// The object's linear velocity
        /// </summary>
        public Vector2 LinearVelocity { get; set; }

        /// <summary>
        /// The object's linear acceleration
        /// </summary>
        private Vector2 linearAcceleration;
        public Vector2 LinearAcceleration
        {
            get { return linearAcceleration; }
            set
            {
                //linearAcceleration = value - new Vector2(0, PhysicsConstants.Gravity);
                linearAcceleration = value;
            }
        }

        /// <summary>
        /// The object's angular velocity
        /// </summary>
        public float AngularVelocity { get; set; }

        /// <summary>
        /// The object's angular acceleration
        /// </summary>
        public float AngularAcceleration { get; set; }

        #endregion

        public PhysicsBody(GameObject gameObject)
        {
            GameObject = gameObject;
            LinearAcceleration = Vector2.Zero;
        }

        public void Update(float elapsedGameTime)
        {
            if (GameObject.Collider.CollidedThisFrame && !GameObject.Collider.CollidedLastFrame)
            {
                FullLinearStop(Dimensions.kY);
            }

            // Update the angular components
            AngularVelocity += AngularAcceleration * elapsedGameTime;
            GameObject.LocalRotation += AngularVelocity * elapsedGameTime;

            // Update the linear components
            LinearVelocity += LinearAcceleration * elapsedGameTime;

            if (!GameObject.Collider.CollidedThisFrame)
            {
                LinearVelocity -= new Vector2(0, PhysicsConstants.Gravity * elapsedGameTime);
            }

            GameObject.LocalPosition -= Vector2.Transform(LinearVelocity, Matrix.CreateRotationZ(GameObject.LocalRotation)) * elapsedGameTime;
        }

        #region Utility Functions

        public void FullLinearStop(Dimensions dimensions = Dimensions.kXAndY)
        {
            Vector2 velocity = Vector2.Zero;
            Vector2 acceleration = Vector2.Zero;

            if (dimensions == Dimensions.kY)
            {
                velocity.X = LinearVelocity.X;
                acceleration.X = LinearAcceleration.X;
            }

            if (dimensions == Dimensions.kX)
            {
                velocity.Y = LinearVelocity.Y;
                acceleration.Y = LinearAcceleration.Y;
            }

            LinearVelocity = velocity;
            LinearAcceleration = acceleration;
        }

        #endregion
    }
}
