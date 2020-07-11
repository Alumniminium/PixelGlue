using System;
using System.Runtime.CompilerServices;
using Pixel.ECS.Components;
using Shared.ECS;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Walk
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Handle(MsgWalk packet)
        {
            var uniqueId = packet.UniqueId;
            var tickCount = packet.TickCount;
            
            if (World.UniqueIdToEntityId.TryGetValue(uniqueId, out var entityId))
            {
                var entity = World.Entities[entityId];
                ref var dst = ref entity.Get<DestinationComponent>();
                
                if (tickCount + 10000 * 1000 * 5 != DateTime.UtcNow.Ticks)
                    dst.Value = packet.Position;
            }
        }
    }
}