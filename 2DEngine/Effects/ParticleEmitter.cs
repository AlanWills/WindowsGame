using Microsoft.Xna.Framework;

namespace _2DEngine
{
    /// <summary>
    /// A particle emitter
    /// </summary>
    public class ParticleEmitter : UIContainer
    {
        // Start and end size
        // Start and end colour
        // Size variation
        // Life span
        // Life span variation
        // Velocity
        // Emit rate

        #region Properties and Fields

        /// <summary>
        /// The beginning size for an emitted particle
        /// </summary>
        public Vector2 StartSize { get; set; }

        /// <summary>
        /// The end size for an emitted particle
        /// </summary>
        public Vector2 EndSize { get; set; }

        /// <summary>
        /// The variation of our end size
        /// </summary>
        public Vector2 SizeVariation { get; set; }

        /// <summary>
        /// The velocity of our particle
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The variation in the velocity of our particle
        /// </summary>
        public Vector2 VelocityVariation { get; set; }

        /// <summary>
        /// The start colour of our particle
        /// </summary>
        public Color StartColour { get; set; }

        /// <summary>
        /// The end colour of our particle
        /// </summary>
        public Color EndColour { get; set; }

        /// <summary>
        /// The life time of a particle
        /// </summary>
        public float ParticleLifeTime { get; set; }

        /// <summary>
        /// The variation in the life time of a particle
        /// </summary>
        public float ParticleLifeTimeVariation { get; set; }

        /// <summary>
        /// The time delay between emitting particles
        /// </summary>
        public float EmitTimer { get; set; }

        /// <summary>
        /// The texture asset for an emitted particle
        /// </summary>
        private string ParticleTextureAsset { get; set; }

        private float currentEmitTimer = 0;

        #endregion

        public ParticleEmitter(
            Vector2 startSize, 
            Vector2 endSize, 
            Vector2 sizeVariation,
            Vector2 velocity,
            Vector2 velocityVariation,
            Color startColour,
            Color endColour,
            float particleLifeTime,
            float particleLifeTimeVariation,
            float emitTimer,
            Vector2 localPosition, 
            string particleTextureAsset) :
            base(localPosition, particleTextureAsset)
        {
            StartSize = startSize;
            EndSize = endSize;
            SizeVariation = sizeVariation;
            Velocity = velocity;
            VelocityVariation = velocityVariation;
            StartColour = startColour;
            EndColour = endColour;
            ParticleLifeTime = particleLifeTime;
            ParticleLifeTimeVariation = particleLifeTimeVariation;
            EmitTimer = emitTimer;
            ParticleTextureAsset = particleTextureAsset;
        }

        /// <summary>
        /// Creates a new particle
        /// </summary>
        private void EmitParticle()
        {
            float extraLifeTime = MathUtils.GenerateFloat(0, ParticleLifeTimeVariation);

            Particle particle = new Particle(StartSize, Vector2.Zero, ParticleTextureAsset, ParticleLifeTime + extraLifeTime);
            particle.EndSize = EndSize + new Vector2(MathUtils.GenerateFloat(0, SizeVariation.X), MathUtils.GenerateFloat(0, SizeVariation.X));
            particle.Colour = StartColour;
            particle.EndColour = EndColour;
            particle.Velocity = Velocity + new Vector2(MathUtils.GenerateFloat(-VelocityVariation.X, VelocityVariation.X), MathUtils.GenerateFloat(0, VelocityVariation.Y));

            AddObject(particle, true, true);
        }

        #region Virtual Functions

        /// <summary>
        /// Emits particles when our timer has reached a certain amount
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            currentEmitTimer += elapsedGameTime;
            while (currentEmitTimer >= EmitTimer)
            {
                EmitParticle();
                currentEmitTimer -= EmitTimer;
            }
        }

        #endregion
    }
}