using Microsoft.Xna.Framework.Content;

namespace _2DEngine
{
    /// <summary>
    /// A screen to be displayed on start up whilst we load content.
    /// Very simple - just displays a logo and possibly a background.
    /// </summary>
    public class StartupLogoScreen : BaseScreen
    {
        #region Virtual Functions

        /// <summary>
        /// Adds the startup logo.
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            AddScreenUIObject(new Image(GetScreenCentre(), AssetManager.StartupLogoTextureAsset));
        }

        /// <summary>
        /// Loads all the game assets into their managers.
        /// Do this in the update loop so that we have one draw call before loading.
        /// This way the screen UI will be displayed whilst we load.
        /// </summary>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            // Check to see if we should update
            if (!ShouldUpdate) { return; }

            ContentManager content = ScreenManager.Instance.Content;

            AssetManager.LoadAssets(content);
            MusicManager.LoadAssets(content);
            SFXManager.LoadAssets(content);

            Die();
        }

        #endregion
    }
}
