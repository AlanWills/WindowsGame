﻿using Microsoft.Xna.Framework;
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
            Lights.ShouldDraw = true;

            CollisionObjects = new List<GameObject>();
        }

        #region Virtual Functions

        public override void Initialise()
        {
            base.Initialise();

            Camera.SetFree(Vector2.Zero);
        }

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            CollisionObjects.RemoveAll(x => !x.IsAlive);

            foreach (GameObject collisionObject in CollisionObjects)
            {
                foreach (UIObject backgroundObject in BackgroundObjects)
                {
                    if (collisionObject.Collider.CheckCollisionWith(backgroundObject.Collider))
                    {
                        break;
                    }
                }

                foreach (GameObject gameObject in GameObjects)
                {
                    if (collisionObject != gameObject && collisionObject.Collider.CheckCollisionWith(gameObject.Collider))
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        #region Utility Functions

        protected void AddCollisionObject(GameObject collisionObject)
        {
            Debug.Assert(collisionObject.UsesCollider);

            CollisionObjects.Add(collisionObject);
        }

        #endregion
    }
}
