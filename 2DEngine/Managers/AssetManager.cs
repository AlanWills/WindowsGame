using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

        public const string MouseTextureAsset = "Sprites\\Cursor";
        public const string DefaultSpriteFontAsset = "";
        public const string DefaultButtonTextureAsset = "";
        public const string DefaultTextBoxTextureAsset = "";
        public const string StartupLogoTextureAsset = "Sprites\\Logo";

        #endregion

        #region Properties

        public static Dictionary<string, Texture2D> Textures
        {
            get;
            private set;
        }

        public static Dictionary<string, BaseData> Data
        {
            get;
            private set;
        }

        public static Dictionary<string, SpriteFont> SpriteFonts
        {
            get;
            private set;
        }

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
                string[] spriteFontFiles = Directory.GetFiles(content.RootDirectory + SpriteFontsPath, "*.*", SearchOption.AllDirectories);
                for (int i = 0; i < spriteFontFiles.Length; i++)
                {
                    // Remove the Content\\ from the start
                    spriteFontFiles[i] = spriteFontFiles[i].Remove(0, 8);

                    // Remove the .xnb at the end
                    spriteFontFiles[i] = spriteFontFiles[i].Split('.')[0];

                    SpriteFonts.Add(spriteFontFiles[i], content.Load<SpriteFont>(spriteFontFiles[i]));
                }
            }
            catch { /*Debug.Fail("Serious failure in AssetManager lpading SpriteFonts.");*/ }

            Textures = new Dictionary<string, Texture2D>();

            try
            {
                string[] textureFiles = Directory.GetFiles(content.RootDirectory + "\\Sprites", "*.*", SearchOption.AllDirectories);
                for (int i = 0; i < textureFiles.Length; i++)
                {
                    // Remove the Content\\ from the start
                    textureFiles[i] = textureFiles[i].Remove(0, 8);

                    // Remove the .xnb at the end
                    textureFiles[i] = textureFiles[i].Split('.')[0];

                    Textures.Add(textureFiles[i], content.Load<Texture2D>(textureFiles[i]));
                }
            }
            catch { Debug.Fail("Serious failure in AssetManager loading Textures."); }

            // Can't tell whether this will load all the data in as BaseData or will be clever and load as it should be
            Data = new Dictionary<string, BaseData>();

            /*try
            {
                string[] dataFiles = Directory.GetFiles(content.RootDirectory + "\\Data", "*.*", SearchOption.AllDirectories);
                for (int i = 0; i < dataFiles.Length; i++)
                {
                    // Remove the Content\\ from the start
                    dataFiles[i] = dataFiles[i].Remove(0, 8);

                    // Remove the .xnb at the end
                    dataFiles[i] = dataFiles[i].Split('.')[0];

                    Data.Add(dataFiles[i], content.Load<BaseData>(dataFiles[i]));
                }
            }
            catch { Debug.Fail("Serious failure in AssetManager loading Data."); }*/
            
        }

        #region Utility Functions

        /// <summary>
        /// Get a loaded SpriteFont
        /// </summary>
        /// <param name="name">The name of the SpriteFont, e.g. "SpriteFont"</param>
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
        /// <param name="name">The name of the Texture, e.g. "Cursor"</param>
        /// <returns>Returns the texture</returns>
        public static Texture2D GetTexture(string name)
        {
            Texture2D texture = null;
            Textures.TryGetValue(name, out texture);

            return texture;
        }

        public static T GetData<T>(string name) where T : BaseData
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
            }*/

            return data as T;
        }

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

