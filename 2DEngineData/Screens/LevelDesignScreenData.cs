using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace _2DEngineData
{
    public struct LevelObjectData
    {
        public string TextureAsset;
        public Vector2 Position;
        public float Rotation;
        public bool Collision;
    }

    public class LevelDesignScreenData : BaseScreenData
    {
        /// <summary>
        /// The data for the normal tiles in our level.
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<LevelObjectData> NormalTiles { get; set; }

        /// <summary>
        /// The data for the collision tiles in our level.
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<LevelObjectData> CollisionTiles { get; set; }

        /// <summary>
        /// The data for the normal decals in our level.
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<LevelObjectData> NormalDecals { get; set; }

        /// <summary>
        /// The data for the collision decals in our level
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<LevelObjectData> CollisionDecals { get; set; }
    }
}
