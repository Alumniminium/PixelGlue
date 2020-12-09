using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Helpers;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class ParticleEmitterSystem : PixelSystem<PositionComponent,ParticleEmitterComponent>
    {
        public ParticleEmitterSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name  = "Particle Emitter System"; }
        
        public override void Update(float deltaTime, GCNeutralList<Entity> Entities)
        {
            for(int e =0; e< Entities.Count; e++)
            {
                ref var entity = ref Entities[e];
                ref var pem = ref entity.Get<ParticleEmitterComponent>();

                if (!pem.Active)
                    continue;

                if (pem.Particles < pem.MaxParticles)
                {
                    for (int i = 0; i < pem.SpawnFrequency; i++)
                    {
                        ref readonly var pos = ref entity.Get<PositionComponent>();
                        ref var particle = ref World.CreateEntity();
                        
                        ref readonly var particle_pos = ref particle.Add(new PositionComponent(pos.Value));
                        ref var particel_drw = ref particle.Add(new DrawableComponent(Color.Red, new Rectangle((int)particle_pos.Value.Y, (int)particle_pos.Value.Y, 1, 1)));
                        ref var particle_vel = ref particle.Add(new VelocityComponent());
                        ref var particle_par = ref particle.Add(new ParticleComponent(pem.LifetimeFrames,pem.ParticleStartingEnergy,entity.EntityId));

                        switch (pem.EmitterType)
                        {
                            case EmitterType.Up:
                                particle_vel.Value = new Vector2(PxlRng.Get(-1, 1), PxlRng.Get(-1, -1));
                                break;
                            case EmitterType.Down:
                                particle_vel.Value = new Vector2(PxlRng.Get(-1, 1), PxlRng.Get(1, 3));
                                break;
                            case EmitterType.Left:
                                particle_vel.Value = new Vector2(PxlRng.Get(-3, -1), PxlRng.Get(-1, 1));
                                break;
                            case EmitterType.Right:
                                particle_vel.Value = new Vector2(PxlRng.Get(1, 3), PxlRng.Get(-1, 1));
                                break;
                            case EmitterType.Sphere:
                                particle_vel.Value = new Vector2(PxlRng.Get(-3, 3), PxlRng.Get(-3, -3));
                                break;
                        }
                        particle_vel.Value = (particle_vel.Value * particle_par.Energy) * deltaTime;
                        pem.Particles++;
                    }
                }
            }
        }
    }
}