using System;
using System.Runtime.CompilerServices;
using Pixel.ECS.Components;
using Shared.ECS;
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
                ref var dst = ref entity.Get<DestinationComponent>();

                dst.Value = packet.Position;
            }
        }
    }
}