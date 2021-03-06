﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace _2DEngine
{
    /// <summary>
    /// A screen which is responsible for presenting the options for the game to the user to edit.
    /// For now, it holds generic options so can be overridden for custom game options.
    /// </summary>
    public class OptionsScreen : MenuScreen
    {
        public OptionsScreen(MenuScreen previousMenuScreen, string screenDataAsset = "Content\\Data\\Screens\\OptionsScreen.xml") :
            base(previousMenuScreen, screenDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Adds UI to alter the UI.
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            float padding = ScreenDimensions.Y * 0.1f;

            Label titleLabel = AddScreenUIObject(new Label("Options", new Vector2(ScreenCentre.X, ScreenDimensions.Y * 0.1f))) as Label;

            Slider musicVolumeSlider = AddScreenUIObject(new Slider(OptionsManager.MusicVolume.Value, "Music Volume", new Vector2(0, padding))) as Slider;
            musicVolumeSlider.SetParent(titleLabel);
            OptionsManager.MusicVolume.Connect(musicVolumeSlider.CurrentValue);

            Slider sfxVolumeSlider = AddScreenUIObject(new Slider(OptionsManager.SFXVolume.Value, "SFX Volume", new Vector2(0, padding))) as Slider;
            sfxVolumeSlider.SetParent(musicVolumeSlider);
            // Connect up SFX manager

            Button fullScreenButton = AddScreenUIObject(new Button(OptionsManager.IsFullScreen.ToString(), new Vector2(0, padding))) as Button;
            fullScreenButton.Name = "Fullscreen Button";
            fullScreenButton.SetParent(sfxVolumeSlider);
            fullScreenButton.OnClicked += SyncOptionsIsFullScreen;

            Label fullScreenLabel = AddScreenUIObject(new Label("Fullscreen", Vector2.Zero)) as Label;
            fullScreenLabel.Name = "Fullscreen Label";
            fullScreenLabel.SetParent(fullScreenButton);
        }

        /// <summary>
        /// Fixup UI positions
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Label fullScreenLabel = FindScreenUIObject<Label>("Fullscreen Label");
            Button fullScreenButton = FindScreenUIObject<Button>("Fullscreen Button");

            float padding = 5;
            fullScreenLabel.LocalPosition = new Vector2(-fullScreenLabel.Size.X * 0.5f - fullScreenButton.Size.X * 0.5f - padding, 0);
        }

        // Use scroll wheel to move camera up and down if we have loads of options?

        #endregion

        #region Callbacks

        /// <summary>
        /// Syncs our button's value with our options is full screen option
        /// </summary>
        /// <param name="button"></param>
        private void SyncOptionsIsFullScreen(ClickableImage image)
        {
            Button button = image as Button;
            DebugUtils.AssertNotNull(button);

            OptionsManager.IsFullScreen = !OptionsManager.IsFullScreen;
            button.Label.Text = OptionsManager.IsFullScreen.ToString();
        }

        #endregion
    }
}