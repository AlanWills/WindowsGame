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

            float padding = ScreenDimensions.Y * 0.1f;

            Button playButton = AddScreenUIObject(new Button("Play", ScreenCentre)) as Button;
            playButton.OnClicked += OnPlayGameButtonClicked;

            Button optionsButton = AddScreenUIObject(new Button("Options", new Vector2(0, padding))) as Button;
            optionsButton.Parent = playButton;
            optionsButton.OnClicked += OnOptionsButtonClicked;
        }

        #endregion

        #region Event callbacks for main menu screen buttons

        /// <summary>
        /// The callback to execute when we press the 'Play' button
        /// </summary>
        /// <param name="image">The image that was clicked</param>
        protected virtual void OnPlayGameButtonClicked(ClickableImage image)
        {
            Transition(new PlatformGameplayScreen("Content\\Data\\Levels\\Level1.xml"));
        }

        /// <summary>
        /// The callback to execute when we press the 'Options' button
        /// </summary>
        /// <param name="image">The image that was clicked</param>
        protected virtual void OnOptionsButtonClicked(ClickableImage image)
        {
            Transition(new OptionsScreen());
        }

        #endregion
    }
}