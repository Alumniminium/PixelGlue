using System.Numerics;
using Pixel.ECS.Components;
using Shared.ECS;

namespace Server.ECS.Systems
{
    public class MoveSystem : PixelSystem
    {
        public override string Name { get; set; } = "Move System";

        public MoveSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent, VelocityComponent, DestinationComponent, SpeedComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref var pos = ref entity.Get<PositionComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();

                if (pos.Value != dst.Value)
                {
                    ref readonly var spd = ref entity.Get<SpeedComponent>();
                    ref var vel = ref entity.Get<VelocityComponent>();
                    
                    var dir = dst.Value - pos.Value;
                    dir = Vector2.Normalize(dir);

                    vel.Velocity = dir * spd.Speed * spd.SpeedMulti * deltaTime;

                    var distanceToDest = Vector2.Distance(pos.Value, dst.Value);
                    var moveDistance = Vector2.Distance(pos.Value, pos.Value + vel.Velocity);

                    if (distanceToDest > moveDistance)
                        pos.Value += vel.Velocity;
                    else
                    {
                        pos.Value = dst.Value;
                        vel.Velocity = Vector2.Zero;
                        //ref readonly var net = ref entity.Get<NetworkComponent>();
                        //NetworkSystem.Send(MsgWalk.Create(net.UniqueId, pos.Value));
                    }
                }
            }
        }
    }
}