namespace _2DEngine
{
    /// <summary>
    /// This class is essentially a UI container, except only one will exist - on the gameplay screen.
    /// It can also be overridden for a custom HUD, which can cache useful pieces of UI rather than looking them up.
    /// </summary>
    public class HUD : UIContainer
    {
        #region Properties and Fields

        private static HUD instance;
        public static HUD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HUD();
                }

                return instance;
            }
        }

        #endregion

        public HUD() :
            base(ScreenManager.Instance.ScreenDimensions, ScreenManager.Instance.ScreenCentre)
        {

        }
    }
}
