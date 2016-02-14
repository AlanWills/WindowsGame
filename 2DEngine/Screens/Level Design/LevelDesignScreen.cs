using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using _2DEngineData;

namespace _2DEngine
{
    /// <summary>
    /// A screen used for creating and serializing levels used for our game.
    /// Will handle collisions tiles, decals and start points.
    /// Can be overridden to create a custom screen for a more custom level output.
    /// </summary>
    public class LevelDesignScreen : GameplayScreen
    {
        protected enum LevelDesignType
        {
            kNormalTile,        // Used for general environment but without collider
            kCollisionTile,     // Used for general environment but with collider
            kNormalDecal,       // Used for decals which enrich the level but without collider
            kCollisionDecal,    // Used for decals which enrich the level but with collider

            kNumTypes,
        }

        #region Properties and Fields

        /// <summary>
        /// A dictionary of lists of objects indexed by their type.
        /// When we create new level objects, they will be added to the appropriate list for serialization.
        /// </summary>
        private Dictionary<LevelDesignType, List<LevelDesignObject>> LevelObjects { get; set; }

        /// <summary>
        /// The current type we will use when adding level objects
        /// </summary>
        private LevelDesignType CurrentType { get; set; }

        /// <summary>
        /// A label which we will update with our current type of level object
        /// </summary>
        private Label CurrentTypeLabel { get; set; }

        /// <summary>
        /// A list of available assets to use in our level objects
        /// </summary>
        private List<string> AvailableAssets { get; set; }

        /// <summary>
        /// The current selected object we are going to place
        /// </summary>
        private Image CurrentSelectedObject { get; set; }

        /// <summary>
        /// UI Constants
        /// </summary>
        private Vector2 ButtonSize = new Vector2(32, 32);
        private Vector2 TileSize = new Vector2(128, 128);
        private float ButtonPadding = 8;

        #endregion

        public LevelDesignScreen(string screenDataAsset) :
            base(screenDataAsset)
        {
            LevelObjects = new Dictionary<LevelDesignType, List<LevelDesignObject>>((int)LevelDesignType.kNumTypes);

            for (LevelDesignType type = LevelDesignType.kNormalTile; type < LevelDesignType.kNumTypes; type++)
            {
                LevelObjects[type] = new List<LevelDesignObject>();
            }

            CurrentType = LevelDesignType.kNormalTile;
            CurrentTypeLabel = new Label(GetLabelText(), GetScreenDimensions() * 0.9f);
            AddScreenUIObject(CurrentTypeLabel);

            AvailableAssets = new List<string>()
            {
                "Sprites\\Level\\Tiles\\Acid (1)",
                "Sprites\\Level\\Tiles\\Acid (2)",
                "Sprites\\Level\\Tiles\\BGTile (1)",
                "Sprites\\Level\\Tiles\\BGTile (2)",
                "Sprites\\Level\\Tiles\\BGTile (3)",
                "Sprites\\Level\\Tiles\\BGTile (4)",
                "Sprites\\Level\\Tiles\\BGTile (5)",
                "Sprites\\Level\\Tiles\\BGTile (6)",
                "Sprites\\Level\\Tiles\\Fence (1)",
                "Sprites\\Level\\Tiles\\Fence (2)",
                "Sprites\\Level\\Tiles\\Fence (3)",
                "Sprites\\Level\\Tiles\\Spike",
                "Sprites\\Level\\Tiles\\Tile (1)",
                "Sprites\\Level\\Tiles\\Tile (2)",
                "Sprites\\Level\\Tiles\\Tile (3)",
                "Sprites\\Level\\Tiles\\Tile (4)",
                "Sprites\\Level\\Tiles\\Tile (5)",
                "Sprites\\Level\\Tiles\\Tile (6)",
                "Sprites\\Level\\Tiles\\Tile (7)",
                "Sprites\\Level\\Tiles\\Tile (8)",
                "Sprites\\Level\\Tiles\\Tile (9)",
                "Sprites\\Level\\Tiles\\Tile (10)",
                "Sprites\\Level\\Tiles\\Tile (11)",
                "Sprites\\Level\\Tiles\\Tile (12)",
                "Sprites\\Level\\Tiles\\Tile (13)",
                "Sprites\\Level\\Tiles\\Tile (14)",
                "Sprites\\Level\\Tiles\\Tile (15)",
                "Sprites\\Level\\Objects\\Barrel (1)",
                "Sprites\\Level\\Objects\\Barrel (2)",
                "Sprites\\Level\\Objects\\Box",
                "Sprites\\Level\\Objects\\DoorLocked",
                "Sprites\\Level\\Objects\\DoorOpen",
                "Sprites\\Level\\Objects\\DoorUnlocked",
                "Sprites\\Level\\Objects\\Saw",
                "Sprites\\Level\\Objects\\Switch (1)",
                "Sprites\\Level\\Objects\\Switch (2)",
            };
        }

        #region Virtual Functions

