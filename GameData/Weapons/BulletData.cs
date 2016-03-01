using _2DEngineData;

namespace GameData
{
    public class BulletData : GameObjectData
    {
        /// <summary>
        /// The damage our weapon causes
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// The speed at which our bullet travels
        /// </summary>
        public float Speed { get; set; }
    }
}
