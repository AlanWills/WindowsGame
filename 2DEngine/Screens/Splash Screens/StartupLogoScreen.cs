using Microsoft.Xna.Framework.Content;
using System.Threading;

namespace _2DEngine
{
    /// <summary>
    /// A screen to be displayed on start up whilst we load content.
    /// Very simple - just displays a logo and possibly a background.
    /// </summary>
    public class StartupLogoScreen : MenuScreen
    {
        #region Properties and Fields

        /// <summary>
        /// The screen we wish to transition to after we have finished loading
        /// </summary>
        private BaseScreen ScreenAfterLoading { get; set; }

        #endregion

        public StartupLogoScreen(BaseScreen screenAfterLoading, string screenDataAsset = "Content\\Data\\Screens\\StartupLogoScreen.xml") :
            base(screenDataAsset)
        {
            ScreenAfterLoading = screenAfterLoading;
        }

        #region Virtual Functions

        /// <summary>
        /// Adds the startup logo.
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            AddScreenUIObject(new Logo());
        }

        /// <summary>
        /// Creates a thread to load the content.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            ThreadManager.CreateThread(LoadAllAssetsCallback, TransitionCallback);
        }

        #endregion

        /// <summary>
        /// A callback for our loading thread to load all our game's assets.
        /// </summary>
        private void LoadAllAssetsCallback()
        {
            ContentManager content = ScreenManager.Instance.Content;

            AssetManager.LoadAssets(content);
            MusicManager.LoadAssets(content);
            SFXManager.LoadAssets(content);

            ScreenAfterLoading.LoadContent();
            ScreenAfterLoading.Initialise();
        }

        /// <summary>
        /// A callback for our loading thread to complete when finished loading.
        /// It will transition to the next screen.
        /// </summary>
        private void TransitionCallback()
        {
            Transition(ScreenAfterLoading, false, false);
        }
    }
}