        /// <summary>
        /// Adds the buttons for the available assets and loads our level
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            int row = 1;
            int buttonsOnRow = (int)(GetScreenDimensions().X / (ButtonSize.X + ButtonPadding));
            int currentButton = 1;
            for (int i = 0; i < AvailableAssets.Count; i++)
            {
                ClickableImage assetImage = new ClickableImage(ButtonSize, new Vector2(currentButton * (ButtonSize.X + ButtonPadding), ButtonSize.Y * row), AvailableAssets[i]);
                assetImage.StoredObject = AvailableAssets[i];
                assetImage.ClickEvent += SetCurrentSelectedObject;
                AddInGameUIObject(assetImage);

                currentButton++;

                if (i > buttonsOnRow)
                {
                    currentButton = 1;
                    row++;
                }
            }

            // Initialise the CurrentSelectedObject to a default rather than checking for whether it is null etc.
            Debug.Assert(AvailableAssets.Count > 0);
            CurrentSelectedObject = new Image(Vector2.Zero, AvailableAssets[0]);
            CurrentSelectedObject.Parent = GameMouse.Instance;
            CurrentSelectedObject.Hide();

            AddScreenUIObject(CurrentSelectedObject);

            Button serializeButton = new Button("Serialize", new Vector2(GetScreenDimensions().X * 0.1f, GetScreenDimensions().Y * 0.9f));
            serializeButton.ClickEvent += SerializeLevel;
            AddScreenUIObject(serializeButton);

            DeserializeLevel();
        }

        protected override BaseScreenData LoadScreenData()
        {
            return AssetManager.GetData<LevelDesignScreenData>(ScreenDataAsset);
        }

        /// <summary>
        /// Handles mouse input for adding/removing objects and 
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        { 
            base.HandleInput(elapsedGameTime, mousePosition);

            if (GameMouse.Instance.IsClicked(MouseButton.kLeftButton))
            {
                if (CurrentSelectedObject.ShouldHandleInput)
                {
                    AddLevelObject();
                }
            }
            else if (GameMouse.Instance.IsClicked(MouseButton.kMiddleButton) || GameKeyboard.IsKeyPressed(Keys.Q))
            {
                CurrentSelectedObject.Hide();
            }

            if (GameKeyboard.IsKeyPressed(Keys.D1))
            {
                CurrentType = LevelDesignType.kNormalTile;
                CurrentTypeLabel.Text = GetLabelText();
            }
            else if (GameKeyboard.IsKeyPressed(Keys.D2))
            {
                CurrentType = LevelDesignType.kCollisionTile;
                CurrentTypeLabel.Text = GetLabelText();
            }
            else if (GameKeyboard.IsKeyPressed(Keys.D3))
            {
                CurrentType = LevelDesignType.kNormalDecal;
                CurrentTypeLabel.Text = GetLabelText();
            }
            else if (GameKeyboard.IsKeyPressed(Keys.D4))
            {
                CurrentType = LevelDesignType.kCollisionDecal;
                CurrentTypeLabel.Text = GetLabelText();
            }
        }

