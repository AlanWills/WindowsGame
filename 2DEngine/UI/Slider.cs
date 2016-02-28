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
        public Property<float> CurrentValue { get; private set; }

        /// <summary>
        /// The handle for for our slider
        /// </summary>
        private Image SliderHandle { get; set; }

        /// <summary>
        /// Optional info label for this slider
        /// </summary>
        public Label InfoLabel { get; private set; }

        /// <summary>
        /// A label to display the current value of the slider
        /// </summary>
        public Label ValueLabel { get; private set; }

        /// <summary>
        /// An event handler used to call a function when the slider's value is changed.
        /// Useful for updating other UI, like a label etc.
        /// </summary>
        public event OnSliderValueChanged OnValueChanged;

        public Slider(
            float currentValue,
            string infoText, 
            Vector2 localPosition, 
            string sliderHandleTextureAsset = AssetManager.DefaultSliderHandleTextureAsset,
            string sliderBarTextureAsset = AssetManager.DefaultSliderBarTextureAsset,
            float lifeTime = float.MaxValue) :
            this(0, 1, currentValue, infoText, localPosition, sliderHandleTextureAsset, sliderBarTextureAsset, lifeTime)
        {

        }

        public Slider(
            float minValue,
            float maxValue,
            float currentValue,
            string infoText,
            Vector2 localPosition,
            string sliderHandleTextureAsset = AssetManager.DefaultSliderHandleTextureAsset,
            string sliderBarTextureAsset = AssetManager.DefaultSliderBarTextureAsset,
            float lifeTime = float.MaxValue) :
            base(localPosition, sliderBarTextureAsset, lifeTime)
        {
            // We do not want to use collisions ourself, but rather for our handle
            UsesCollider = false;

            // Our bar will not have any collision detection on it
            SliderHandle = new Image(Vector2.Zero, sliderHandleTextureAsset, lifeTime);
            SliderHandle.SetParent(this);

            // Fix up our label position after all the textures are initialised
            // Parent it to the bar
            InfoLabel = new Label(infoText, Vector2.Zero, AssetManager.DefaultSpriteFontAsset, lifeTime);
            InfoLabel.SetParent(this);

            // Fix up our label position after all the textures are initialised
            // Parent it to the bar
            ValueLabel = new Label(currentValue.ToString(), Vector2.Zero);
            ValueLabel.SetParent(this);

            MaxValue = maxValue;
            MinValue = minValue;
            CurrentValue = new Property<float>(currentValue);
        }

        #region Virtual Functions

        /// <summary>
        /// Loads our bar and handle objects
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            SliderHandle.LoadContent();
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

            SliderHandle.Initialise();
            InfoLabel.Initialise();
            ValueLabel.Initialise();

            base.Initialise();
        }

        /// <summary>
        /// Fix up UI positions now that we have all the sizes etc. for the slider bar texture.
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            // Maps the current value between [0, 1]
            float currentValue = (CurrentValue.Value - MinValue) / (MaxValue - MinValue);
            SliderHandle.LocalPosition = new Vector2((currentValue - 0.5f) * Size.X, 0);

            float padding = 5f;
            InfoLabel.LocalPosition = new Vector2(-(Size.X * 0.5f + InfoLabel.Size.X * 0.5f + SliderHandle.Size.X * 0.5f + padding), 0);
            ValueLabel.LocalPosition = new Vector2(Size.X * 0.5f + InfoLabel.Size.X * 0.5f + SliderHandle.Size.X * 0.5f + padding, 0);
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

            SliderHandle.HandleInput(elapsedGameTime, mousePosition);

            if (SliderHandle.Collider.IsPressed)
            {
                float sliderBarHalfWidth = Size.X * 0.5f;
                float newX = MathHelper.Clamp(mousePosition.X - WorldPosition.X, -sliderBarHalfWidth, sliderBarHalfWidth);

                SliderHandle.LocalPosition = new Vector2(newX, 0);

                // Calculate our new value
                float multiplier = (newX + sliderBarHalfWidth) / (2 * sliderBarHalfWidth);
                Debug.Assert(multiplier >= 0 && multiplier <= 1);

                CurrentValue.Value = (1 - multiplier) * MinValue + multiplier * MaxValue;

                // Update our value label text
                ValueLabel.Text = CurrentValue.ToString();

                // We do asserts rather than actual clamping, because if these asserts are false the slider is behaving incorrectly
                Debug.Assert(CurrentValue.Value >= MinValue);
                Debug.Assert(CurrentValue.Value <= MaxValue);

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

            SliderHandle.Update(elapsedGameTime);
            InfoLabel.Update(elapsedGameTime);
            ValueLabel.Update(elapsedGameTime);
        }

        /// <summary>
        /// Draws the bar first, then the slider
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            SliderHandle.Draw(spriteBatch);
            InfoLabel.Draw(spriteBatch);
            ValueLabel.Draw(spriteBatch);
        }

        /// <summary>
        /// When the slider dies, kill all objects associated with it - the label and the background bar.
        /// </summary>
        public override void Die()
        {
            base.Die();

            SliderHandle.Die();
            InfoLabel.Die();
            ValueLabel.Die();
        }

        #endregion
    }
}
