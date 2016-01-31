using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DEngine
{
    public class Button : ClickableImage
    {
        #region Properties and Fields

        /// <summary>
        /// A colour representing the default colour for our button
        /// </summary>
        public Color DefaultColour { get; set; }

        /// <summary>
        /// A colour representing the colour the button will be when highlighted
        /// </summary>
        public Color HighlightedColour { get; set; }

        /// <summary>
        /// A label for any text we wish to draw on top of our button texture
        /// </summary>
        private Label ButtonText { get; set; }

        #endregion

        public Button(string buttonText, Vector2 localPosition, string textureAsset = AssetManager.DefaultButtonTextureAsset, float lifeTime = float.MaxValue) :
            this(buttonText, Vector2.Zero, localPosition, textureAsset, lifeTime)
        {

        }

        public Button(string buttonText, Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultButtonTextureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, textureAsset, lifeTime)
        {
            // Create the label in the centre of the button
            ButtonText = new Label(buttonText, Vector2.Zero);
            ButtonText.Parent = this;
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the button text and button
        /// </summary>
        public override void LoadContent()
        {
            // Checks to see if we should load
            if (!ShouldLoad) { return; }

            // Loads the button text
            ButtonText.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises the button text and button
        /// </summary>
        public override void Initialise()
        {
            // Checks to see if we should initialise
            if (!ShouldInitialise) { return; }

            // Initialises the button text
            ButtonText.Initialise();

            base.Initialise();
        }

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            // Check to see whether we should handle input
            if (!ShouldHandleInput) { return; }

            // Play SFX if we have entered the button
            if (/*have entered*/true)
            {
                //Play enter SFX
            }

            // If the mouse is over, set the colour to the highlighted colour
            if (Collider.IsMouseOver && ClickState != ClickState.kPressed)
            {
                Colour = HighlightedColour;
            }
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            // Check to see whether we should update
            if (!ShouldUpdate) { return; }

            // Lerp back to the default colour - gives the effect of highlighted -> original colour over time
            Colour = Color.Lerp(Colour, DefaultColour, elapsedGameTime * 3);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Check to see whether we should draw
            if (!ShouldDraw) { return; }

            ButtonText.Draw(spriteBatch);
        }

        #endregion
    }
}