using _2DEngine;
using _2DEngineData;
using LevelEditorData;
using Microsoft.Xna.Framework;

namespace WindowsGame
{
    /// <summary>
    /// Our main gameplay screen which is predescribed in our level data XML
    /// </summary>
    public class PlatformGameplayScreen : GameplayScreen
    {
        #region Properties and Fields

        private Vector2 TileSize = new Vector2(128, 128);

        #endregion

        public PlatformGameplayScreen(string levelDataAsset) :
            base(levelDataAsset)
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Load this screen's data as LevelDesignScreenData.
        /// </summary>
        /// <returns></returns>
        protected override BaseScreenData LoadScreenData()
        {
            return AssetManager.GetData<LevelDesignScreenData>(ScreenDataAsset);
        }

        /// <summary>
        /// Deserialize our level and add the appropriate background/UI objects
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            DeserializeLevel();
        }

        /// <summary>
        /// Add our initial game objects to the scene.
        /// </summary>
        protected override void AddInitialGameObjects()
        {
            base.AddInitialGameObjects();

            Player player = AddGameObject(new Player(ScreenCentre, "Content\\Data\\Character Data\\Hero.xml")) as Player;
            player.Name = "Hero";
            AddCollisionObject(player);
        }

        /// <summary>
        /// Add our initial game objects to the scene.
        /// </summary>
        protected override void AddInitialLights()
        {
            base.AddInitialLights();

            // Not being added at the moment
            PointLight pointLight = new PointLight(new Vector2(1000, 1000), Vector2.Zero, Color.Red);
            pointLight.Parent = FindGameObject<GameObject>("Hero");
        }

        #endregion

        #region Level Loading Functions

        /// <summary>
        /// Deserialize the LevelDesignScreenData.
        /// </summary>
        private void DeserializeLevel()
        {
            LevelDesignScreenData levelData = ScreenData.As<LevelDesignScreenData>();

            foreach (LevelObjectData levelObjectData in levelData.NormalTiles)
            {
                DeserializeLevelObject(levelObjectData, TileSize);
            }

            foreach (LevelObjectData levelObjectData in levelData.CollisionTiles)
            {
                DeserializeLevelObject(levelObjectData, TileSize);
            }

            foreach (LevelObjectData levelObjectData in levelData.NormalDecals)
            {
                DeserializeLevelObject(levelObjectData, TileSize);
            }

            foreach (LevelObjectData levelObjectData in levelData.CollisionDecals)
            {
                DeserializeLevelObject(levelObjectData, TileSize);
            }
        }

        /// <summary>
        /// Create a background object for our level using the XML data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        private void DeserializeLevelObject(LevelObjectData data, Vector2 size)
        {
            Image newObject = AddEnvironmentObject(new Image(size, data.Position, data.TextureAsset)) as Image;
            newObject.LocalRotation = data.Rotation;
            newObject.UsesCollider = data.Collision;
        }

        #endregion
    }
}
