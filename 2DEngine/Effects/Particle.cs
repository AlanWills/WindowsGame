using Microsoft.Xna.Framework;

namespace _2DEngine
{
    public class Particle : UIObject
    {
        public Vector2 EndSize { private get; set; }
        public Color EndColour { private get; set; }
        public Vector2 Velocity { private get; set; }

        public Particle(Vector2 startSize, Vector2 localPosition, string textureAsset, float lifeTime) :
            base(startSize, localPosition, textureAsset, lifeTime)
        {
            
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Colour = Color.Lerp(Colour, EndColour, elapsedGameTime);
            Size = Vector2.Lerp(Size, EndSize, elapsedGameTime);

            LocalPosition += Velocity;
        }
    }
}
