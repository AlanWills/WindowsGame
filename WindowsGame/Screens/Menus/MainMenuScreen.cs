using _2DEngine;
using Microsoft.Xna.Framework;
using System;

namespace WindowsGame
{
    /// <summary>
    /// The starting screen that we will reach after the splash screen.
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(MenuScreen previousMenuScreen, string screenDataAsset = "Content\\Data\\Screens\\MainMenuScreen.xml") :
            base(previousMenuScreen, screenDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Add Buttons to our MainMenuScreen
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            float padding = ScreenDimensions.Y * 0.1f;

            Button playButton = AddScreenUIObject(new Button("Play", ScreenCentre)) as Button;
            playButton.OnClicked += OnPlayGameButtonClicked;

            Button optionsButton = AddScreenUIObject(new Button("Options", new Vector2(0, padding))) as Button;
            optionsButton.SetParent(playButton);
            optionsButton.OnClicked += OnOptionsButtonClicked;

            Button exitGameButton = AddScreenUIObject(new Button("Exit", new Vector2(0, padding))) as Button;
            exitGameButton.SetParent(optionsButton);
            exitGameButton.OnClicked += OnExitGameButtonClicked;
        }

        #endregion

        #region Event callbacks for main menu screen buttons

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="image">The image that was clicked</param>
        protected virtual void OnPlayGameButtonClicked(ClickableImage image)
        {
            Transition(new PlatformGameplayScreen("Content\\Data\\Screens\\Levels\\Level1.xml"));
        }

        /// <summary>
        /// The callback to execute when we press the 'Options' button
        /// </summary>
        /// <param name="image">The image that was clicked</param>
        protected virtual void OnOptionsButtonClicked(ClickableImage image)
        {
            Transition(new OptionsScreen(this));
        }

        /// <summary>
        /// The callback to execute when we press the 'Exit' button
        /// </summary>
        /// <param name="image">Unused</param>
        protected virtual void OnExitGameButtonClicked(ClickableImage image)
        {
            ScreenManager.Instance.EndGame();
        }

        #endregion
    }
}