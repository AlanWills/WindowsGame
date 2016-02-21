using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A delegate used for events that are triggered when a slider has it's value changed.
    /// </summary>
    public delegate void OnSliderValueChanged(Slider slider);

    /// <summary>
    /// A UIObject representing a slider we can use to adjust a float between two limits.
    /// </summary>
    public class Slider : UIObject
    {
        /// <summary>
        /// The maximum value for our slider
        /// </summary>
        private float MaxValue { get; set; }

        /// <summary>
        /// The minimum value for our slider
        /// </summary>
        private float MinValue { get; set; }

        /// <summary>
        /// The current value of our slider
        /// </summary>
        public float CurrentValue { get; private set; }

        /// <summary>
        /// The background bar for our slider
        /// </summary>
        private Image SliderBar { get; set; }

        /// <summary>
        /// Optional info label for this slider
        /// </summary>
        private Label InfoLabel { get; set; }

        /// <summary>
        /// A label to display the current value of the slider
        /// </summary>
        private Label ValueLabel { get; set; }

        /// <summary>
        /// An event handler used to call a function when the slider's value is changed.
        /// Useful for updating other UI, like a label etc.
        /// </summary>
        public event OnSliderValueChanged OnValueChanged;

        public Slider(
            string text, 
            Vector2 localPosition, 
            string sliderHandleTextureAsset = AssetManager.DefaultSliderHandleTextureAsset,
            string sliderBarTextureAsset = AssetManager.DefaultSliderBarTextureAsset,
            float lifeTime = float.MaxValue) :
            this(0, 1, 0.5f, text, localPosition, sliderHandleTextureAsset, sliderBarTextureAsset, lifeTime)
        {

        }

        public Slider(
            float minValue,
            float maxValue,
            float currentValue,
            string text,
            Vector2 localPosition,
            string sliderHandleTextureAsset = AssetManager.DefaultSliderHandleTextureAsset,
            string sliderBarTextureAsset = AssetManager.DefaultSliderBarTextureAsset,
            float lifeTime = float.MaxValue) :
            base(localPosition, sliderHandleTextureAsset, lifeTime)
        {
            // Our bar will not have any collision detection on it
            SliderBar = new Image(localPosition, sliderBarTextureAsset, lifeTime);
            SliderBar.UsesCollider = false;

            // Fix up our label position after all the textures are initialised
            // Parent it to the bar
            InfoLabel = new Label(text, Vector2.Zero, AssetManager.DefaultSpriteFontAsset, lifeTime);
            InfoLabel.Parent = SliderBar;

            // Fix up our label position after all the textures are initialised
            // Parent it to the bar
            ValueLabel = new Label(currentValue.ToString(), Vector2.Zero);
            ValueLabel.Parent = SliderBar;

            MaxValue = maxValue;
            MinValue = minValue;
            CurrentValue = currentValue;
        }

        #region Virtual Functions

        /// <summary>
        /// Loads our bar and handle objects
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            SliderBar.LoadContent();
            InfoLabel.LoadContent();
            ValueLabel.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Initialises our bar and handle objects
        /// </summary>
        public override void Initialise()
        {
            CheckShouldInitialise();

            SliderBar.Initialise();
            InfoLabel.Initialise();
            ValueLabel.Initialise();

            // Maps the current value between [0, 1]
            float currentValue = (CurrentValue - MinValue) / (MaxValue - MinValue);
            LocalPosition += new Vector2((currentValue - 0.5f) * SliderBar.Size.X, 0);

            float padding = 5f;
            InfoLabel.LocalPosition = new Vector2(-(SliderBar.Size.X * 0.5f + InfoLabel.Size.X * 0.5f + padding), 0);
            ValueLabel.LocalPosition = new Vector2(SliderBar.Size.X * 0.5f + InfoLabel.Size.X * 0.5f + padding, 0);

            base.Initialise();
        }

        /// <summary>
        /// Handles the behaviour we want from this UI - to move with the mouse when it is pressed over it, and stop when released.
        /// Then updates the value if needs be.
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (Collider.IsPressed)
            {
                float sliderBarHalfWidth = SliderBar.Size.X * 0.5f;
                float newX = MathHelper.Clamp(mousePosition.X, SliderBar.WorldPosition.X - sliderBarHalfWidth, SliderBar.WorldPosition.X + sliderBarHalfWidth);

                LocalPosition = new Vector2(newX, LocalPosition.Y);

                // Calculate our new value
                CurrentValue = MinValue + (newX - (SliderBar.WorldPosition.X - sliderBarHalfWidth)) / (2 * sliderBarHalfWidth);

                // Update our value label text
                ValueLabel.Text = CurrentValue.ToString();

                // We do asserts rather than actual clamping, because if these asserts are false the slider is behaving incorrectly
                Debug.Assert(CurrentValue >= MinValue);
                Debug.Assert(CurrentValue <= MaxValue);

                if (OnValueChanged != null)
                {
                    OnValueChanged(this);
                }
            }
        }

        /// <summary>
        /// Updates the handle and bar
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            SliderBar.Update(elapsedGameTime);
            InfoLabel.Update(elapsedGameTime);
            ValueLabel.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draws the bar first, then the slider
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            SliderBar.Draw(spriteBatch);

            base.Draw(spriteBatch);

            InfoLabel.Draw(spriteBatch);
            ValueLabel.Draw(spriteBatch);
        }

        /// <summary>
        /// When the slider dies, kill all objects associated with it - the label and the background bar.
        /// </summary>
        public override void Die()
        {
            base.Die();

            SliderBar.Die();
            InfoLabel.Die();
            ValueLabel.Die();
        }

        #endregion
    }
}
