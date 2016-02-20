using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Collections;

namespace _2DEngine
{
    /// <summary>
    /// A class which is used to manage game components - it will load, initialise, update, draw and handle input
    /// Typical examples of what you would use this for include screens and objects in game
    /// </summary>
    /// <typeparam name="T">An object which extends Component</typeparam>
    public class ObjectManager<T> : Component, IEnumerable<T> where T : Component
    {
        #region Properties and Fields

        // A list to temporarily hold objects we wish to add to ActiveObjects
        protected List<T> ObjectsToAdd { get; private set; }

        // All the current objects which we will update etc.
        protected List<T> ActiveObjects { get; private set; }

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
            // Check to see whether we have already called LoadContent
            CheckShouldLoad();

            foreach (T obj in ObjectsToAdd)
            {
                obj.LoadContent();
            }

            base.LoadContent();
        }

        /// <summary>
        /// Because we add objects in the update loop, the current objects we need to load are in ObjectsToRemove.
        /// Iterate through them and call Initialise on each of them
        /// </summary>
        public override void Initialise()
        {
            // Check to see whether we have already called Initialise
            CheckShouldInitialise();

            foreach (T obj in ObjectsToAdd)
            {
                obj.Initialise();
            }

            base.Initialise();
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

            // Loop through the active object
            foreach (T obj in ActiveObjects)
            {
                // Handle input for the object
                if (obj.ShouldHandleInput)
                {
                    obj.HandleInput(elapsedGameTime, mousePosition);
                }
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

            // Add the objects and then clear the list - it is only a temporary holder
            ActiveObjects.AddRange(ObjectsToAdd);
            ObjectsToAdd.Clear();

            // Loop through the active object
            foreach (T obj in ActiveObjects)
            {
                if (obj.ShouldUpdate)
                {
                    // Update the object
                    obj.Update(elapsedGameTime);
                }
            }

            // Remove all the objects that are no longer alive
            ActiveObjects.RemoveAll(x => x.IsAlive == false);
        }

        /// <summary>
        /// Loop through all the objects and call Draw
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch we can use to draw any textures</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (T obj in ActiveObjects)
            {
                if (obj.ShouldDraw)
                {
                    // Draw the object
                    obj.Draw(spriteBatch);
                }
            }
        }

        #endregion

        #region Object Management Functions

        /// <summary>
        /// Adds an object to this Manager
        /// </summary>
        /// <param name="objectToAdd">The object we wish to add</param>
        /// <param name="load">A flag to indicate whether we wish to call LoadContent on this object before adding - false by default.</param>
        /// <param name="initialise">A flag to indicate whether we wish to call Initialise on this object before adding - false by default.</param>
        public virtual T AddObject(T objectToAdd, bool load = false, bool initialise = false)
        {
            if (load)
            {
                objectToAdd.LoadContent();
            }

            if (initialise)
            {
                objectToAdd.Initialise();
            }

            ObjectsToAdd.Add(objectToAdd);

            return objectToAdd;
        }

        /// <summary>
        /// Removes an object from this manager
        /// </summary>
        /// <param name="objectToRemove">The object we wish to remove</param>
        public void RemoveObject(T objectToRemove)
        {
            DebugUtils.AssertNotNull(objectToRemove);

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
                if (obj is K)
                {
                    objects.Add(obj as K);
                }
            }

            return objects;
        }

        /// <summary>
        /// Finds an object of the inputted name and casts to the inputted type K.
        /// First searches the ActiveObjects and then the ObjectsToAdd
        /// </summary>
        /// <typeparam name="K">The type we wish to return the found object as</typeparam>
        /// <param name="name">The name of the object we wish to find</param>
        /// <returns>Returns the object casted to K or null</returns>
        public K FindObject<K>(string name) where K : T
        {
            K obj = null;
            obj = ActiveObjects.Find(x => x.Name == name) as K;

            if (obj != null) { return obj; }

            obj = ObjectsToAdd.Find(x => x.Name == name) as K;

            // Really we shouldn't be returning null, because we assume we are trying to find something we know exists
            DebugUtils.AssertNotNull(obj);

            return obj;
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Calls Show on all sub items in this Manager
        /// </summary>
        public override void Show()
        {
            base.Show();

            foreach (T obj in ActiveObjects)
            {
                obj.Show();
            }

            foreach (T obj in ObjectsToAdd)
            {
                obj.Show();
            }
        }

        /// <summary>
        /// Calls Hide on all sub items in this Manager
        /// </summary>
        public override void Hide()
        {
            base.Hide();

            foreach (T obj in ActiveObjects)
            {
                obj.Hide();
            }

            foreach (T obj in ObjectsToAdd)
            {
                obj.Hide();
            }
        }

        public override void Die()
        {
            base.Die();

            foreach (T obj in ActiveObjects)
            {
                obj.Die();
            }

            foreach (T obj in ObjectsToAdd)
            {
                obj.Die();
            }
        }

        /// <summary>
        /// Iterator used so that we can use this class in a foreach loop and it will iterate through the active objects
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)ActiveObjects).GetEnumerator();
        }

        /// <summary>
        /// Iterator used so that we can use this class in a foreach loop and it will iterate through the active objects
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)ActiveObjects).GetEnumerator();
        }

        #endregion
    }
}