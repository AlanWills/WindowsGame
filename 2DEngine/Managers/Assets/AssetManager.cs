using _2DEngineData;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace _2DEngine
{
    /// <summary>
    /// A class to load and cache all the assets (including XML data).
    /// This will be done on startup in the Game1 LoadContent function
    /// </summary>
    public static class AssetManager
    {
        #region File Paths

        private const string SpriteFontsPath = "\\SpriteFonts";
        private const string TexturesPath = "\\Textures";
        private const string DataPath = "\\Data";

        #endregion

        #region Default Assets

        public const string MouseTextureAsset = "Sprites\\UI\\Cursor";
        public const string DefaultSpriteFontAsset = "SpriteFonts\\DefaultSpriteFont";
        public const string DefaultButtonTextureAsset = "Sprites\\UI\\ColourButtonTrial1";
        public const string DefaultTextBoxTextureAsset = "";
        public const string StartupLogoTextureAsset = "Sprites\\UI\\Logo";

        #endregion

        #region Properties

        public static Dictionary<string, SpriteFont> SpriteFonts { get; private set; }
        public static Dictionary<string, Texture2D> Textures { get; private set; }
        public static Dictionary<string, BaseData> Data { get; private set; }

        #endregion

        /// <summary>
        /// Loads all the assets from the default spritefont, sprites and data directories.
        /// Formats them into dictionaries so that they can be obtained with just the filename (minus the filetype)
        /// </summary>
        /// <param name="content"></param>
        public static void LoadAssets(ContentManager content)
        {
            SpriteFonts = new Dictionary<string, SpriteFont>();

            try
            {
                string[] spriteFontFiles = Directory.GetFiles(content.RootDirectory + SpriteFontsPath, "*.xnb*", SearchOption.AllDirectories);
                for (int i = 0; i < spriteFontFiles.Length; i++)
                {
                    // Remove the Content\\ from the start
                    spriteFontFiles[i] = spriteFontFiles[i].Remove(0, 8);

                    // Remove the .xnb at the end
                    spriteFontFiles[i] = spriteFontFiles[i].Split('.')[0];

                    try
                    {
                        SpriteFonts.Add(spriteFontFiles[i], content.Load<SpriteFont>(spriteFontFiles[i]));
                    }
                    catch { Debug.Fail("Adding spritefont more than once."); }
                }
            }
            catch { Debug.Fail("Serious failure in AssetManager loading SpriteFonts."); }

            Textures = new Dictionary<string, Texture2D>();

            try
            {
                string[] textureFiles = Directory.GetFiles(content.RootDirectory + "\\Sprites", "*.xnb", SearchOption.AllDirectories);
                for (int i = 0; i < textureFiles.Length; i++)
                {
                    // Remove the Content\\ from the start
                    textureFiles[i] = textureFiles[i].Remove(0, 8);

                    // Remove the .xnb at the end
                    textureFiles[i] = textureFiles[i].Split('.')[0];

                    try
                    {
                        Textures.Add(textureFiles[i], content.Load<Texture2D>(textureFiles[i]));
                    }
                    catch { Debug.Fail("Adding texture more than once."); }
                }
            }
            catch { Debug.Fail("Serious failure in AssetManager loading Textures."); }

            // Can't tell whether this will load all the data in as BaseData or will be clever and load as it should be
            Data = new Dictionary<string, BaseData>();
        }

        #region Utility Functions

        /// <summary>
        /// Get a loaded SpriteFont
        /// </summary>
        /// <param name="name">The full path of the SpriteFont, e.g. "SpriteFonts\\DefaultSpriteFont"</param>
        /// <returns>Returns the sprite font</returns>
        public static SpriteFont GetSpriteFont(string name)
        {
            SpriteFont spriteFont = null;
            SpriteFonts.TryGetValue(name, out spriteFont);

            return spriteFont;
        }

        /// <summary>
        /// Get a loaded texture
        /// </summary>
        /// <param name="name">The full path of the Texture, e.g. "Sprites\\UI\\Cursor"</param>
        /// <returns>Returns the texture</returns>
        public static Texture2D GetTexture(string name)
        {
            Texture2D texture = null;
            Textures.TryGetValue(name, out texture);

            return texture;
        }

        /// <summary>
        /// Loads all the XML data in the XML Registry.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /*private static void LoadData(string name, Type type)
        {
            Type data = null;

            FileStream readFileStream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);

            XmlRootAttribute rootAttr = new XmlRootAttribute("Root");

            XmlSerializer xml = new XmlSerializer(type, rootAttr);
            data = ()xml.Deserialize(readFileStream);

            Debug.Assert(data != null);

            Data.Add(name, data);
        }*/

        public static T GetData<T>(string name) where T : BaseData
        {
            T data = null;

            using (FileStream readFileStream = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlRootAttribute rootAttr = new XmlRootAttribute(typeof(T).Name);

                XmlSerializer xml = new XmlSerializer(typeof(T), rootAttr);
                data = (T)xml.Deserialize(readFileStream);

                Debug.Assert(data != null);
            }

            return data;
        }

        public static void SaveData<T>(T data, string name) where T : BaseData
        {
            DebugUtils.AssertNotNull(data);

            using (FileStream writeFileStream = new FileStream(name, FileMode.Create,FileAccess.Write))
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(writeFileStream, data);
            }
        }

        /*public static T GetData<T>(string name) where T : BaseData
        {
            BaseData data = null;
            Data.TryGetValue(name, out data);

            Debug.Assert(data != null);

            /*if (data == null)
            {
                
                try
                {
                    return ScreenManager.Content.Load<T>(name);
                }
                catch
                {
                    return null;
                }
            }

            return data as T;
        }*/

        /*public static List<T> GetAllData<T>() where T : BaseData
        {
            List<T> dataOfType = new List<T>();

            foreach (BaseData data in Data.Values)
            {
                T newData = data as T;
                if (newData != null)
                    dataOfType.Add(newData);
            }

            return dataOfType;
        }

        public static string GetKeyFromData(BaseData data)
        {
            return Data.FirstOrDefault(x => x.Value == data).Key;
        }*/

        #endregion
    }
}

