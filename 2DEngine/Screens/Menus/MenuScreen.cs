﻿using Microsoft.Xna.Framework;
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
        /// Going to need to think this through - currently ScreenManager removes it from active screens on transitioning.
        /// Clone it or something?
        /// </summary>
        //private MenuScreen PreviousMenuScreen { get; set; }

        #endregion

        public MenuScreen(string screenDataAsset) :
            base(screenDataAsset)
        {
            Lights.ShouldDraw = false;
        }

        #region Virtual Functions

        public override void Initialise()
        {
            base.Initialise();

            Camera.SetFixed(Vector2.Zero);
        }

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (GameKeyboard.IsKeyPressed(Keys.Escape))
            {
                // Transition(PreviousMenuScreen);
            }
        }

        #endregion
    }
}
