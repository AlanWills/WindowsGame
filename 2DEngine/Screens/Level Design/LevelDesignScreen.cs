using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2DEngine
{
    /// <summary>
    /// A screen used for creating and serializing levels used for our game.
    /// Will handle collisions tiles, decals and start points.
    /// Can be overridden to create a custom screen for a more custom level output.
    /// </summary>
    public class LevelDesignScreen : BaseScreen
    {
        protected enum LevelDesignType
        {
            kNormalTile = 1 << 0,
            kCollisionTile = 1 << 1,
            kNormalDecal = 1 << 2,
            kCollisionDecal = 1 << 3,

            kNumTypes,
        }

        #region Properties and Fields

        /// <summary>
        /// A list of the normal tiles in our level
        /// </summary>
        private List<UIObject> NormalTiles { get; set; }

        /// <summary>
        /// A list of the collision tiles in our level
        /// </summary>
        private List<UIObject> CollisionTiles { get; set; }

        /// <summary>
        /// A list of the normal decals in our level
        /// </summary>
        private List<UIObject> NormalDecals { get; set; }

        /// <summary>
        /// A list of the collision decals in our level
        /// </summary>
        private List<UIObject> CollisionDecals { get; set; }

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
        private UIObject CurrentSelectedObject { get; set; }

        private Vector2 ButtonSize = new Vector2(32, 32);
        private float ButtonPadding = 8;

        #endregion

        public LevelDesignScreen(string screenDataAsset = "Content\\Data\\Screens\\LevelDesignScreen.xml") :
            base(screenDataAsset)
        {
            NormalTiles = new List<UIObject>();
            CollisionTiles = new List<UIObject>();
            NormalDecals = new List<UIObject>();
            CollisionDecals = new List<UIObject>();

            CurrentType = LevelDesignType.kNormalTile;
            CurrentTypeLabel = new Label(GetLabelText(), GetScreenDimensions() * 0.9f);

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
        /// Adds the buttons for the available assets
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
        }

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
            else if (GameMouse.Instance.IsClicked(MouseButton.kMiddleButton))
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

        #endregion

        #region Utility Functions

        private void SetCurrentSelectedObject(object sender, EventArgs e)
        {
            ClickableImage image = sender as ClickableImage;
            Debug.Assert(image != null);

            RemoveScreenUIObject(CurrentSelectedObject);

            CurrentSelectedObject = new Image(Vector2.Zero, (string)image.StoredObject);
            CurrentSelectedObject.StoredObject = image.StoredObject;
            CurrentSelectedObject.Parent = GameMouse.Instance;

            AddScreenUIObject(CurrentSelectedObject, true, true);

            GameMouse.Instance.IsFlushed = true;
        }

        private void AddLevelObject()
        {
            AddInGameUIObject(new Image(Camera.ScreenToGameCoords(GameMouse.Instance.WorldPosition), (string)CurrentSelectedObject.StoredObject), true, true);
        }

        private string GetLabelText()
        {
            string normalOrCollision = (CurrentType == LevelDesignType.kNormalTile) || (CurrentType == LevelDesignType.kNormalDecal) ? "Normal" : "Collision";
            string tileOrDecal = (CurrentType == LevelDesignType.kNormalTile) || (CurrentType == LevelDesignType.kCollisionTile) ? "Tile" : "Decal";

            return normalOrCollision + " " + tileOrDecal;
        }

        #endregion
    }
}