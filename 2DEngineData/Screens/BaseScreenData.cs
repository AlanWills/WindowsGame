namespace _2DEngineData
{
    /// <summary>
    /// A class to handle basic data for all screens.
    /// Can be overidden to provide data for custom screens.
    /// </summary>
    public class BaseScreenData : BaseData
    {
        /// <summary>
        /// The path for the background image.  Leave blank if no image is included.
        /// </summary>
        public string BackgroundTextureAsset { get; set; }
    }
}
