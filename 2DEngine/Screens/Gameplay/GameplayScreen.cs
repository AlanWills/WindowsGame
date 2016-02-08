namespace _2DEngine
{
    /// <summary>
    /// A simple class which contains a HUD.
    /// Really a base class for more custom gameplay screens
    /// </summary>
    public class GameplayScreen : BaseScreen
    {
        // Can't work out whether this should be static or not.
        // How many gameplay screens will we have?
        // protected HUD HUD {get;set;}

        public GameplayScreen(string screenDataAsset) :
            base(screenDataAsset)
        {

        }
    }
}
