﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace _2DEngine
{
    /// <summary>
    /// A class for drawing text.
    /// Has no collider by default.
    /// </summary>
    public class Label : UIObject
    {
        #region Properties and Fields

        /// <summary>
        /// The text which we will be rendering.
        /// Automatically changes the size so the new text will be in the same dimensions.
        /// </summary>
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                // Initially, our text will be null so cannot use the old dimensions to calculate the new ones, so use an indentity vector
                Vector2 oldScale = Vector2.One;
                if (Text != null)
                {
                    oldScale = Vector2.Divide(Size, TextDimensions);
                }
                
                text = value;
                Size = Vector2.Multiply(oldScale, TextDimensions);
            }
        }

        /// <summary>
        /// Used in drawing the text.  Corresponds to the centre of the text string.
        /// </summary>
        private Vector2 TextCentre { get { return SpriteFont.MeasureString(Text) * 0.5f; } }

        /// <summary>
        /// Corresponds to the dimensions of the text using the SpriteFont we have set up.
        /// </summary>
        private Vector2 TextDimensions { get { return SpriteFont.MeasureString(Text); } }

        /// <summary>
        /// An alternative way of changing the size of this text - normally, inputting a size is very hit and miss so a scale just multiplies the TextDimensions
        /// </summary>
        public float Scale { get; set; }

        #endregion

        public Label(string text, Vector2 localPosition, string spriteFontAsset = AssetManager.DefaultSpriteFontAsset, float lifeTime = float.MaxValue) :
            this(1, text, localPosition, "", lifeTime)
        {
            Text = text;

            // Labels do not need a collider
            UsesCollider = false;
        }

        public Label(float scale, string text, Vector2 localPosition, string spriteFontAsset = AssetManager.DefaultSpriteFontAsset, float lifeTime = float.MaxValue) :
            base(localPosition, "", lifeTime)
        {
            Scale = scale;
            Text = text;

            // Labels do not need a collider
            UsesCollider = false;
        }

        #region Virtual Functions

        /// <summary>
        /// We do not need a texture or any other set up that happens from base classes so do not call their LoadContent function
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();

            ShouldLoad = false;
        }

        /// <summary>
        /// Updates the size of the text if necessary
        /// </summary>
        public override void Initialise()
        {
            // Check to see if we should initialise
            CheckShouldInitialise();

            // Change the size to the text dimensions if unset
            if (Size == Vector2.Zero)
            {
                Size = TextDimensions;
            }

            ShouldInitialise = false;
        }

        /// <summary>
        /// Draws the text
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch used for rendering</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the text
            DebugUtils.AssertNotNull(SpriteFont);
            spriteBatch.DrawString(
                SpriteFont, 
                Text, 
                WorldPosition, 
                Colour * Opacity, 
                WorldRotation, 
                TextCentre, 
                Vector2.Multiply(Vector2.Divide(Size, TextDimensions), Scale), 
                SpriteEffects.None, 
                0);
        }

        #endregion
    }
}
