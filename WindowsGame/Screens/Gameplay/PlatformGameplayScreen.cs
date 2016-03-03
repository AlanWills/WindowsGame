using _2DEngine;
using _2DEngineData;
using GameData;
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

        /// <summary>
        /// A reference to our playable hero
        /// </summary>
        private Player Hero { get; set; }

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
            return AssetManager.GetData<PlatformGameplayScreenData>(ScreenDataAsset);
        }

        /// <summary>
        /// Add our initial game objects to the scene.
        /// </summary>
        protected override void AddInitialGameObjects()
        {
            base.AddInitialGameObjects();

            Hero = AddGameObject(new Player(ScreenCentre, "Content\\Data\\Character Data\\Hero.xml")) as Player;
            Hero.Name = "Hero";
            AddCollisionObject(Hero);
        }

        /// <summary>
        /// Add our initial game objects to the scene.
        /// </summary>
        protected override void AddInitialLights()
        {
            base.AddInitialLights();

            AddLight(new AmbientLight(Color.White, 0.35f));
        }

        /// <summary>
        /// Deserialize our level and add the appropriate background/UI objects
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            //DeserializeLevel();
            PlatformGameplayScreenData data = ScreenData as PlatformGameplayScreenData;
            DebugUtils.AssertNotNull(data);

            GenerationEngine generationEngine = new GenerationEngine(this, data.LevelGenerationDataAsset);
            generationEngine.GenerateLevel();

            DebugUtils.AssertNotNull(Hero);
            DebugUtils.AssertNotNull(Hero.Weapon);
            HUD.Instance.AddObject(new WeaponStatusUI(Hero.Weapon));
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
