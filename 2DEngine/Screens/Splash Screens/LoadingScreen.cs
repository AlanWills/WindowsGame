using System.Threading;

namespace _2DEngine
{
    /// <summary>
    /// An intermediary class used for moving to a gameplay screen.
    /// Is added automatically when we call Transition from the ScreenManager
    /// </summary>
    public class LoadingScreen : BaseScreen
    {
        #region Properties and Fields

        /// <summary>
        /// The screen we wish to transition to after this loading screen is completed
        /// </summary>
        private GameplayScreen ScreenToTransitionTo { get; set; }

        Thread loadingThread;

        #endregion

        public LoadingScreen(GameplayScreen screenToTransitionTo, string screenDataAsset = "Content\\Data\\Screens\\LoadingScreen.xml") :
            base(screenDataAsset)
        {
            ScreenToTransitionTo = screenToTransitionTo;
        }

        #region Virtual Functions

        /// <summary>
        /// Transition to the gameplay screen.
        /// Because the screenmanager will call load during Transition,
        /// this screen hides the pause whilst we load the assets.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            loadingThread = new Thread(new ThreadStart(LoadScreenToTransitionTo));
            loadingThread.Start();
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (!loadingThread.IsAlive)
            {
                Transition(ScreenToTransitionTo, false, false);
            }
        }

        #endregion

        /// <summary>
        /// A function we will pass to our loading thread to perform all the loading and initialising of our screen we wish to transition to.
        /// </summary>
        private void LoadScreenToTransitionTo()
        {
            ScreenToTransitionTo.LoadContent();
            ScreenToTransitionTo.Initialise();
        }
    }
}
