using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using PixelShared;

namespace Pixel.ECS.Systems
{
    public class ParticleSystem : IEntitySystem
    {
        public string Name { get; set; } = "Particle System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float deltaTime)
        {
            foreach(var entity in CompIter<PositionComponent, VelocityComponent,DrawableComponent>.Get())
            {
                ref var pos = ref ComponentArray<PositionComponent>.Get(entity);
                ref readonly var vel = ref ComponentArray<VelocityComponent>.Get(entity);
                ref readonly var drw = ref ComponentArray<DrawableComponent>.Get(entity);
                
                pos.Position += vel.Velocity;
            }
        }
    }
}