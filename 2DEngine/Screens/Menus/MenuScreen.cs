using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace _2DEngine
{
    /// <summary>
    /// A fairly basic class that fixes the camera and sets up transitioning between menus.
    /// </summary>
    public class MenuScreen : BaseScreen
    {
        #region Properties and Fields

        /// <summary>
        /// The previous screen which we will transition to if we press Esc.
        /// </summary>
        private MenuScreen PreviousMenuScreen { get; set; }

        private Type type;

        #endregion

        public MenuScreen(MenuScreen previousMenuScreen, string screenDataAsset) :
            base(screenDataAsset)
        {
            Lights.ShouldDraw = false;
            PreviousMenuScreen = previousMenuScreen;
        }

        #region Virtual Functions

        /// <summary>
        /// Set up the camera to be fixed and resset it to it's default position of zero
        /// </summary>
        public override void Initialise()
        {
            base.Initialise();

            Camera.SetFixed(Vector2.Zero);
        }

        /// <summary>
        /// Handle input and check whether we are transitioning to the previous screen if it exists (press escape)
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (PreviousMenuScreen != null && GameKeyboard.IsKeyPressed(Keys.Escape))
            {
                // Yeah I know - eurgh
                var parameters = new object[2] { this, PreviousMenuScreen.ScreenDataAsset };
                var newScreen = Activator.CreateInstance(PreviousMenuScreen.GetType(), parameters);
                Transition((BaseScreen)newScreen);
            }
        }

        #endregion
    }
}
