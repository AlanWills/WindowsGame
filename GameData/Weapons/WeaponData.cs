using _2DEngineData;

namespace GameData
{
    /// <summary>
    /// A class representing all the information for the weapons in our game
    /// </summary>
    public class WeaponData : GameObjectData
    {
        /// <summary>
        /// The full path for the bullet XML data file
        /// </summary>
        public string BulletDataAsset { get; set; }

        /// <summary>
        /// The full path for a sprite UI thumbnail used in our game HUD
        /// </summary>
        public string BulletThumbnailTextureAsset { get; set; }

        /// <summary>
        /// The display name of our weapon
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The number of shots before we have to reload (for grenades this would be 1)
        /// </summary>
        public int MagazineSize { get; set; }

        /// <summary>
        /// The time between shots (this will be hard coded based on the shoot animation).
        /// </summary>
        public float TimeBetweenShots { get; set; }

        /// <summary>
        /// The time taken to reload the weapon
        /// </summary>
        public float ReloadTime { get; set; }
    }
}
