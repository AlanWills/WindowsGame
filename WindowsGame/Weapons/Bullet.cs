using _2DEngine;
using Microsoft.Xna.Framework;

namespace WindowsGame
{
    public class Bullet : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// The current time our bullet has been alive.
        /// </summary>
        private float CurrentLifeTimer { get; set; }

        #endregion

        public Bullet(Vector2 localPosition, string textureAsset) :
            base(localPosition, "")
        {
            TextureAsset = textureAsset;

            AddPhysicsBody();
            DebugUtils.AssertNotNull(PhysicsBody);

            PhysicsBody.AffectedByGravity = false;
        }

        #region Virtual Functions

        /// <summary>
        /// Kills this bullet if it has been alive for too long
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            CurrentLifeTimer += elapsedGameTime;
            if (CurrentLifeTimer > 2.5f)
            {
                Die();
            }
        }

        #endregion
    }
}
