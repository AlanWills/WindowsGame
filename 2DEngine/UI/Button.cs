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
        public Label Label { get; private set; }

        #endregion

        public Button(string buttonText, Vector2 localPosition, string textureAsset = AssetManager.DefaultButtonTextureAsset, float lifeTime = float.MaxValue) :
            this(buttonText, Vector2.Zero, localPosition, textureAsset, lifeTime)
        {

        }

        public Button(string buttonText, Vector2 size, Vector2 localPosition, string textureAsset = AssetManager.DefaultButtonTextureAsset, float lifeTime = float.MaxValue) :
            base(size, localPosition, textureAsset, lifeTime)
        {
            // Create the label in the centre of the button
            Label = new Label(buttonText, Vector2.Zero);
            Label.SetParent(this);

            DefaultColour = Color.Black;
            HighlightedColour = Color.DarkGray;
            Colour = DefaultColour;
        }

        #region Virtual Functions

        /// <summary>
        /// Loads the button text and button
        /// </summary>
        public override void LoadContent()
        {
            // Checks to see if we should load
            CheckShouldLoad();

            // Loads the button text
            Label.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises the button text and button
        /// </summary>
        public override void Initialise()
        {
            // Checks to see if we should initialise
            CheckShouldInitialise();

            // Initialises the button text
            Label.Initialise();

            base.Initialise();
        }

        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

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

            // Lerp back to the default colour - gives the effect of highlighted -> original colour over time
            Colour = Color.Lerp(Colour, DefaultColour, elapsedGameTime * 3);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Label.Draw(spriteBatch);
        }

        #endregion
    }
}