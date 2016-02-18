using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// A class which corresponds to a light which will affect our entire game scene
    /// </summary>
    public class AmbientLight : Light
    {
        public AmbientLight(Color colour, float intensity = 1, float lifeTime = float.MaxValue) :
            base(ScreenManager.Instance.ScreenCentre, colour, "", intensity, lifeTime)
        {

        }

        #region Virtual Functions

        public override void LoadContent()
        {
            CheckShouldLoad();

            ShouldLoad = false;
        }

        public override void Initialise()
        {
            CheckShouldInitialise();

            ShouldInitialise = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        #endregion
    }
}
