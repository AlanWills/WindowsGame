using _2DEngineData;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        private const string SpritesPath = "\\Sprites";
        private const string EffectsPath = "\\Effects";
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

        private static Dictionary<string, SpriteFont> SpriteFonts { get; set; }
        private static Dictionary<string, Texture2D> Sprites { get; set; }
        private static Dictionary<string, Effect> Effects { get; set; }
        private static Dictionary<string, BaseData> Data { get; set; }

        #endregion

        /// <summary>
        /// Loads all the assets from the default spritefont, sprites and data directories.
        /// Formats them into dictionaries so that they can be obtained with just the filename (minus the filetype)
        /// </summary>
        /// <param name="content"></param>
        public static void LoadAssets(ContentManager content)
        {
            SpriteFonts = Load<SpriteFont>(content, SpriteFontsPath);
            Sprites = Load<Texture2D>(content, SpritesPath);
            Effects = Load<Effect>(content, EffectsPath);
        }

        /// <summary>
        /// Loads all the assets of an inputted type that exist in our Content folder
        /// </summary>
        /// <typeparam name="T">The type of asset to load</typeparam>
        /// <param name="content">The ContentManager we will use to load our content</param>
        /// <param name="path">The path of the assets we wish to load</param>
        /// <returns>Returns the dictionary of all loading content</returns>
        private static Dictionary<string, T> Load<T>(ContentManager content, string path)
        {
            Dictionary<string, T> objects = new Dictionary<string, T>();

            try
            {
                string[] files = Directory.GetFiles(content.RootDirectory + path, "*.xnb*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    // Remove the Content\\ from the start
                    files[i] = files[i].Remove(0, 8);

                    // Remove the .xnb at the end
                    files[i] = files[i].Split('.')[0];

                    try
                    {
                        objects.Add(files[i], LoadFromContentManager<T>(files[i]));
                    }
                    catch { Debug.Fail("Adding asset more than once."); }
                }
            }
            catch { Debug.Fail("Serious failure in AssetManager loading assets from " + path); }

            return objects;
        }

        /// <summary>
        /// A wrapper for loading content directly using the ContentManager.
        /// Should only be used as a last resort.
        /// </summary>
        /// <typeparam name="T">The type of content to load</typeparam>
        /// <param name="path">The path of the object e.g. Sprites\\UI\\Cursor</param>
        /// <returns>The loaded content</returns>
        public static T LoadFromContentManager<T>(string path)
        {
            T asset = ScreenManager.Instance.Content.Load<T>(path);

            DebugUtils.AssertNotNull(asset);

            return asset;
        }

        #region Utility Functions

        /// <summary>
        /// Get a loaded SpriteFont
        /// </summary>
        /// <param name="path">The full path of the SpriteFont, e.g. "SpriteFonts\\DefaultSpriteFont"</param>
        /// <returns>Returns the sprite font</returns>
        public static SpriteFont GetSpriteFont(string path)
        {
            SpriteFont spriteFont;

            if (SpriteFonts.TryGetValue(path, out spriteFont))
            {
                return spriteFont;
            }
            else
            {
                return LoadFromContentManager<SpriteFont>(path);
            }
        }

        /// <summary>
        /// Get a pre-loaded sprite
        /// </summary>
        /// <param name="path">The full path of the Sprite, e.g. "Sprites\\UI\\Cursor"</param>
        /// <returns>Returns the texture</returns>
        public static Texture2D GetSprite(string path)
        {
            Texture2D sprite;

            if (Sprites.TryGetValue(path, out sprite))
            {
                return sprite;
            }
            else
            {
                return LoadFromContentManager<Texture2D>(path);
            }
        }

        /// <summary>
        /// Get a pre-loaded effect
        /// </summary>
        /// <param name="path">The full path of the Effect, e.g. "Effects\\LightEffect"</param>
        /// <returns>Returns the effect</returns>
        public static Effect GetEffect(string path)
        {
            Effect effect;

            if (Effects.TryGetValue(path, out effect))
            {
                return effect;
            }
            else
            {
                return LoadFromContentManager<Effect>(path);
            }
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

                DebugUtils.AssertNotNull(data);
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

