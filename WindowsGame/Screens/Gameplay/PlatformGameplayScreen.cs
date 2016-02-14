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
            AddGameObject(new Player(GetScreenCentre(), "Content\\Data\\Animations\\Hero\\HeroAnimations.xml"));
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
