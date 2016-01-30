using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DEngine
{
    /// <summary>
    /// This class is the very base class for game objects and screens
    /// It holds very basic information
    /// It contains the basic functions that all game objects and screens should call:
    /// LoadContent - obtain textures and data
    /// Initialise - set up class variables from data
    /// Begin - called during the first update loop
    /// HandleInput - uses keyboard and mouse states to perform logic
    /// Update - updates the class logic
    /// Draw - rendering
    /// </summary>

    public class Component
    {
        #region

        // A bool to hold whether LoadContent has been called
        // This is an optimization to stop LoadContent being called multiple times
        protected bool IsLoaded { get; private set; }

        // A bool to hold whether Initialise has been called
        // This is an optimization to stop Initialise being called multiple times
        protected bool IsInitialised { get; private set; }

        // A bool to indicate whether Begun has been called on the object
        // This should only be done once and the first update loop that is called on this object
        protected bool IsBegun { get; private set; }

        // A bool used to clear up this component - if set to false it will be removed from the manager it is in automatically
        public bool IsAlive { get; private set; }

        // A bool used to indicate whether we should call HandleInput on this object
        public bool ShouldHandleInput { get; set; }

        // A bool used to indicate whether we should call Update on this object
        public bool ShouldUpdate { get; set; }

        // A bool used to indicate whether we should call Draw on this object
        public bool ShouldDraw { get; set; }

        #endregion

        // Constructor
        public Component()
        {
            IsLoaded = false;
            IsInitialised = false;
            IsBegun = false;

            IsAlive = true;
            ShouldHandleInput = true;
            ShouldUpdate = true;
            ShouldDraw = true;
        }

        #region Virtual Functions

        /// <summary>
        /// Loads textures and data
        /// </summary>
        public virtual void LoadContent()
        {
            IsLoaded = true;

            // This may seem completely pointless, but it is to emphasise the fact that this check
            // Needs to be done for every single class that inherits of of Component
            if (IsLoaded) { return; }
        }

        /// <summary>
        /// Set up class properties
        /// </summary>
        public virtual void Initialise()
        {
            IsInitialised = true;

            // This may seem completely pointless, but it is to emphasise the fact that this check
            // Needs to be done for every single class that inherits of of Component
            if (IsInitialised) { return; }
        }

        /// <summary>
        /// Call functions at the start of the update loop - used for music etc. in screens for example
        /// </summary>
        public virtual void Begin()
        {
            // Check that we have loaded and initialised this object
            Debug.Assert(IsLoaded);
            Debug.Assert(IsInitialised);

            IsBegun = true;
        }

        /// <summary>
        /// Called every frame - use keyboard state and mouse state to update class logic
        /// </summary>
        /// <param name="elapsedGameTime">The seconds that have elapsed since the last update loop</param>
        /// <param name="mousePosition">The current position of the mouse in the space of the Component (screen or game)</param>
        public virtual void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            // If we shouldn't update then we return
            if (!ShouldHandleInput) { return; }
        }

        /// <summary>
        /// Called every frame - update class logic
        /// </summary>
        /// <param name="elapsedGameTime">The seconds that have elapsed since the last update loop</param>
        public virtual void Update(float elapsedGameTime)
        {
            if (!IsBegun)
            {
                Begin();
            }

            Debug.Assert(IsBegun);

            // If we shouldn't update then we return
            if (!ShouldUpdate) { return; }
        }

        /// <summary>
        /// Called every frame - draws text and sprites
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch we can use to draw textures</param>
        /// <param name="spriteFont">The SpriteFont we can use to draw text</param>
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            // If we shouldn't draw then we return
            if (!ShouldDraw) { return; }
        }

        /// <summary>
        /// Sets IsAlive to false.  The object will then be cleaned up by the ObjectManager it is in.
        /// Can be overrided to provide custom behaviour upon death
        /// </summary>
        public virtual void Die()
        {
            IsAlive = false;
        }

        #endregion
    }
}
