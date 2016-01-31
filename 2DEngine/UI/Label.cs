using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class for drawing text
    /// </summary>
    public class Label : UIObject
    {
        #region Properties and Fields

        /// <summary>
        /// The text which we will be rendering
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Used in drawing the text.  Corresponds to the centre of the text string.
        /// </summary>
        private Vector2 TextCentre { get { return SpriteFont.MeasureString(Text) * 0.5f; } }

        /// <summary>
        /// Corresponds to the dimensions of the text using the SpriteFont we have set up.
        /// </summary>
        public Vector2 TextDimensions { get { return SpriteFont.MeasureString(Text); } }

        #endregion

        public Label(string text, Vector2 localPosition, string spriteFontAsset = AssetManager.DefaultSpriteFontAsset) :
            base(localPosition, "")
        {

        }

        #region Virtual Functions

        /// <summary>
        /// Sets up the SpriteFont for this text
        /// </summary>
        public override void LoadContent()
        {
            // Check to see if we should load
            if (!ShouldLoad) { return; }

            SetupSpriteFont();

            base.LoadContent();
        }

        /// <summary>
        /// Updates the size of the text if necessary
        /// </summary>
        public override void Initialise()
        {
            // Check to see if we should initialise
            if (!ShouldInitialise) { return; }

            // Change the size to the text dimensions if unset
            if (Size == Vector2.Zero)
            {
                Size = TextDimensions;
            }

            base.Initialise();
        }

        /// <summary>
        /// Draws the text
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for rendering</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Check if we should draw
            if (!ShouldDraw) { return; }

            // Draw the text
            Debug.Assert(SpriteFont != null);
            spriteBatch.DrawString(
                SpriteFont, 
                Text, 
                WorldPosition, 
                Colour * Opacity, 
                WorldRotation, 
                TextCentre, 
                Vector2.Divide(Size, TextDimensions), 
                SpriteEffects.None, 
                0);
        }

        #endregion
    }
}