        /// <summary>
        /// Update the game mouse position based on the current type of object we have selected.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (CurrentSelectedObject.ShouldUpdate && (CurrentType == LevelDesignType.kNormalTile || CurrentType == LevelDesignType.kCollisionTile))
            {
                if (GameMouse.Instance.Snapping == false)
                {
                    GameMouse.Instance.SetSnapping(true, TileSize);
                }
            }
            else
            {
                if (GameMouse.Instance.Snapping == true)
                {
                    GameMouse.Instance.SetSnapping(false, Vector2.Zero);
                }
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Changes the object we will be adding to our level.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetCurrentSelectedObject(object sender, EventArgs e)
        {
            ClickableImage image = sender as ClickableImage;
            DebugUtils.AssertNotNull(image);

            RemoveScreenUIObject(CurrentSelectedObject);

            CurrentSelectedObject = new Image(TileSize, Vector2.Zero, (string)image.StoredObject);
            CurrentSelectedObject.StoredObject = image.StoredObject;
            CurrentSelectedObject.Parent = GameMouse.Instance;

            AddScreenUIObject(CurrentSelectedObject, true, true);

            GameMouse.Instance.IsFlushed = true;
        }

        /// <summary>
        /// Creates a new object in our level based on the mouse's current position.
        /// Also adds it to the LevelObjects dictionary.
        /// </summary>
        private void AddLevelObject()
        {
            LevelDesignObject newObject = null;

            // If we have a tile, use TileSize, otherwise use the natural object size
            if (CurrentType == LevelDesignType.kNormalTile || CurrentType == LevelDesignType.kCollisionTile)
            {
                newObject = new LevelDesignObject(TileSize, GameMouse.Instance.InGamePosition, (string)CurrentSelectedObject.StoredObject);
            }
            else
            {
                newObject = new LevelDesignObject(GameMouse.Instance.InGamePosition, (string)CurrentSelectedObject.StoredObject);
            }

            DebugUtils.AssertNotNull(newObject);

            LevelObjects[CurrentType].Add(newObject);
            AddInGameUIObject(newObject, true, true);
        }

        /// <summary>
        /// Gets the appropriate text based on the current type of the objects we will be adding.
        /// </summary>
        /// <returns></returns>
        private string GetLabelText()
        {
            string normalOrCollision = (CurrentType == LevelDesignType.kNormalTile) || (CurrentType == LevelDesignType.kNormalDecal) ? "Normal" : "Collision";
            string tileOrDecal = (CurrentType == LevelDesignType.kNormalTile) || (CurrentType == LevelDesignType.kCollisionTile) ? "Tile" : "Decal";

            return normalOrCollision + " " + tileOrDecal;
        }

        #endregion

        #region Level Serialization and Deserialization

        /// <summary>
        /// Deserializes our Level XML data file and creates all the objects for our level editor.
        /// </summary>
        protected void DeserializeLevel()
        {
            // Load our previously serialized level here
            LevelDesignScreenData levelData = ScreenData.As<LevelDesignScreenData>();
            DebugUtils.AssertNotNull(levelData);

            // Add normal tiles from serialized data
            foreach (LevelObjectData data in levelData.NormalTiles)
            {
                UIObject newObject = DeserializeLevelObject(data, LevelDesignType.kNormalTile, TileSize);
            }

            // Add collision tiles from serialized data
            foreach (LevelObjectData data in levelData.CollisionTiles)
            {
                UIObject newObject = DeserializeLevelObject(data, LevelDesignType.kCollisionTile, TileSize);
            }

            // Add normal decals from serialized data
            foreach (LevelObjectData data in levelData.NormalDecals)
            {
                DeserializeLevelObject(data, LevelDesignType.kNormalDecal, Vector2.Zero);
            }

            // Add collision decals from serialized data
            foreach (LevelObjectData data in levelData.CollisionDecals)
            {
                DeserializeLevelObject(data, LevelDesignType.kCollisionDecal, Vector2.Zero);
            }
        }

        /// <summary>
        /// A function which creates a UIObject for the inputted data and adds it to the appropriate LevelObjects list.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns>Returns the newly created UIObject we have made from the inputted data</returns>
        private LevelDesignObject DeserializeLevelObject(LevelObjectData data, LevelDesignType type, Vector2 size)
        {
            LevelDesignObject newObject = new LevelDesignObject(size, data.Position, data.TextureAsset);
            newObject.LocalRotation = data.Rotation;
            newObject.UsesCollider = data.Collision;

            LevelObjects[type].Add(newObject);
            AddInGameUIObject(newObject);

            return newObject;
        }

        /// <summary>
        /// Serializes all the UIObjects in our level editor into an XML data file.
        /// </summary>
        protected void SerializeLevel(object sender, EventArgs e)
        {
            LevelDesignScreenData levelData = ScreenData.As<LevelDesignScreenData>();
            DebugUtils.AssertNotNull(levelData);

            levelData.ClearAll();

            // Serialize normal tiles to level data
            foreach (LevelDesignObject levelObject in LevelObjects[LevelDesignType.kNormalTile].FindAll(x => x.IsAlive))
            {
                levelData.NormalTiles.Add(SerializeLevelObject(levelObject));
            }

            // Serialize collision tiles to level data
            foreach (LevelDesignObject levelObject in LevelObjects[LevelDesignType.kCollisionTile].FindAll(x => x.IsAlive))
            {
                levelData.CollisionTiles.Add(SerializeLevelObject(levelObject));
            }

            // Serialize normal decals to level data
            foreach (LevelDesignObject levelObject in LevelObjects[LevelDesignType.kNormalDecal].FindAll(x => x.IsAlive))
            {
                levelData.NormalDecals.Add(SerializeLevelObject(levelObject));
            }

            // Serialize collision decals to level data
            foreach (LevelDesignObject levelObject in LevelObjects[LevelDesignType.kCollisionDecal].FindAll(x => x.IsAlive))
            {
                levelData.CollisionDecals.Add(SerializeLevelObject(levelObject));
            }

            // Load our previously serialized level here
            AssetManager.SaveData(levelData, ScreenDataAsset);
        }

        /// <summary>
        /// Serializes an object in our level for our XML.
        /// </summary>
        /// <param name="levelObject">The object we wish to serialize into our XML.</param>
        /// <returns>Returns the XML struct for our data.</returns>
        private LevelObjectData SerializeLevelObject(LevelDesignObject levelObject)
        {
            LevelObjectData objectData = new LevelObjectData();
            objectData.TextureAsset = levelObject.TextureAsset;
            objectData.Position = levelObject.WorldPosition;
            objectData.Rotation = levelObject.WorldRotation;
            objectData.Collision = levelObject.UsesCollider;

            return objectData;
        }

        #endregion
    }
}