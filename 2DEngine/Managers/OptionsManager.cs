using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Xml.Serialization;

namespace _2DEngine
{
    public class OptionsData
    {
        public bool IsFullScreen
        {
            get;
            set;
        }

        public float MusicVolume
        {
            get;
            set;
        }

        public float SFXVolume
        {
            get;
            set;
        }
    }

    public static class OptionsManager
    {
        #region Properties and Fields

        public static bool IsFullScreen
        {
            get;
            set;
        }

        public static float MusicVolume
        {
            get;
            set;
        }

        public static float SFXVolume
        {
            get;
            set;
        }

        public static string OptionsFilePath = ScreenManager.Instance.Content.RootDirectory + "\\Options.xml";

        #endregion

        #region Methods

        public static void Load()
        {
            OptionsData optionsData;

            XmlSerializer mySerializer = new XmlSerializer(typeof(OptionsData));
            // To read the file, create a FileStream.

            try
            {
                FileStream myFileStream = new FileStream(OptionsFilePath, FileMode.Open);
                // Call the Deserialize method and cast to the object type.
                optionsData = (OptionsData)mySerializer.Deserialize(myFileStream);

                IsFullScreen = optionsData.IsFullScreen;
                MusicVolume = optionsData.MusicVolume;
                SFXVolume = optionsData.SFXVolume;
            }
            catch
            {
                IsFullScreen = false;
                MusicVolume = 0.5f;
                SFXVolume = 0.25f;
            }

            ScreenManager.Instance.GraphicsDeviceManager.IsFullScreen = IsFullScreen;
            ScreenManager.Instance.GraphicsDeviceManager.ApplyChanges();
            MediaPlayer.Volume = MusicVolume;
        }

        public static void Save()
        {
            OptionsData optionsData = new OptionsData();
            optionsData.IsFullScreen = IsFullScreen;
            optionsData.MusicVolume = MusicVolume;
            optionsData.SFXVolume = SFXVolume;

            XmlSerializer mySerializer = new XmlSerializer(typeof(OptionsData));
            // To write to a file, create a StreamWriter object and overriding current file
            StreamWriter myWriter = new StreamWriter(OptionsFilePath, false);
            mySerializer.Serialize(myWriter, optionsData);
            myWriter.Close();
        }

        #endregion

        #region Virtual Methods

        #endregion
    }
}
