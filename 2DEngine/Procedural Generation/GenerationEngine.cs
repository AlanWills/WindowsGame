using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace _2DEngine
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

        private enum Type
        {
            kWalkableLayer,
            kHazard,
            kBelowWalkableLayer,
            kAboveWalkableLayer
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

        private List<KeyValuePair<Point, Type>> walkingLayerPoints = new List<KeyValuePair<Point, Type>>();

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
                //{ Position.kLeft, "Sprites\\Level\\Tiles\\Tile (1)" },
                { Position.kLeft, "Sprites\\Level\\Tiles\\Tile (2)" },
                { Position.kMiddle, "Sprites\\Level\\Tiles\\Tile (2)" },
                { Position.kRight, "Sprites\\Level\\Tiles\\Tile (2)" },
                //{ Position.kRight, "Sprites\\Level\\Tiles\\Tile (3)" },
            };

            Texture2D tex = AssetManager.GetSprite(TileData[Position.kMiddle]);
            tileDimensions = new Vector2(tex.Width, tex.Height);

            levelObjects = new UIObject[height, width];
        }

        public void GenerateLevel()
        {
            GenerateWalkableLayer();
            GenerateHazards();
            GenerateBackgroundBelowWalkableLayer();
            GenerateBackgroundAboveWalkableLayer();

            AddObjectsInFrontOfBackground();
        }

        private void AddObjectsInFrontOfBackground()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (levelObjects[y, x] != null)
                    {
                        // Do we want to add environment objects for these guys?
                        // Or should it be game objects?
                        LevelScreen.AddEnvironmentObject(levelObjects[y, x]);
                    }
                }
            }
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

            //Camera.FocusOnPosition(levelObjects[currentHeight, 0].WorldPosition, false);

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

                levelObjects[currentHeight, xIndex] = new Image(new Vector2(tileDimensions.X * xIndex, tileDimensions.Y * currentHeight), QueryTextureAsset());
                levelObjects[currentHeight, xIndex].StoredObject = QueryPositionType();

                Point thisPoint = new Point(xIndex, currentHeight);
                Debug.Assert(!walkingLayerPoints.Exists(x => x.Key == thisPoint));
                walkingLayerPoints.Add(new KeyValuePair<Point, Type>(thisPoint, Type.kWalkableLayer));
            }

            return levelObjects[currentHeight, xIndex];
        }

        private void ShiftExistenceFlags()
        {
            PreviousObjectExists = CurrentObjectExists;
            CurrentObjectExists = NextObjectExists;
            NextObjectExists = ShouldCreate(surfaceWidthFrequency);
        }

        private Position QueryPositionType()
        {
            if (PreviousObjectExists && NextObjectExists)
            {
                return Position.kMiddle;
            }
            else if (PreviousObjectExists && !NextObjectExists)
            {
                return Position.kRight;
            }
            else if (!PreviousObjectExists && NextObjectExists)
            {
                return Position.kLeft;
            }
            else
            {
                return Position.kMiddle;
            }
        }

        private string QueryTextureAsset()
        {
            return TileData[QueryPositionType()];
        }

        #endregion

        #region Hazards

        private void GenerateHazards()
        {
            string hazard1 = "Sprites\\Level\\Tiles\\Acid (1)";
            string hazard2 = "Sprites\\Level\\Tiles\\Spike";

            Debug.Assert(width > 2);

            List<KeyValuePair<Point, Type>> tempListOfHazards = new List<KeyValuePair<Point, Type>>();

            for (int index = 0; index < walkingLayerPoints.Count - 1; index++)
            {
                Point currentPoint = walkingLayerPoints[index].Key;
                Point nextPoint = walkingLayerPoints[index + 1].Key;

                int diff = nextPoint.X - currentPoint.X - 1;
                // Do max because increasing y is downwards
                int y = MathHelper.Max(nextPoint.Y, currentPoint.Y);

                // Use the same hazard texture for all the hazards in this block
                string chosenTexture = MathUtils.GenerateFloat(0, 1) < 0.5f ? hazard1 : hazard2;

                while (diff > 0)
                {
                    Point thisPoint = new Point(nextPoint.X - diff, y);

                    levelObjects[y, thisPoint.X] = new Image(new Vector2(thisPoint.X * tileDimensions.X, thisPoint.Y * tileDimensions.Y), chosenTexture);
                    levelObjects[y, thisPoint.X].StoredObject = Position.kMiddle;
                    
                    Debug.Assert(!walkingLayerPoints.Exists(x => x.Key == thisPoint));
                    Debug.Assert(!tempListOfHazards.Exists(x => x.Key == thisPoint));
                    tempListOfHazards.Add(new KeyValuePair<Point, Type>(thisPoint, Type.kHazard));
                    diff--;
                }
            }

            walkingLayerPoints.AddRange(tempListOfHazards);
        }

        #endregion

        #region Background

        private void GenerateBackgroundBelowWalkableLayer()
        {
            Dictionary<Position, string> belowWalkableLayer = new Dictionary<Position, string>()
            {
                //{ Position.kLeft, "Sprites\\Level\\Tiles\\Tile (4)" },
                { Position.kLeft, "Sprites\\Level\\Tiles\\Tile (5)" },
                { Position.kMiddle, "Sprites\\Level\\Tiles\\Tile (5)" },
                { Position.kRight, "Sprites\\Level\\Tiles\\Tile (5)" },
                //{ Position.kRight, "Sprites\\Level\\Tiles\\Tile (6)" },
            };

            for (int index = 0; index < walkingLayerPoints.Count; index++)
            {
                Point point = walkingLayerPoints[index].Key;
                Position positionType = (Position)levelObjects[point.Y, point.X].StoredObject;

                for (int y = point.Y + 1; y < height; y++)
                {
                    UIObject addedObject = LevelScreen.AddEnvironmentObject(new Image(new Vector2(point.X * tileDimensions.X, y * tileDimensions.Y), belowWalkableLayer[positionType]));
                    addedObject.UsesCollider = false;
                }
            }
        }

        private void GenerateBackgroundAboveWalkableLayer()
        {


            add lights





            int backgroundHeight = 4;
            int minY = walkingLayerPoints[0].Key.Y;

            string background = "Sprites\\Level\\Tiles\\BGTile (3)";

            while (walkingLayerPoints.Exists(x => x.Key.Y < minY))
            {
                minY = walkingLayerPoints.Find(x => x.Key.Y < minY).Key.Y;
            }

            for (int index = 0; index < walkingLayerPoints.Count; index++)
            {
                KeyValuePair<Point, Type> info = walkingLayerPoints[index];

                // Want to create a background image behind our hazard so need to increment the currentPoint y by 1 so we start underneath our hazard
                Point currentPoint = info.Key;
                if (info.Value == Type.kHazard)
                {
                    currentPoint.Y++;
                }

                for (int y = currentPoint.Y - 1; y > minY - 1 - backgroundHeight; y--)
                {
                    UIObject addedObject = LevelScreen.AddEnvironmentObject(new Image(new Vector2(currentPoint.X, y) * tileDimensions, background));
                    addedObject.UsesCollider = false;
                }
            }
        }

        #endregion
    }
}