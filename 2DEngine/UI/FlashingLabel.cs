using Microsoft.Xna.Framework;

namespace _2DEngine
{
    /// <summary>
    /// A very simple extension to the Label class which just wraps up oscillating the opacity to give the appearance of flashing
    /// </summary>
    public class FlashingLabel : Label
    {
        #region Properties and Fields

        /// <summary>
        /// A simple bool just to determine whether we are lerping down to 0 or up to 1
        /// </summary>
        private bool flashingOut;

        #endregion

        public FlashingLabel(string text, Vector2 localPosition, string spriteFontAsset = AssetManager.DefaultSpriteFontAsset, float lifeTime = float.MaxValue) :
            base(text, localPosition, spriteFontAsset, lifeTime)
        {
            flashingOut = true;
        }

        public FlashingLabel(float scale, string text, Vector2 localPosition, string spriteFontAsset = AssetManager.DefaultSpriteFontAsset, float lifeTime = float.MaxValue) :
            base(scale, text, localPosition, spriteFontAsset, lifeTime)
        {
            flashingOut = true;
        }

        #region Properties and Fields

        /// <summary>
        /// Implement the flashing here - changing the opacity
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (flashingOut)
            {
                Opacity = MathHelper.Lerp(Opacity, 0, 5 * elapsedGameTime);

                if (Opacity <= 0.1)
                {
                    flashingOut = false;
                }
            }
            else
            {
                Opacity = MathHelper.Lerp(Opacity, 1, 5 * elapsedGameTime);

                if (Opacity >= 0.9)
                {
                    flashingOut = true;
                }
            }
        }

        #endregion
    }
}
