using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// A singleton class which is responsible for managing the screens in game
    /// </summary>
    public class ScreenManager : ObjectManager<BaseScreen>
    {
        #region Properties and Fields

        /// <summary>
        /// We will only have one instance of this class, so have a static Instance which can be accessed
        /// anywhere in the program by calling ScreenManager.Instance
        /// </summary>
        private static ScreenManager instance;
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// The SpriteBatch we will use for all our rendering.
        /// It will be used in three rendering loops:
        /// Game Objects - objects we want to be affected by our camera position
        /// In Game UI Objects - UI objects we want to be affected by our camera position
        /// Screen UI Objects - UI Objects we want to be independent of our camera position
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        /// <summary>
        /// The device we can use to load content.
        /// Not really used as we use the AssetManager to obtain all of our Content instead.
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// The Graphics Device we can use to change display settings.
        /// Not really used except at startup and during Options changes.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        /// <summary>
        /// The Viewport for our game window - can be used to access screen dimensions
        /// </summary>
        public Viewport Viewport { get; private set; }

        /// <summary>
        /// A wrapper property to return the Viewport's width and height
        /// </summary>
        public Vector2 ScreenDimensions { get; private set; }

        /// <summary>
        /// A wrapper property to return the centre of the screen
        /// </summary>
        public Vector2 ScreenCentre { get; private set; }

        /// <summary>
        /// A reference to our current screen
        /// </summary>
        public BaseScreen CurrentScreen { get; private set; }

        #endregion

        /// <summary>
        /// Constructor is private because this class will be accessed through static 'Instance' property
        /// </summary>
        private ScreenManager() :
            base()
        {
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the game mouse and any screens already added
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            OptionsManager.Load();
            ScriptManager.Instance.LoadContent();
            GameMouse.Instance.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises the camera and game mouse
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            ScriptManager.Instance.Initialise();
            ThreadManager.Initialise();
            Camera.Initialise();
            GameMouse.Instance.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Updates the keyboard and mouse.
        /// Handles input for Camera and screens in ScreenManager.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition">The screen position of the mouse</param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            // Update keyboard and mouse first
            GameKeyboard.Update();
            GameMouse.Instance.Update(elapsedGameTime);

            // Then handle camera input
            Camera.HandleInput(elapsedGameTime);

            // Handle input for all of the scripts in our script manager
            ScriptManager.Instance.HandleInput(elapsedGameTime, mousePosition);

            // Then finally handle screen input
            base.HandleInput(elapsedGameTime, mousePosition);
        }

        /// <summary>
        /// Update the camera and then any screens
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            // Updates Thread manager
            ThreadManager.Update();

            // Update camera
            Camera.Update(elapsedGameTime);

            // Update ScriptManager scripts
            ScriptManager.Instance.Update(elapsedGameTime);

            // Then update any screens
            base.Update(elapsedGameTime);

            // Deflush the mouse after all input handling and updating
            // If the draw logic depends on this, it is just wrong
            GameMouse.Instance.IsFlushed = false;
        }

        /// <summary>
        /// Adds a screen to the screenmanager and updates our current screen reference
        /// </summary>
        /// <param name="objectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override BaseScreen AddObject(BaseScreen objectToAdd, bool load = false, bool initialise = false)
        {
            CurrentScreen = objectToAdd;

            return base.AddObject(objectToAdd, load, initialise);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Sets up class variables from the main Game1 class which will be useful for our game.
        /// Loads options from XML.
        /// MUST be called before LoadContent and Initialise.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch from our Game1 class</param>
        /// <param name="viewport">The Viewport corresponding to the window</param>
        public void Setup(SpriteBatch spriteBatch, Viewport viewport, ContentManager content, GraphicsDeviceManager graphics)
        {
            // Check that we have called this before loading and initialising
            CheckShouldLoad();
            CheckShouldInitialise();

            SpriteBatch = spriteBatch;
            Content = content;
            Viewport = viewport;
            GraphicsDeviceManager = graphics;

            OptionsManager.Load();

            ScreenDimensions = new Vector2(viewport.Width, viewport.Height);
            ScreenCentre = new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
        }

        /// <summary>
        /// Remove one screen and add another.
        /// If we are transitioning to a gameplay screen, we add a loading screen first (unless we are going from a loading screen).
        /// </summary>
        /// <param name="transitionFrom">The screen to remove</param>
        /// <param name="transitionTo">The screen to add</param>
        /// <param name="load">Whether we should call LoadContent on the screen to add</param>
        /// <param name="initialise">Whether we should call Initialise on the screen to add</param>
        public void Transition(BaseScreen transitionFrom, BaseScreen transitionTo, bool load = true, bool initialise = true)
        {
            if (transitionTo is GameplayScreen && !(transitionFrom is LoadingScreen))
            {
                AddObject(new LoadingScreen(transitionTo as GameplayScreen), true, true);
            }
            else
            {
                AddObject(transitionTo, load, initialise);
            }

            transitionFrom.Die();
        }

        /// <summary>
        /// Adds a startup logo screen and kicks off asset loading
        /// </summary>
        /// <param name="screenAfterLoading">The screen we wish to display after the StartupLogoScreen</param>
        public void StartGame(BaseScreen screenAfterLoading)
        {
            AddObject(new StartupLogoScreen(screenAfterLoading), true, true);
        }

        #endregion
    }
}
