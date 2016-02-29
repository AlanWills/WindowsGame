using _2DEngineData;

namespace GameData
{
    /// <summary>
    /// A class representing all the information for the weapons in our game
    /// </summary>
    public class WeaponData : BaseData
    {
        /// <summary>
        /// The display name of our weapon
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The texture asset of our weapon
        /// </summary>
        public string TextureAsset { get; set; }

        /// <summary>
        /// The damage our weapon causes
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// The number of shots before we have to reload (for grenades this would be 1)
        /// </summary>
        public int MagazineSize { get; set; }

        /// <summary>
        /// The time taken to reload the weapon
        /// </summary>
        public float ReloadTime { get; set; }
    }
}
