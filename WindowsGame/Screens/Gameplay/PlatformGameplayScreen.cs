using _2DEngine;
using _2DEngineData;
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
            Player player = new Player(ScreenCentre, "Content\\Data\\Character Data\\Hero.xml");
            AddGameObject(player);
            AddCollisionObject(player);
        }

        #region Virtual Functions

        protected override BaseScreenData LoadScreenData()
        {
            return AssetManager.GetData<LevelDesignScreenData>(ScreenDataAsset);
        }

        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            DeserializeLevel();
            /*emitter = new ParticleEmitter(
                new Vector2(3),
                new Vector2(5, 5),
                new Vector2(3, 3),
                new Vector2(0, -1),
                new Vector2(0.25f, 0.35f),
                Color.Cyan,
                Color.OrangeRed,
                2,
                0.25f,
                0.001f,
                GetScreenCentre(),
                "Sprites\\Effects\\TrialParticle");

            emitter.LoadContent();
            emitter.Initialise();*/
        }

        #endregion

        #region Level Loading Functions

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

        private void DeserializeLevelObject(LevelObjectData data, Vector2 size)
        {
            LevelDesignObject newObject = new LevelDesignObject(size, data.Position, data.TextureAsset);
            newObject.LocalRotation = data.Rotation;
            newObject.UsesCollider = data.Collision;

            AddBackgroundObject(newObject);
        }

        #endregion
    }
}
