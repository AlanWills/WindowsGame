using _2DEngine;

namespace WindowsGame
{
    /// <summary>
    /// The starting screen that we will reach after the splash screen.
    /// </summary>
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(string screenDataAsset = "Content\\Data\\Screens\\MainMenuScreen.xml") :
            base(screenDataAsset)
        {
            AddGameObject(new Player(GetScreenCentre(), "Content\\Data\\Animations\\Hero\\HeroAnimations.xml"));
        }
    }
}
