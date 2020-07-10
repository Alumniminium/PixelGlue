using Pixel.ECS.Components;
using Pixel.Scenes;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Walk
    {
        public static void Handle(MsgWalk packet)
        {
            var uniqueId = packet.UniqueId;
            var tickCount = packet.TickCount;
            var scene = SceneManager.ActiveScene;
            if (scene.UniqueIdToEntityId.TryGetValue(uniqueId, out var entityId))
            {
                var entity = scene.Entities[entityId];
                ref var dst = ref entity.Get<DestinationComponent>();                
                dst.Value = packet.Position;
            }
        }
    }
}