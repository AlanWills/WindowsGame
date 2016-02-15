using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// A class which extends the ObjectManager.
    /// A game object which also contains a GameObjectManager.
    /// </summary>
    public class GameObjectContainer : GameObject
    {
        #region Properties and Fields

        /// <summary>
        /// A manager for the GameObjects associated with this object.
        /// </summary>
        private ObjectManager<GameObject> GameObjects { get; set; }

        #endregion

        public GameObjectContainer(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            GameObjects = new ObjectManager<GameObject>();
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the objects in the GameObjects manager
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            GameObjects.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises the objects in the GameObjects Manager
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            GameObjects.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Handle Input for all the game objects 
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            GameObjects.HandleInput(elapsedGameTime, mousePosition);
        }

        /// <summary>
        /// Update the game objects
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            GameObjects.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draw the game objects
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            GameObjects.Draw(spriteBatch);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Adds a GameObject to the manager associated with this object.
        /// The object's parent will be set to this object.
        /// </summary>
        /// <param name="gameObjectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        public void AddObject(GameObject gameObjectToAdd, bool load = false, bool initialise = false)
        {
            gameObjectToAdd.Parent = this;

            GameObjects.AddObject(gameObjectToAdd, load, initialise);
        }

        #endregion
    }
}