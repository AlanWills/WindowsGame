using _2DEngine;
using System;

namespace LevelEditor
{
    /// <summary>
    /// A class used at startup only
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(MenuScreen previousMenuScreen, string dataAsset = "Content\\Data\\Screens\\MainMenuScreen.xml") :
            base(previousMenuScreen, dataAsset)
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
            playButton.OnClicked += OnPlayGameButtonClicked;
        }

        #endregion

        #region Event callbacks for main menu screen buttons

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPlayGameButtonClicked(ClickableImage image)
        {
            Transition(new LevelDesignScreen("Content\\Data\\Levels\\Level1.xml"));
        }

        #endregion
    }
}
