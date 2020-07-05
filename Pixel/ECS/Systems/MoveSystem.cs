using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using PixelShared;
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
            foreach (var entity in CompIter<VelocityComponent, DestinationComponent,PositionComponent>.Get())
            {
                ref var vc = ref ComponentArray<VelocityComponent>.Get(entity);
                ref var pc = ref ComponentArray<PositionComponent>.Get(entity);
                ref var dc = ref ComponentArray<DestinationComponent>.Get(entity);

                if (dc.Value != pc.Value)
                {
                    var distanceToDest = Vector2.Distance(pc.Value, dc.Value);
                    var moveDistance = Vector2.Distance(pc.Value, pc.Value + vc.Velocity);

                    if (distanceToDest > moveDistance)
                        pc.Value += vc.Velocity;
                    else
                    {
                        pc.Value = dc.Value;
                        vc.Velocity = Vector2.Zero;
                    }
                    if (SceneManager.ActiveScene.Player.EntityId == entity)
                    {
                        if(!SceneManager.ActiveScene.Player.Has<NetworkComponent>())
                            continue;
                        ref readonly var net = ref ComponentArray<NetworkComponent>.Get(entity);
                        NetworkSystem.Send(MsgWalk.Create(net.UniqueId, pc.Value));
                    }
                }
            }
        }
    }
    public class PlayerMoveSystem : IEntitySystem
    {
        public string Name { get; set; } = "Player Move System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public void FixedUpdate(float _) { }
        public void Update(float dt)
        {
            foreach (var entity in CompIter<InputComponent,PositionComponent, VelocityComponent>.Get())
            {
                ref var inp = ref ComponentArray<InputComponent>.Get(entity);
                ref var dst = ref ComponentArray<DestinationComponent>.Get(entity);
                ref var pos = ref ComponentArray<PositionComponent>.Get(entity);

                if(pos.Value != dst.Value)
                    continue;

                if(inp.Axis.X == 1)
                    dst.Value.X = pos.Value.X + Global.TileSize;
                else if (inp.Axis.X == -1)
                    dst.Value.X = pos.Value.X - Global.TileSize;
                if(inp.Axis.Y == 1)
                    dst.Value.Y = pos.Value.Y + Global.TileSize;
                else if (inp.Axis.Y == -1)
                    dst.Value.Y = pos.Value.Y - Global.TileSize;
            }
        }
    }
}