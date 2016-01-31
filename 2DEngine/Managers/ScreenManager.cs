using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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
            if (!ShouldLoad) { return; }

            GameMouse.Instance.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises the camera and game mouse
        /// </summary>
        public override void Initialise()
        {
            if (!ShouldInitialise) { return; }

            Camera.Initialise();
            GameMouse.Instance.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Updates the keyboard and mouse.
        /// Handles input for Camera and screens in ScreenManager
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            // Check to see if we should handle input
            if (!ShouldHandleInput) { return; }

            // Update keyboard and mouse first
            GameKeyboard.Update();
            GameMouse.Instance.Update(elapsedGameTime);

            // Then handle camera input
            Camera.HandleInput(elapsedGameTime);

            // Then finally handle screen input
            base.HandleInput(elapsedGameTime, mousePosition);
        }

        /// <summary>
        /// Update the camera and then any screens
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            // Check to see if we should update
            if (!ShouldUpdate) { return; }

            // Update camera
            Camera.Update(elapsedGameTime);

            // Then update any screens
            base.Update(elapsedGameTime);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Sets up class variables from the main Game1 class which will be useful for our game.
        /// MUST be called before LoadContent and Initialise.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch from our Game1 class</param>
        /// <param name="viewport">The Viewport corresponding to the window</param>
        public void Setup(SpriteBatch spriteBatch, Viewport viewport, ContentManager content)
        {
            // Check that we have called this before loading and initialising
            Debug.Assert(ShouldLoad);
            Debug.Assert(ShouldInitialise);

            SpriteBatch = spriteBatch;
            Viewport = viewport;

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
            if (transitionTo.Is<GameplayScreen>() && !transitionFrom.Is<LoadingScreen>())
            {
                AddObject(new LoadingScreen(transitionTo.As<GameplayScreen>()), load, initialise);
            }
            else
            {
                AddObject(transitionTo, load, initialise);
            }

            transitionFrom.Die();
        }

        #endregion
    }
}
