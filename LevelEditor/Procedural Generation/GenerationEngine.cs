using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

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
        private UIObject[,] levelObjects;

        private List<Point> walkingLayerPoints = new List<Point>();

        private int width = 20;
        private int height = 10;
        private int currentHeight = 7;

        private float surfaceWidthFrequency = 0.75f;
        private float surfaceHeightFrequency = 0.15f;
        private int maximumHeightChange = 2;
        private Vector2 tileDimensions;

        private Dictionary<Position, string> TileData { get; set; }

        /*
        Improvements :
            have a 2D array with width and height
            when moving up and down, only go up and down based on our current position in the grid
            then we can create extra walking layers and other non-collision layers (possibly in other generation engines)
        */

        public GenerationEngine(GameplayScreen levelScreen)
        {
            LevelScreen = levelScreen;

            TileData = new Dictionary<Position, string>()
            {
                { Position.kLeft, "Sprites\\Level\\Tiles\\Tile (1)" },
                { Position.kMiddle, "Sprites\\Level\\Tiles\\Tile (2)" },
                { Position.kRight, "Sprites\\Level\\Tiles\\Tile (3)" },
            };

            Texture2D tex = AssetManager.GetSprite(TileData[Position.kMiddle]);
            tileDimensions = new Vector2(tex.Width, tex.Height);

            levelObjects = new UIObject[height, width];
        }

        public void GenerateBackground()
        {
            GenerateWalkableLayer();
            GenerateHazards();
        }

        #region Walkable Layer

        private void GenerateWalkableLayer()
        {
            UIObject previousObject = Setup();

            for (int x = 2; x < width - 1; x++)
            {
                ShiftExistenceFlags();
                previousObject = Create(x);
            }

            TakeDown();
        }

        private UIObject Setup()
        {
            Vector2 screenDimensions = ScreenManager.Instance.ScreenDimensions;

            PreviousObjectExists = false;
            CurrentObjectExists = true;
            NextObjectExists = true;

            Create(0);

            Camera.FocusOnPosition(levelObjects[currentHeight, 0].WorldPosition, false);

            PreviousObjectExists = true;
            CurrentObjectExists = true;
            NextObjectExists = ShouldCreate(surfaceWidthFrequency);

            Create(1);

            return levelObjects[currentHeight, 1];
        }

        private void TakeDown()
        {
            PreviousObjectExists = CurrentObjectExists;
            CurrentObjectExists = true;
            NextObjectExists = false;

            Create(width - 1);
        }

        private bool ShouldCreate(float bound)
        {
            return MathUtils.GenerateFloat(0, 1) <= bound;
        }

        private UIObject Create(int xIndex)
        {
            if (!CurrentObjectExists)
            {
                levelObjects[currentHeight, xIndex] = null;
            }
            else
            {
                float yDelta = 0;

                if (MathUtils.GenerateFloat(0, 1) <= surfaceHeightFrequency)
                {
                    int amount = MathUtils.GenerateInt(-maximumHeightChange, maximumHeightChange);

                    currentHeight += amount;
                    currentHeight = MathHelper.Clamp(currentHeight, 0, height - 1);

                    yDelta = amount * tileDimensions.Y;
                }

                levelObjects[currentHeight, xIndex] = LevelScreen.AddEnvironmentObject(new Image(new Vector2(tileDimensions.X * xIndex, tileDimensions.Y * currentHeight), QueryTextureAsset()), true, true) as Image;
                walkingLayerPoints.Add(new Point(xIndex, currentHeight));
            }

            return levelObjects[currentHeight, xIndex];
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
                return TileData[Position.kMiddle];
            }
            else if (PreviousObjectExists && !NextObjectExists)
            {
                return TileData[Position.kRight];
            }
            else if (!PreviousObjectExists && NextObjectExists)
            {
                return TileData[Position.kLeft];
            }
            else
            {
                return TileData[Position.kMiddle];
            }
        }

        #endregion

        #region Hazards

        private void GenerateHazards()
        {
            string hazard1 = "Sprites\\Level\\Tiles\\Acid (1)";
            string hazard2 = "Sprites\\Level\\Tiles\\Spike";

            Debug.Assert(width > 2);

            for (int index = 1; index < walkingLayerPoints.Count - 1; index++)
            {
                Point currentPoint = walkingLayerPoints[index];
                Point nextPoint = walkingLayerPoints[index + 1];

                int diff = nextPoint.X - currentPoint.X - 1;
                // Do max because increasing y is downwards
                int y = MathHelper.Max(nextPoint.Y, currentPoint.Y);

                while (diff > 0)
                {
                    string chosenTexture = MathUtils.GenerateFloat(0, 1) < 0.5f ? hazard1 : hazard2;
                    LevelScreen.AddEnvironmentObject(new Image(new Vector2((nextPoint.X - diff) * tileDimensions.X, y * tileDimensions.Y), chosenTexture), true, true);
                    diff--;
                }
            }
        }

        #endregion
    }
}