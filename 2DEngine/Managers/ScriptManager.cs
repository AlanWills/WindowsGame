using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// A class for managing scripts in a screen
    /// </summary>
    public class ScriptManager : ObjectManager<Script>
    {
        #region Properties

        /// <summary>
        /// Loops through all the scripts. 
        /// If one exists which should not update the game, returns false. Else returns true.
        /// </summary>
        public bool ShouldUpdateGame
        {
            get
            {
                foreach (Script script in ActiveObjects)
                {
                    // If a script exists which should not update the game, we return false
                    if (!script.ShouldUpdateGame) { return false; }
                }

                return true;
            }
        }

        /// <summary>
        /// A reference to the screen that this ScriptManager corresponds to.
        /// </summary>
        private BaseScreen ParentScreen { get; set; }

        #endregion

        /// <summary>
        /// Private constructor to enforce the singleton.
        /// </summary>
        public  ScriptManager(BaseScreen parentScreen) :
            base()
        {
            ParentScreen = parentScreen;
        }

        #region Virtual Functions

        public override void Draw(SpriteBatch spriteBatch)
        {
            // No drawing in scripts
        }

        #endregion

        #region Utility Functions

        public override void AddObject(Script objectToAdd, bool load = false, bool initialise = false)
        {
            if (objectToAdd.ParentScreen == null)
            {
                objectToAdd.ParentScreen = ParentScreen;
            }

            base.AddObject(objectToAdd, load, initialise);
        }

        #endregion
    }
}
