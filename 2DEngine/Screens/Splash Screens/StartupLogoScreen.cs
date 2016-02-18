﻿using Microsoft.Xna.Framework.Content;
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

        Thread loadThread;

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

        public override void Begin()
        {
            base.Begin();

            loadThread = new Thread(new ThreadStart(LoadAllAssets));
            loadThread.Start();
        }

        /// <summary>
        /// Loads all the game assets into their managers.
        /// Do this in the update loop so that we have one draw call before loading.
        /// This way the screen UI will be displayed whilst we load.
        /// </summary>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (!loadThread.IsAlive)
            {
                Transition(ScreenAfterLoading);
            }
        }

        #endregion

        public void LoadAllAssets()
        {
            ContentManager content = ScreenManager.Instance.Content;

            AssetManager.LoadAssets(content);
            MusicManager.LoadAssets(content);
            SFXManager.LoadAssets(content);
        }
    }
}