using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LevelEditor
{
    /// <summary>
    /// A class which generates a level using a custom algorithm
    /// </summary>
    public class GenerationEngine
    {
        private enum Position
        {
            kLeft,
            kMiddle,
            kRight,
        }

        private struct SpriteData
        {
            public string textureAsset;
            public Vector2 textureDimensions;

            public SpriteData(string asset)
            {
                textureAsset = asset;

                Texture2D tex = AssetManager.GetSprite(textureAsset);
                textureDimensions = new Vector2(tex.Width, tex.Height);
            }
        }

        /// <summary>
        /// Whether the previous object in our array was used 
        /// </summary>
        private bool PreviousObjectExists { get; set; }

        /// <summary>
        /// Whether we will currently add an object for this slot
        /// </summary>
        private bool CurrentObjectExists { get; set; }

        /// <summary>
        /// Whether we will add an object for the next slot
        /// </summary>
        private bool NextObjectExists { get; set; }

        /// <summary>
        /// A reference to the level we will add the generated objects too
        /// </summary>
        private GameplayScreen LevelScreen { get; set; }

        /// <summary>
        /// An array of our objects
        /// </summary>
        private UIObject[] levelObjects;

        private int width = 20;
        private int height = 5;

        private float surfaceWidthFrequency = 0.75f;
        private float surfaceHeightFrequency = 0.3f;
        private int maximumHeightChange = 2;
        private Vector2 spacing;

        private Dictionary<Position, SpriteData> TileData { get; set; }

        /*
        Improvements :
            have a 2D array with width and height
            when moving up and down, only go up and down based on our current position in the grid
            then we can create extra walking layers and other non-collision layers (possibly in other generation engines)
        */

        public GenerationEngine(GameplayScreen levelScreen)
        {
            LevelScreen = levelScreen;

            TileData = new Dictionary<Position, SpriteData>()
            {
                { Position.kLeft, new SpriteData("Sprites\\Level\\Tiles\\Tile (1)") },
                { Position.kMiddle, new SpriteData("Sprites\\Level\\Tiles\\Tile (2)") },
                { Position.kRight, new SpriteData("Sprites\\Level\\Tiles\\Tile (3)") },
            };

            spacing = TileData[Position.kMiddle].textureDimensions;

            levelObjects = new UIObject[width];
        }

        public void GenerateBackground()
        {
            UIObject previousObject = null;
            Vector2 previousPosition = Vector2.Zero;

            Setup(ref previousObject, ref previousPosition);

            for (int x = 0; x < width - 1; x++)
            {
                ShiftExistenceFlags();
                previousObject = Create(ref previousPosition, x);
            }

            TakeDown(previousPosition);
        }

        private void Setup(ref UIObject previousObject, ref Vector2 previousPosition)
        {
            Vector2 screenDimensions = ScreenManager.Instance.ScreenDimensions;

            PreviousObjectExists = false;
            CurrentObjectExists = true;
            NextObjectExists = true;
;
            levelObjects[0] = LevelScreen.AddEnvironmentObject(new Image(new Vector2(0, screenDimensions.Y), QueryTextureAsset()), true, true) as Image;

            PreviousObjectExists = true;
            CurrentObjectExists = true;
            NextObjectExists = ShouldCreate(surfaceWidthFrequency);

            levelObjects[1] = LevelScreen.AddEnvironmentObject(new Image(new Vector2(TileData[Position.kLeft].textureDimensions.X, screenDimensions.Y), QueryTextureAsset()), true, true) as Image;

            previousObject = levelObjects[1];
            previousPosition = previousObject.WorldPosition;
        }

        private void TakeDown(Vector2 previousPosition)
        {
            PreviousObjectExists = CurrentObjectExists;
            CurrentObjectExists = true;
            NextObjectExists = false;

            float xDelta = PreviousObjectExists ? TileData[Position.kMiddle].textureDimensions.X : TileData[Position.kLeft].textureDimensions.X;

            Create(ref previousPosition, width - 1);
        }

        private bool ShouldCreate(float bound)
        {
            return MathUtils.GenerateFloat(0, 1) <= bound;
        }

        private UIObject Create(ref Vector2 previousPosition, int index)
        {
            if (!CurrentObjectExists)
            {
                levelObjects[index] = null;
                previousPosition += new Vector2(spacing.X, 0);
            }
            else
            {
                float xDelta = PreviousObjectExists ? TileData[Position.kMiddle].textureDimensions.X : TileData[Position.kLeft].textureDimensions.X;
                float yDelta = 0;

                if (!PreviousObjectExists && MathUtils.GenerateFloat(0, 1) <= surfaceHeightFrequency)
                {
                    float yIncrease = TileData[Position.kMiddle].textureDimensions.Y;
                    float amount = MathUtils.GenerateInt(-maximumHeightChange, maximumHeightChange);

                    yDelta = amount * TileData[Position.kMiddle].textureDimensions.Y;
                }

                levelObjects[index] = LevelScreen.AddEnvironmentObject(new Image(previousPosition + new Vector2(xDelta, 0), QueryTextureAsset()), true, true) as Image;
                previousPosition += new Vector2(xDelta, yDelta);
            }

            return levelObjects[index];
        }

        private void ShiftExistenceFlags()
        {
            PreviousObjectExists = CurrentObjectExists;
            CurrentObjectExists = NextObjectExists;
            NextObjectExists = ShouldCreate(surfaceWidthFrequency);
        }

        private string QueryTextureAsset()
        {
            if (PreviousObjectExists && NextObjectExists)
            {
                return TileData[Position.kMiddle].textureAsset;
            }
            else if (PreviousObjectExists && !NextObjectExists)
            {
                return TileData[Position.kRight].textureAsset;
            }
            else if (!PreviousObjectExists && NextObjectExists)
            {
                return TileData[Position.kLeft].textureAsset;
            }
            else
            {
                return TileData[Position.kMiddle].textureAsset;
            }
        }
    }
}

