using System;
using System.Runtime.CompilerServices;
using Pixel.ECS.Components;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Walk
    {
        public static void Handle(MsgWalk packet)
        {
            if (World.UidExists(packet.UniqueId))
            {
                ref readonly var entity = ref World.GetEntityByUniqueId(packet.UniqueId);

                if (entity.Has<DestinationComponent>())
                {
                    ref var dst = ref entity.Get<DestinationComponent>();
                    dst.Value = packet.Position;
                }
                else
                {
                    entity.Add(new DestinationComponent(packet.Position));
                }
            }
        }
    }
}