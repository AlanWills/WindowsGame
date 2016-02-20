namespace _2DEngine
{
    /// <summary>
    /// A singleton class for managing scripts in for our screens.
    /// </summary>
    public class ScriptManager : ObjectManager<Script>
    {
        #region Properties

        /// <summary>
        /// Our ScriptManager singleton.
        /// </summary>
        private static ScriptManager instance;
        public static ScriptManager Instance
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

        #endregion

        /// <summary>
        /// Private constructor to enforce the singleton.
        /// </summary>
        public  ScriptManager() :
            base()
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a script and sets the it's ParentScreen property to the current screen if not currently set.
        /// </summary>
        /// <param name="objectToAdd"></param>
        /// <param name="load"></param>
        /// <param name="initialise"></param>
        /// <returns></returns>
        public override Script AddObject(Script objectToAdd, bool load = false, bool initialise = false)
        {
            if (objectToAdd.ParentScreen == null)
            {
                objectToAdd.ParentScreen = ScreenManager.Instance.CurrentScreen;
            }

            return base.AddObject(objectToAdd, load, initialise);
        }

        #endregion
    }
}
