using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using PixelShared.TerribleSockets.Packets;

namespace Pixel.ECS.Systems
{
    public class MoveSystem : IEntitySystem
    {
        public string Name { get; set; } = "Move System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float dt)
        {
            foreach (var entity in CompIter<VelocityComponent, PositionComponent>.Get())
            {
                ref var vc = ref ComponentArray<VelocityComponent>.Get(entity);
                ref var pc = ref ComponentArray<PositionComponent>.Get(entity);

                if (pc.Destination != pc.Position)
                {
                    var dir = pc.Destination - pc.Position;
                    dir.Normalize();

                    vc.Velocity = dir * vc.Speed * vc.SpeedMulti * dt;

                    var distanceToDest = Vector2.Distance(pc.Position, pc.Destination);
                    var moveDistance = Vector2.Distance(pc.Position, pc.Position + vc.Velocity);

                    if (distanceToDest > moveDistance)
                        pc.Position += vc.Velocity;
                    else
                    {
                        pc.Position = pc.Destination;
                        vc.Velocity = Vector2.Zero;
                    }
                    if (SceneManager.ActiveScene.Player.EntityId == entity)
                    {
                        ref readonly var net = ref ComponentArray<NetworkComponent>.Get(entity);
                        NetworkSystem.Send(MsgWalk.Create(net.UniqueId, pc.Position));
                    }
                }
            }
        }
    }
}