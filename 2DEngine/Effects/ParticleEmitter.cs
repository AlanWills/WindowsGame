using Microsoft.Xna.Framework;

namespace _2DEngine
{
    /// <summary>
    /// An experimental particle emitter used to test performance
    /// </summary>
    public class ParticleEmitter : ObjectManager<Particle>
    {
        // Start and end size
        // Start and end colour
        // Size variation
        // Life span
        // Life span variation
        // Velocity
        // Emit rate

        #region Properties and Fields

        public Vector2 StartSize { get; set; }
        public Vector2 EndSize { get; set; }
        public Vector2 SizeVariation { get; set; }

        public Vector2 Velocity { get; set; }
        public Vector2 VelocityVariation { get; set; }

        public Color StartColour { get; set; }
        public Color EndColour { get; set; }

        public float ParticleLifeTime { get; set; }
        public float ParticleLifeTimeVariation { get; set; }

        public float EmitTimer { get; set; }

        private Vector2 LocalPosition { get; set; }
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
            base()
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
            LocalPosition = localPosition;
            ParticleTextureAsset = particleTextureAsset;
        }

        private void EmitParticle()
        {
            float extraLifeTime = MathUtils.GenerateFloat(0, ParticleLifeTimeVariation);

            Particle particle = new Particle(StartSize, LocalPosition, ParticleTextureAsset, ParticleLifeTime + extraLifeTime);
            particle.EndSize = EndSize + new Vector2(MathUtils.GenerateFloat(0, SizeVariation.X), MathUtils.GenerateFloat(0, SizeVariation.X));
            particle.Colour = StartColour;
            particle.EndColour = EndColour;
            particle.Velocity = Velocity + new Vector2(MathUtils.GenerateFloat(-VelocityVariation.X, VelocityVariation.X), MathUtils.GenerateFloat(0, VelocityVariation.Y));

            AddObject(particle, true, true);
        }

        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            currentEmitTimer += elapsedGameTime;
            if (currentEmitTimer >= EmitTimer)
            {
                EmitParticle();
                currentEmitTimer = 0;
            }
        }
    }
}