using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    /// <summary>
    /// A simple class which represents an image and overlayed text.
    /// Auto fits around the text.
    /// </summary>
    public class DialogBox : UIObject
    {
        #region Properties and Fields

        /// <summary>
        /// The text we wish to display
        /// </summary>
        private Label Text { get; set; }

        /// <summary>
        /// The padding on each side
        /// </summary>
        private const float xPadding = 5;
        private const float yPadding = 5;

        #endregion

        public DialogBox(string text, Vector2 localPosition, string textureAsset = AssetManager.DefaultTextBoxTextureAsset, float lifeTime = float.MaxValue) :
            base(localPosition, textureAsset, lifeTime)
        {
            // Set the text to be in the centre of the text box
            Text = new Label(text, Vector2.Zero);
            Text.SetParent(this);
        }

        #region Virtual Functions

        /// <summary>
        /// Load the text box and text
        /// </summary>
        public override void LoadContent()
        {
            // Check to see if we should load
            CheckShouldLoad();

            Text.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialise the text box and text 
        /// </summary>
        public override void Initialise()
        {
            // Check to see if we should initialise
            CheckShouldInitialise();

            Text.Initialise();

            // Sets the size of the text box to wrap around the text and add padding on all sides
            Size = Text.Size + new Vector2(2 * xPadding, 2 * yPadding);

            base.Initialise();
        }

        /// <summary>
        /// Draw the textbox and text
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Text.Draw(spriteBatch);
        }

        #endregion
    }
}
