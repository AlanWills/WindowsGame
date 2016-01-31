using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _2DEngine
{
    /// <summary>
    /// A base abstract class for all screens in our game.
    /// Contains three managers, for the GameObjects, In Game UIObjects and Screen UIObjects.
    /// Is responsible for drawing the mouse.
    /// An instance of this class cannot be created because it is too general
    /// </summary>
    public abstract class BaseScreen : Component
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

        /// <summary>
        /// Each screen has a script manager which contains scripts specific to that screen
        /// </summary>
        private ScriptManager ScriptManager { get; set; }

        /// <summary>
        /// A string for the background image.  Simply set this string in LoadContent before base.LoadContent is called
        /// and the background image will be added in AddInitialUI.
        /// </summary>
        protected string BackgroundDataAsset { private get; set; }

        /// <summary>
        /// The screen background
        /// </summary>
        private UIObject Background { get; set; }

        /// <summary>
        /// A variable used to determine whether this screen should queue it's screen music or not.
        /// Set to 'WaitForCurrent' to queue songs after the music already playing (DEFAULT).
        /// Set to 'PlayImmediately to clear current queued songs and play this screen's music.
        /// </summary>
        protected QueueType MusicQueueType { private get; set; }

        #endregion

        public BaseScreen() : 
            base()
        {
            GameObjects = new ObjectManager<GameObject>();
            InGameUIObjects = new ObjectManager<UIObject>();
            ScreenUIObjects = new ObjectManager<UIObject>();

            MusicQueueType = QueueType.WaitForCurrent;

            ScriptManager = new ScriptManager(this);
        }

        // Do three drawing steps here rather than in screen manager
        // Draw mouse

        #region Virtual Functions

        /// <summary>
        /// Called in the LoadContent loop, before we load the Manager classes.
        /// Use this function to add any initial UI which will then get loaded and initialised.
        /// Sets up the background if the BackgroundDataAsset has been set.
        /// </summary>
        protected virtual void AddInitialUI()
        {
            if (string.IsNullOrEmpty(BackgroundDataAsset))
            {
                AddScreenUIObject(new Image(GetScreenDimensions(), GetScreenCentre(), BackgroundDataAsset));
            }
        }

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
        /// Calls Initialise on the three Managers and ScriptManager.
        /// Adds Initial Scripts to the ScriptManager
        /// </summary>
        public override void Initialise()
        {
            if (!ShouldInitialise) { return; }

            GameObjects.Initialise();
            InGameUIObjects.Initialise();
            ScreenUIObjects.Initialise();

            AddInitialScripts();

            ScriptManager.LoadContent();
            ScriptManager.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Adds Initial Scripts after all the Initialisation for this screen has been performed. 
        /// </summary>
        public virtual void AddInitialScripts() { }

        /// <summary>
        /// Queues up any music for this screen
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            MusicManager.QueueSongs(new List<string>(), MusicQueueType);
        }

        /// <summary>
        /// Call HandleInput on the three managers
        /// </summary>
        /// <param name="elapsedGameTime">The time in seconds since the last frame</param>
        /// <param name="mousePosition">The current screen space position of the mouse</param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // See if we should continue or whether the ScriptManager is preventing us
            ScriptManager.HandleInput(elapsedGameTime, mousePosition);
            ShouldHandleInput = ScriptManager.ShouldUpdateGame;

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

            // See if we should continue or whether the ScriptManager is preventing us
            ScriptManager.Update(elapsedGameTime);
            ShouldUpdate = ScriptManager.ShouldUpdateGame;

            // Check to see if we should update
            if (!ShouldUpdate) { return; }

            GameObjects.Update(elapsedGameTime);
            InGameUIObjects.Update(elapsedGameTime);
            ScreenUIObjects.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draws the background first.
        /// Calls draw on the three objects in the order: GameObjects, InGameUIObjects, ScreenUIObjects.
        /// Draws the mouse at the after everything else.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch we should use for drawing sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Check to see if we should draw
            if (!ShouldDraw) { return; }

            // Draw the background if it has been set
            if (Background != null)
            {
                spriteBatch.Begin();

                Background.Draw(spriteBatch);

                spriteBatch.End();
            }

            // Draw the camera dependent objects using the camera transformation matrix
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.TransformationMatrix);

            GameObjects.Draw(spriteBatch);
            InGameUIObjects.Draw(spriteBatch);

            spriteBatch.End();

            // Draw the camera independent objects and the mouse last
            spriteBatch.Begin();

            ScreenUIObjects.Draw(spriteBatch);
            GameMouse.Instance.Draw(spriteBatch);

            spriteBatch.End();
        }

        #endregion

        #region Functions for Managing Objects

        /// <summary>
        /// Adds a GameObject to this screen's GameObjects manager
        /// </summary>
        /// <param name="gameObjectToAdd">The object to add</param>
        /// <param name="load">A flag to indicate whether LoadContent should be called on this object when adding</param>
        /// <param name="initialise">A flag to indicate whether Initialise should be called on this object when adding</param>
        public void AddGameObject(GameObject gameObjectToAdd, bool load = false, bool initialise = false)
        {
            GameObjects.AddObject(gameObjectToAdd, load, initialise);
        }

        /// <summary>
        /// Removes a GameObject from this screen's GameObjects manager
        /// </summary>
        /// <param name="gameObjectToRemove">The game object to remove</param>
        public void RemoveGameObject(GameObject gameObjectToRemove)
        {
            GameObjects.RemoveObject(gameObjectToRemove);
        }

        /// <summary>
        /// Finds a GameObject in this screen's GameObjects manager
        /// </summary>
        /// <typeparam name="K">The type that we wish to return the found object as</typeparam>
        /// <param name="gameObjectName">The name of the object to find</param>
        /// <returns>Returns the found object casted to type K, or null</returns>
        public K FindGameObject<K>(string gameObjectName) where K : GameObject
        {
            return GameObjects.FindObject<K>(gameObjectName);
        }

        /// <summary>
        /// Adds an InGameUIObject to this screen's InGameUIObjects manager
        /// </summary>
        /// <param name="uiObjectToAdd">The object to add</param>
        /// <param name="load">A flag to indicate whether LoadContent should be called on this object when adding</param>
        /// <param name="initialise">A flag to indicate whether Initialise should be called on this object when adding</param>
        public void AddInGameUIObject(UIObject uiObjectToAdd, bool load = false, bool initialise = false)
        {
            InGameUIObjects.AddObject(uiObjectToAdd, load, initialise);
        }

        /// <summary>
        /// Removes an InGameUIObject from this screen's InGameUIObjects manager
        /// </summary>
        /// <param name="uiObjectToRemove"></param>
        public void RemoveInGameUIObject(UIObject uiObjectToRemove)
        {
            InGameUIObjects.RemoveObject(uiObjectToRemove);
        }

        /// <summary>
        /// Finds an InGameUIObject in this screen's InGameUIObjects manager
        /// </summary>
        /// <typeparam name="K">The type we wish to return the found object as</typeparam>
        /// <param name="uiObjectName">The name of the object to find</param>
        /// <returns>Returns the found object casted to type K, or null</returns>
        public K FindInGameUIObject<K>(string uiObjectName) where K : UIObject
        {
            return InGameUIObjects.FindObject<K>(uiObjectName);
        }

        /// <summary>
        /// Adds a UIObject to this screen's ScreenUIObjects manager
        /// </summary>
        /// <param name="uiObjectToAdd">The object to add</param>
        /// <param name="load">A flag to indicate whether LoadContent should be called on this object when adding</param>
        /// <param name="initialise">A flag to indicate whether Initialise should be called on this object when adding</param>
        public void AddScreenUIObject(UIObject uiObjectToAdd, bool load = false, bool initialise = false)
        {
            ScreenUIObjects.AddObject(uiObjectToAdd, load, initialise);
        }

        /// <summary>
        /// Removes a ScreenUIObject from this screen's ScreenUIObjects manager
        /// </summary>
        /// <param name="uiObjectToRemove"></param>
        public void RemoveScreenUIObject(UIObject uiObjectToRemove)
        {
            ScreenUIObjects.RemoveObject(uiObjectToRemove);
        }

        /// <summary>
        /// Finds a ScreenUIObject in this screen's ScreenObjects manager
        /// </summary>
        /// <typeparam name="K">The type we wish to return the found object as</typeparam>
        /// <param name="uiObjectName">The name of the object to find</param>
        /// <returns>Returns he found object casted to type K, or null</returns>
        public K FindScreenUIObject<K>(string uiObjectName) where K : UIObject
        {
            return ScreenUIObjects.FindObject<K>(uiObjectName);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Returns the dimensions of the game window
        /// </summary>
        /// <returns>The game window dimensions</returns>
        protected Vector2 GetScreenDimensions()
        {
            return ScreenManager.Instance.ScreenDimensions;
        }

        /// <summary>
        /// Returns the centre of the game window
        /// </summary>
        /// <returns>The game window centre</returns>
        protected Vector2 GetScreenCentre()
        {
            return ScreenManager.Instance.ScreenCentre;
        }

        /// <summary>
        /// Calls the ScreenManager Transition function.  Moves from the current screen to the inputted screen.
        /// </summary>
        /// <param name="screenToTransitionTo">The screen to transition to</param>
        /// <param name="load">Whether we should call LoadContent on the screen</param>
        /// <param name="initialise">Whether we should call Initialise on the screen</param>
        public void Transition(BaseScreen screenToTransitionTo, bool load = true, bool initialise = true)
        {
            ScreenManager.Instance.Transition(this, screenToTransitionTo, load, initialise);
        }

        /// <summary>
        /// Adds a script to the ScriptManager
        /// </summary>
        /// <param name="script"></param>
        /// <param name="load">Calls Load on the Script</param>
        /// <param name="initialise">Calls Initialise on the Script</param>
        protected void AddScript(Script script, Script previousScript = null, bool load = false, bool initialise = false)
        {
            script.PreviousScript = null;
            ScriptManager.AddObject(script);
        }

        #endregion
    }
}