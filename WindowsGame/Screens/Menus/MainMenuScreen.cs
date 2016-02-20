using _2DEngine;
using System;

namespace WindowsGame
{
    /// <summary>
    /// The starting screen that we will reach after the splash screen.
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(string screenDataAsset = "Content\\Data\\Screens\\MainMenuScreen.xml") :
            base(screenDataAsset)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Add Buttons to our MainMenuScreen
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            Button playButton = AddScreenUIObject(new Button("Play", ScreenCentre)) as Button;
            playButton.ClickEvent += OnPlayGameButtonClicked;
        }

        #endregion

        #region Event callbacks for main menu screen buttons

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPlayGameButtonClicked(object sender, EventArgs e)
        {
            Transition(new PlatformGameplayScreen("Content\\Data\\Levels\\Level1.xml"));
        }

        #endregion
    }
}