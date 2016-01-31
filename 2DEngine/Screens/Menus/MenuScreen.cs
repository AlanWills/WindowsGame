using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        /// Going to need to think this through - currently ScreenManager removes it from active screens on transitioning
        /// </summary>
        //private MenuScreen PreviousMenuScreen { get; set; }

        #endregion

        public MenuScreen()
        {
            Camera.SetFixed(Vector2.Zero);
        }

        #region Virtual Functions

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // Check to see if we should update
            if (!ShouldHandleInput) { return; }

            if (GameKeyboard.IsKeyPressed(Keys.Escape))
            {
                // Transition(PreviousMenuScreen);
            }
        }

        #endregion
    }
}
