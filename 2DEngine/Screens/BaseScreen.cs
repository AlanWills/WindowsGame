using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// A base class for all screens in our game.
    /// Contains three managers, for the GameObjects, In Game UIObjects and Screen UIObjects.
    /// Is responsible for drawing the mouse
    /// </summary>
    public class BaseScreen : Component
    {
        #region Properties and Fields

        /// <summary>
        /// A Manager for the GameObjects in our screen
        /// </summary>
        private ObjectManager<GameObject> GameObjects { get; set; }

        /// <summary>
        /// A Manager for the In Game (camera dependent) UI Objects in our screen
        /// </summary>
        private ObjectManager<UIObject> InGameUIObjects { get; set; }

        /// <summary>
        /// A Manager for the Screen (camera independent) UI Objects in our screen
        /// </summary>
        private ObjectManager<UIObject> ScreenUIObjects { get; set; }

        #endregion

        public BaseScreen() : 
            base()
        {
            GameObjects = new ObjectManager<GameObject>();
            InGameUIObjects = new ObjectManager<UIObject>();
            ScreenUIObjects = new ObjectManager<UIObject>();
        }

        // Do three drawing steps here rather than in screen manager
        // Draw mouse

        #region Virtual Functions

        /// <summary>
        /// Called in the LoadContent loop, before we load the Manager classes.
        /// Use this function to add any initial UI which will then get loaded and initialised.
        /// </summary>
        protected virtual void AddInitialUI() { }

        /// <summary>
        /// Creates Initial UI and then calls LoadContent on the three Managers
        /// </summary>
        public override void LoadContent()
        {
            // Check if we should load
            if (!ShouldLoad) { return; }

            AddInitialUI();

            GameObjects.LoadContent();
            InGameUIObjects.LoadContent();
            ScreenUIObjects.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Calls Initialise on the three Managers
        /// </summary>
        public override void Initialise()
        {
            if (!ShouldInitialise) { return; }

            GameObjects.Initialise();
            InGameUIObjects.Initialise();
            ScreenUIObjects.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Call HandleInput on the three managers
        /// </summary>
        /// <param name="elapsedGameTime">The time in seconds since the last frame</param>
        /// <param name="mousePosition">The current screen space position of the mouse</param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // Check to see if we should handle input
            if (!ShouldHandleInput) { return; }

            GameObjects.HandleInput(elapsedGameTime, Camera.ScreenToGameCoords(mousePosition));
            InGameUIObjects.HandleInput(elapsedGameTime, Camera.ScreenToGameCoords(mousePosition));
            ScreenUIObjects.HandleInput(elapsedGameTime, mousePosition);
        }

        /// <summary>
        /// Call Update on the three managers
        /// </summary>
        /// <param name="elapsedGameTime">The time in seconds since the last frame</param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            // Check to see if we should update
            if (!ShouldUpdate) { return; }

            GameObjects.Update(elapsedGameTime);
            InGameUIObjects.Update(elapsedGameTime);
            ScreenUIObjects.Update(elapsedGameTime);
        }

        /// <summary>
        /// Calls draw on the three objects in the order: GameObjects, InGameUIObjects, ScreenUIObjects.
        /// Draws the mouse at the after everything else.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch we should use for drawing sprites</param>
        /// <param name="spriteFont">The SpriteFont we should use for drawing text</param>
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            base.Draw(spriteBatch, spriteFont);

            // Check to see if we should draw
            if (!ShouldDraw) { return; }

            // Draw the camera dependent objects using the camera transformation matrix
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.TransformationMatrix);

            GameObjects.Draw(spriteBatch, spriteFont);
            InGameUIObjects.Draw(spriteBatch, spriteFont);

            spriteBatch.End();

            // Draw the camera independent objects and the mouse last
            spriteBatch.Begin();

            ScreenUIObjects.Draw(spriteBatch, spriteFont);
            GameMouse.Instance.Draw(spriteBatch, spriteFont);

            spriteBatch.End();
        }

        #endregion

        #region Functions for Managing Objects

        /// <summary>
        /// Adds a game object to this screen's GameObjects manager
        /// </summary>
        /// <param name="gameObjectToAdd">The object to add</param>
        /// <param name="load">A flag to indicate whether LoadContent should be called on this object when adding</param>
        /// <param name="initialise">A flag to indicate whether Initialise should be called on this object when adding</param>
        public void AddGameObject(GameObject gameObjectToAdd, bool load = false, bool initialise = false)
        {
            GameObjects.AddObject(gameObjectToAdd, load, initialise);
        }

        /// <summary>
        /// Adds an in game ui object to this screen's InGameUIObjects manager
        /// </summary>
        /// <param name="uiObjectToAdd">The object to add</param>
        /// <param name="load">A flag to indicate whether LoadContent should be called on this object when adding</param>
        /// <param name="initialise">A flag to indicate whether Initialise should be called on this object when adding</param>
        public void AddInGameUIObject(UIObject uiObjectToAdd, bool load = false, bool initialise = false)
        {
            InGameUIObjects.AddObject(uiObjectToAdd, load, initialise);
        }

        /// <summary>
        /// Adds a ui object to this screen's ScreenUIObjects manager
        /// </summary>
        /// <param name="uiObjectToAdd">The object to add</param>
        /// <param name="load">A flag to indicate whether LoadContent should be called on this object when adding</param>
        /// <param name="initialise">A flag to indicate whether Initialise should be called on this object when adding</param>
        public void AddScreenUIObject(UIObject uiObjectToAdd, bool load = false, bool initialise = false)
        {
            ScreenUIObjects.AddObject(uiObjectToAdd, load, initialise);
        }

        #endregion
    }
}