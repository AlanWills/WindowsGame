﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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

        /// <summary>
        /// A bool to hold whether LoadContent has been called
        /// This is an optimization to stop LoadContent being called multiple times
        /// </summary>
        protected bool ShouldLoad { get; set; }

        /// <summary>
        /// A bool to hold whether Initialise has been called
        /// This is an optimization to stop Initialise being called multiple times
        /// </summary>
        protected bool ShouldInitialise { get; set; }

        /// <summary>
        /// A bool to indicate whether Begun has been called on the object
        /// This should only be done once and the first update loop that is called on this object
        /// </summary>
        protected bool IsBegun { get; private set; }

        /// <summary>
        /// A bool used to clear up this component - if set to false it will be removed from the manager it is in automatically
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// A bool used to indicate whether we should call HandleInput on this object
        /// </summary>
        public bool ShouldHandleInput { get; set; }

        /// <summary>
        /// A bool used to indicate whether we should call Update on this object
        /// </summary>
        public bool ShouldUpdate { get; set; }

        /// <summary>
        /// A bool used to indicate whether we should call Draw on this object
        /// </summary>
        public bool ShouldDraw { get; set; }

        /// <summary>
        /// A name identifier
        /// </summary>
        public string Name { get; set; }

        #endregion

        /// <summary>
        /// Constructor - sets the component to handle input, update and draw
        /// </summary>
        public Component()
        {
            ShouldLoad = true;
            ShouldInitialise = true;
            IsBegun = false;

            IsAlive = true;
            // Don't call Show here - because we are in a constructor and it is a virtual function, it can have bad knock on effects
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
            ShouldLoad = false;
        }

        /// <summary>
        /// Sets up class properties
        /// </summary>
        public virtual void Initialise()
        {
            ShouldInitialise = false;
        }

        /// <summary>
        /// Call functions at the start of the update loop - used for music etc. in screens for example
        /// </summary>
        public virtual void Begin()
        {
            // Check that we have loaded and initialised this object
            Debug.Assert(!ShouldLoad);
            Debug.Assert(!ShouldInitialise);

            IsBegun = true;
        }

        /// <summary>
        /// Called every frame - use keyboard state and mouse state to update class logic
        /// </summary>
        /// <param name="elapsedGameTime">The seconds that have elapsed since the last update loop</param>
        /// <param name="mousePosition">The current position of the mouse in the space of the Component (screen or game)</param>
        public virtual void HandleInput(float elapsedGameTime, Vector2 mousePosition) { }

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
        }

        /// <summary>
        /// Called every frame - draws text and sprites
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch we can use to draw textures</param>
        public virtual void Draw(SpriteBatch spriteBatch) { }

        /// <summary>
        /// Sets IsAlive to false.  The object will then be cleaned up by the ObjectManager it is in.
        /// Can be overrided to provide custom behaviour upon death
        /// </summary>
        public virtual void Die()
        {
            IsAlive = false;
            Hide();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Sets the component to handle input, update and draw.
        /// DON'T call this during a constructor
        /// </summary>
        public virtual void Show()
        {
            ShouldHandleInput = true;
            ShouldUpdate = true;
            ShouldDraw = true;
        }

        /// <summary>
        /// Sets the component to not handle input, update or draw
        /// </summary>
        public virtual void Hide()
        {
            ShouldHandleInput = false;
            ShouldUpdate = false;
            ShouldDraw = false;
        }


        /// <summary>
        /// Casts this to the inputted type
        /// </summary>
        /// <typeparam name="T">The type we wish to cast to</typeparam>
        /// <returns>this as T or nullptr if not castable</returns>
        public T As<T>() where T : Component
        {
            return this as T;
        }

        /// <summary>
        /// Checks to see whether this is castable to the inputted type
        /// </summary>
        /// <typeparam name="T">The type we wish to cast to</typeparam>
        /// <returns>Returns true if this is castable to T</returns>
        public bool Is<T>() where T : Component
        {
            return this is T;
        }

        #endregion

        #region Debug Checking Functions

        /// <summary>
        /// A function used in Debug to check we are not making unnecessary LoadContent calls
        /// </summary>
        [Conditional("DEBUG")]
        protected void CheckShouldLoad()
        {
            Debug.Assert(ShouldLoad);
        }

        /// <summary>
        /// A function in Debug to check we are not making unnecessary Initialise calls
        /// </summary>
        [Conditional("DEBUG")]
        protected void CheckShouldInitialise()
        {
            Debug.Assert(ShouldInitialise);
        }

        #endregion
    }
}
