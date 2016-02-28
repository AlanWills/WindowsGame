using _2DEngineData;
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
        private static Property<float> musicVolume = new Property<float>(1);
        public static Property<float> MusicVolume
        {
            get { return musicVolume; }
        }

        /// <summary>
        /// A float between 0 and 1 which determines the volume of our game SFX
        /// </summary>
        private static Property<float> sfxVolume = new Property<float>(1);
        public static Property<float> SFXVolume
        {
            get { return sfxVolume; }
        }

        #endregion

        #region Methods

        public static void Load()
        {
            DebugUtils.AssertNotNull(AssetManager.OptionsPath);

            OptionsData options = AssetManager.GetData<OptionsData>(AssetManager.OptionsPath);
            DebugUtils.AssertNotNull(options);

            IsFullScreen = options.IsFullScreen;
            MusicVolume.Value = options.MusicVolume;
            SFXVolume.Value = options.SFXVolume;
        }

        public static void Save()
        {
            DebugUtils.AssertNotNull(AssetManager.OptionsPath);

            OptionsData options = new OptionsData();
            options.IsFullScreen = IsFullScreen;
            options.MusicVolume = MusicVolume.Value;
            options.SFXVolume = SFXVolume.Value;

            AssetManager.SaveData(options, AssetManager.OptionsPath);
        }

        #endregion
    }
}
