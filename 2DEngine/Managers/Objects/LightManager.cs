using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// A class to manager our game's lights and lighting effects.
    /// Use ShouldDraw to determine whether we should use lights or not.
    /// </summary>
    public class LightManager : ObjectManager<Light>
    {
        #region Properties and Fields

        /// <summary>
        /// The effect we will use to draw our lighting.
        /// </summary>
        public Effect LightEffect { get; private set; }

        private const string defaultLightEffect = "Effects\\LightEffect";

        #endregion

        public LightManager() :
            base()
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Loads the effect for the lighting.
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            LightEffect = AssetManager.GetEffect(defaultLightEffect);
            DebugUtils.AssertNotNull(LightEffect);

            base.LoadContent();
        }

        #endregion
    }
}
