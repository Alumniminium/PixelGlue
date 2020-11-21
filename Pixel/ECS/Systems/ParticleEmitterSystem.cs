using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Helpers;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class ParticleEmitterSystem : PixelSystem
    {
        public override string Name { get; set; } = "Particle System";
        public ParticleEmitterSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent>() && entity.Has<ParticleEmitterComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var pos = ref entity.Get<PositionComponent>();
                ref var pem = ref entity.Get<ParticleEmitterComponent>();

                if (!pem.Active)
                    continue;

                if (pem.Particles < pem.MaxParticles)
                {
                    for (int i = 0; i < pem.SpawnFrequency; i++)
                    {
                        ref var particle = ref World.CreateEntity();
                        ref var particle_pos = ref particle.Add(new PositionComponent(pos.Value));
                        ref var particel_drw = ref particle.Add(new DrawableComponent(Color.Red, new Rectangle((int)particle_pos.Value.Y, (int)particle_pos.Value.Y, 4, 4)));
                        ref var particle_vel = ref particle.Add(new VelocityComponent());
                        ref var particle_par = ref particle.Add(new ParticleComponent());
                        particle_par.FramesLeftToLive = pem.LifetimeFrames;
                        particle_par.Energy = 1f;
                        particle_par.EmitterId = entity.EntityId;

                        switch (pem.EmitterType)
                        {
                            case EmitterType.Up:
                                particle_vel.Value = new Vector2(PxlRng.Get(-0.01, 0.01), PxlRng.Get(-0.03, -0.01));
                                break;
                            case EmitterType.Down:
                                particle_vel.Value = new Vector2(PxlRng.Get(-0.01, 0.01), PxlRng.Get(0.01, 0.03));
                                break;
                            case EmitterType.Left:
                                particle_vel.Value = new Vector2(PxlRng.Get(-0.03, -0.01), PxlRng.Get(-0.01, 0.01));
                                break;
                            case EmitterType.Right:
                                particle_vel.Value = new Vector2(PxlRng.Get(0.01, 0.03), PxlRng.Get(-0.01, 0.01));
                                break;
                            case EmitterType.Sphere:
                                particle_vel.Value = new Vector2(PxlRng.Get(-0.03, 0.03), PxlRng.Get(-0.03, -0.03));
                                break;
                        }
                        World.Register(particle.EntityId);
                        pem.Particles++;
                    }
                }
            }
        }
    }
}