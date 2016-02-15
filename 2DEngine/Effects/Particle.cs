using Microsoft.Xna.Framework;

namespace _2DEngine
{
    /// <summary>
    /// A class used in the ParticleEmitter which handles size and colour changing
    /// </summary>
    public class Particle : UIObject
    {
        public Vector2 EndSize { private get; set; }
        public Color EndColour { private get; set; }
        public Vector2 Velocity { private get; set; }

        public Particle(Vector2 startSize, Vector2 localPosition, string textureAsset, float lifeTime) :
            base(startSize, localPosition, textureAsset, lifeTime)
        {
            
        }

        #region Virtual Functions

        /// <summary>
        /// Updates the size, colour and position of our particle
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            Colour = Color.Lerp(Colour, EndColour, elapsedGameTime);
            Size = Vector2.Lerp(Size, EndSize, elapsedGameTime);

            LocalPosition += Velocity;
        }

        #endregion
    }
}
