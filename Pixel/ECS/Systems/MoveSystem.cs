using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared.Enums;
using Shared.TerribleSockets.Packets;
using Shared;
using Shared.ECS;
using Pixel.Scenes;

namespace Pixel.ECS.Systems
{
    public class MoveSystem : PixelSystem
    {
        public override string Name { get; set; } = "Move System";
        public Scene Scene => SceneManager.ActiveScene;

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
                ref readonly var spd = ref entity.Get<SpeedComponent>();

                ref var vel = ref entity.Get<VelocityComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
                ref var dst = ref entity.Get<DestinationComponent>();

                if (pos.Value != dst.Value)
                {
                    var dir = dst.Value - pos.Value;
                    dir.Normalize();

                    vel.Value = dir * spd.Speed * spd.SpeedMulti * deltaTime;

                    var distanceToDest = Vector2.Distance(pos.Value, dst.Value);
                    var moveDistance = Vector2.Distance(pos.Value, pos.Value + vel.Value);
                    var keepmoving = false;
                    if (distanceToDest <= moveDistance && entity.Has<KeyboardComponent>())
                    {
                        ref readonly var inp = ref entity.Get<KeyboardComponent>();
                        keepmoving = inp.OldButtons.Contains(PixelGlueButtons.Left) || inp.OldButtons.Contains(PixelGlueButtons.Right) || inp.OldButtons.Contains(PixelGlueButtons.Down) || inp.OldButtons.Contains(PixelGlueButtons.Up);
                        if (entity.Has<NetworkComponent>() && entity.EntityId == Scene.Player.EntityId)
                        {
                            ref readonly var net = ref entity.Get<NetworkComponent>();
                            NetworkSystem.Send(MsgWalk.Create(net.UniqueId, dst.Value));
                        }
                        dst.Value += inp.Axis * Global.TileSize;
                    }

                    if (distanceToDest > moveDistance || keepmoving)
                        pos.Value += vel.Value;
                    else
                    {
                        pos.Value = dst.Value;
                        vel.Value = Vector2.Zero;
                        if (entity.Has<NetworkComponent>() && entity.EntityId == Scene.Player.EntityId)
                        {
                            ref readonly var net = ref entity.Get<NetworkComponent>();
                            NetworkSystem.Send(MsgWalk.Create(net.UniqueId, pos.Value));
                        }
                    }
                }
            }
        }
    }
}