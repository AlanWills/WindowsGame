using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class which is used to manage game components - it will load, initialise, update, draw and handle input
    /// Typical examples of what you would use this for include screens and objects in game
    /// </summary>
    /// <typeparam name="T">An object which extends Component</typeparam>
    public class ObjectManager<T> : Component where T : Component
    {
        #region Properties and Fields

        // A list to temporarily hold objects we wish to add to ActiveObjects
        private List<T> ObjectsToAdd { get; set; }

        // All the current objects which we will update etc.
        private List<T> ActiveObjects { get; set; }

        #endregion

        // Constructor
        public ObjectManager() : base()
        {
            // Create the lists here
            ObjectsToAdd = new List<T>();
            ActiveObjects = new List<T>();
        }

        #region Virtual Functions

        /// <summary>
        /// Because we add objects in the update loop, the current objects we need to load are in ObjectsToAdd
        /// Iterate through them and call LoadContent on each of them
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            
            // Check to see whether we have already called LoadContent
            if (IsLoaded) { return; }

            foreach (T obj in ObjectsToAdd)
            {
                obj.LoadContent();
            }
        }

        /// <summary>
        /// Because we add objects in the update loop, the current objects we need to load are in ObjectsToRemove.
        /// Iterate through them and call Initialise on each of them
        /// </summary>
        public override void Initialise()
        {
            base.Initialise();

            // Check to see whether we have already called Initialise
            if (IsInitialised) { return; }

            foreach (T obj in ObjectsToAdd)
            {
                obj.Initialise();
            }
        }

        /// <summary>
        /// Iterate through all the objects in ObjectsToAdd and call Begin
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // Do not need to check whether begun has already been called - this is guaranteed

            foreach (T obj in ObjectsToAdd)
            {
                // Call begin on the object
                obj.Begin();
            }
        }

        /// <summary>
        /// Iterate through the ActiveObjects and call HandleInput on them
        /// </summary>
        /// <param name="elapsedGameTime">The seconds that have elapsed since the last update loop</param>
        /// <param name="mousePosition">The current position of the mouse in the space of the Component (screen or game)</param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // If we shouldn't HandleInput, we return
            if (!ShouldHandleInput) { return; }

            // Loop through the active object
            foreach (T obj in ActiveObjects)
            {
                // Update the object
                obj.HandleInput(elapsedGameTime, mousePosition);
            }
        }

        /// <summary>
        /// Add all the objects in ObjectsToAdd to ActiveObjects, then update ActiveObjects before deleting objects in ObjectsToRemove
        /// </summary>
        /// <param name="elapsedGameTime">The seconds that have elapsed since the last update loop</param>
        public override void Update(float elapsedGameTime)
        {
            // Always call the super class' function - it will deal with whether it should run it itself
            base.Update(elapsedGameTime);

            // If we should not Update, we return
            if (!ShouldUpdate) { return; }

            // Add the objects and then clear the list - it is only a temporary holder
            ActiveObjects.AddRange(ObjectsToAdd);
            ObjectsToAdd.Clear();

            // Loop through the active object
            foreach (T obj in ActiveObjects)
            {
                // Update the object
                obj.Update(elapsedGameTime);
            }

            // Remove all the objects that are no longer alive
            ActiveObjects.RemoveAll(x => x.IsAlive == false);
        }

        /// <summary>
        /// Loop through all the objects and call Draw
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch we can use to draw any textures</param>
        /// <param name="spriteFont">The SpriteFont we can use to draw any text</param>
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            base.Draw(spriteBatch, spriteFont);

            // If we shouldn't draw, we return
            if (!ShouldDraw) { return; }

            foreach (T obj in ActiveObjects)
            {
                // Update the object
                obj.Draw(spriteBatch, spriteFont);
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Adds an object to this Manager
        /// </summary>
        /// <param name="objectToAdd">The object we wish to add</param>
        /// <param name="load">A flag to indicate whether we wish to call LoadContent on this object before adding - false by default.</param>
        /// <param name="initialise">A flag to indicate whether we wish to call Initialise on this object before adding - false by default.</param>
        void AddObject(T objectToAdd, bool load = false, bool initialise = false)
        {
            if (load)
            {
                objectToAdd.LoadContent();
            }

            if (initialise)
            {
                objectToAdd.Initialise();
            }
        }

        /// <summary>
        /// Removes an object from this manager
        /// </summary>
        /// <param name="objectToRemove">The object we wish to remove</param>
        void RemoveObject(T objectToRemove)
        {
            Debug.Assert(objectToRemove != null);

            // This function will set IsAlive to false so that the object gets cleaned up next Update loop
            objectToRemove.Die();
        }

        /// <summary>
        /// Iterates through the ActiveObjects in the Manager and returns all of the inputted type in a list
        /// </summary>
        /// <typeparam name="K">Inputted type that we wish to find objects of</typeparam>
        /// <returns>List of all objects in ActiveObjects that can be casted to type K</returns>
        public List<K> GetObjectsOfType<K>() where K : T
        {
            List<K> objects = new List<K>();
            foreach (T obj in ActiveObjects)
            {
                K castedObj = (K)obj;
                if (castedObj != null)
                {
                    objects.Add(castedObj);
                }
            }

            return objects;
        }

        #endregion
    }
}