using System.Collections.Generic;
using System.Xml.Serialization;

namespace _2DEngineData
{
    /// <summary>
    /// A class to hold the animation data assets for an animated character.
    /// </summary>
    public class AnimatedCharacterData : GameObjectData
    {
        /// <summary>
        /// The path to the folder for these animations.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// The list of animation data assets that will be used by this character.
        /// Should be the full path - folder path of the xml file describing the animation.
        /// </summary>
        [XmlArrayItem(ElementName = "Item")]
        public List<string> AnimationInfo { get; set; }
    }
}
