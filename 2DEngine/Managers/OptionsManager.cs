﻿using _2DEngineData;
using Microsoft.Xna.Framework.Media;

namespace _2DEngine
{
    public class OptionsData : BaseData
    {
        public bool IsFullScreen { get; set; }
        public float MusicVolume { get; set; }
        public float SFXVolume { get; set; }
    }

    public static class OptionsManager
    {
        #region Properties and Fields

        /// <summary>
        /// A bool to indicate whether our game is full screen or not.
        /// </summary>
        private static bool isFullScreen = false;
        public static bool IsFullScreen
        {
            get { return isFullScreen; }
            set
            {
                isFullScreen = value;

                ScreenManager.Instance.GraphicsDeviceManager.IsFullScreen = IsFullScreen;
                ScreenManager.Instance.GraphicsDeviceManager.ApplyChanges();
            }
        }

        /// <summary>
        /// A float between 0 and 1 which determines the volume of our game music
        /// </summary>
        private static float musicVolume = 1;
        public static float MusicVolume
        {
            get { return musicVolume; }
            set
            {
                musicVolume = value;

                MediaPlayer.Volume = musicVolume;
            }
        }

        /// <summary>
        /// A float between 0 and 1 which determines the volume of our game SFX
        /// </summary>
        private static float sfxVolume = 1;
        public static float SFXVolume
        {
            get { return sfxVolume; }
            set
            {
                sfxVolume = value;

                // Add volume control for SFX manager here
            }
        }

        #endregion

        #region Methods

        public static void Load()
        {
            DebugUtils.AssertNotNull(AssetManager.OptionsPath);

            OptionsData options = AssetManager.GetData<OptionsData>(AssetManager.OptionsPath);
            DebugUtils.AssertNotNull(options);

            IsFullScreen = options.IsFullScreen;
            MusicVolume = options.MusicVolume;
            SFXVolume = options.SFXVolume;
        }

        public static void Save()
        {
            DebugUtils.AssertNotNull(AssetManager.OptionsPath);

            OptionsData options = new OptionsData();
            options.IsFullScreen = IsFullScreen;
            options.MusicVolume = MusicVolume;
            options.SFXVolume = SFXVolume;

            AssetManager.SaveData(options, AssetManager.OptionsPath);
        }

        #endregion
    }
}
