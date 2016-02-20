using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A singleton class for managing scripts in for our screens.
    /// </summary>
    public class ScriptManager : ObjectManager<Script>
    {
        #region Properties

        private ScriptManager instance;
        public ScriptManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScriptManager();
                }

                return instance;
            }
        }

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

        #endregion

        /// <summary>
        /// Private constructor to enforce the singleton.
        /// </summary>
        public  ScriptManager() :
            base()
        {
            
        }
    }
}
