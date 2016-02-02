using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame
{
    public class Character : GameObject
    {
        protected Animation Animation { get; set; }

        protected override Texture2D Texture
        {
            get
            {
                return Animation.Texture;
            }
        }

        public Character(Vector2 localPosition, string dataAsset) :
            base(localPosition, dataAsset)
        {
            Animation = new Animation("Sprites\\CharacterSpriteSheets\\Hero\\Walk_000_1x16_Resized", 1, 16, 0.05f);
        }

        public override void LoadContent()
        {
            if (!ShouldLoad) { return; }

            Animation.LoadContent();

            base.LoadContent();
        }

        public override void Initialise()
        {
            if (!ShouldInitialise) { return; }

            base.Initialise();

            LocalPosition += new Vector2(0, Size.Y * 0.5f);
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (!ShouldUpdate) { return; }

            Animation.Update(elapsedGameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!ShouldDraw) { return; }

            SourceRectangle = Animation.CurrentSourceRectangle;

            base.Draw(spriteBatch);
        }
    }
}
