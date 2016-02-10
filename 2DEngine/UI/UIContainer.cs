namespace _2DEngine
{
    /// <summary>
    /// A class which is designed to contain UIObjects.  Useful for Menus and custom UI.
    /// Marked as abstract because we do not want to create an instance of this class since it does not inherit from BaseObject
    /// </summary>
    public abstract class UIContainer : ObjectManager<UIObject>
    {
        #region Properties and Fields

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Adds Initial UI and calls LoadContent on any added objects
        /// </summary>
        public override void LoadContent()
        {
            // Check to see if we should load
            CheckShouldLoad();

            AddInitialUI();

            base.LoadContent();
        }

        /// <summary>
        /// Adds an initial UI to the container
        /// </summary>
        public virtual void AddInitialUI() { }

        #endregion

        #region Utility Functions

        #endregion
    }
}
