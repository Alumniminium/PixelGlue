using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Scenes;
using Pixel.World;
using PixelShared;
using PixelShared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Walk
    {
        public static void Handle(MsgWalk packet)
        {
            var uniqueId = packet.UniqueId;
            var location = new Vector2(packet.X, packet.Y);
            var tickCount = packet.TickCount;
            var scene = SceneManager.ActiveScene;
            if (!scene.UniqueIdToEntityId.TryGetValue(uniqueId, out var entityId))
            {
                var entity = scene.CreateEntity<Npc>(uniqueId);
            }
            else
            {
                var entity = scene.Entities[entityId];
                ref var position = ref entity.Get<PositionComponent>();
                position.Destination = location;
            }
        }
    }
}