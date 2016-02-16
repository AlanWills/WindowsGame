using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace _2DEngine
{
    /// <summary>
    /// A class which is designed to contain UIObjects.  Useful for Menus and custom UI.
    /// Marked as abstract because we do not want to create an instance of this class since it does not inherit from BaseObject
    /// </summary>
    public class UIContainer : UIObject, IEnumerable
    {
        #region Properties and Fields

        /// <summary>
        /// A manager for the GameObjects associated with this object.
        /// </summary>
        private ObjectManager<UIObject> UIObjects { get; set; }

        #endregion

        public UIContainer(Vector2 localPosition, string textureAsset) :
            this(Vector2.Zero, localPosition, textureAsset)
        {

        }

        public UIContainer(Vector2 size, Vector2 localPosition, string textureAsset) :
            base(size, localPosition, textureAsset)
        {
            UIObjects = new ObjectManager<UIObject>();
        }

        #region Virtual Functions

        /// <summary>
        /// Adds any initial UI to the container
        /// </summary>
        public virtual void AddInitialUI() { }

        /// <summary>
        /// Loads Content for all the UIObjects and adds initial UI
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            AddInitialUI();

            UIObjects.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises all the UIObjects
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            UIObjects.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Handles all the input for the objects in our manager
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            UIObjects.HandleInput(elapsedGameTime, mousePosition);
        }

        /// <summary>
        /// Updates all the objects in our manager
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            UIObjects.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draws all the objects in our manager
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            UIObjects.Draw(spriteBatch);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Adds a UIObject to the manager associated with this object.
        /// The object's parent will be set to this object.
        /// </summary>
        /// <param name="uiObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        public void AddObject(UIObject uiObjectToAdd, bool load = false, bool initialise = false)
        {
            uiObjectToAdd.Parent = this;

            UIObjects.AddObject(uiObjectToAdd, load, initialise);
        }

        /// <summary>
        /// Iterator used so that we can use this class in a foreach loop and it will iterate through the manager's active objects
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return UIObjects.GetEnumerator();
        }

        #endregion
    }
}