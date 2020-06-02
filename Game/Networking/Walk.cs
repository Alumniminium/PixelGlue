using PixelGlueCore.ECS.Components;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using System;
using TerribleSockets.Packets;
using PixelGlueCore.ECS;

namespace PixelGlueCore.Networking.Handlers
{
    public static class Walk
    {
        static Random Random = new Random();
        public static void Handle(MsgWalk packet, Scene scene)
        {
            var uniqueId = packet.UniqueId;
            var location = new Vector2(packet.X, packet.Y);
            var tickCount = packet.TickCount;

            if (!scene.GameObjects.TryGetValue(uniqueId, out var entity))
            {
                var srcEntity = Database.Entities[Random.Next(0, Database.Entities.Count)];
                var newEntity = new Npc(srcEntity, packet.X, packet.Y);
                newEntity.UniqueId = uniqueId;
                scene.GameObjects.Add(newEntity.UniqueId, newEntity);
            }
            else
            {
                if (entity.TryGetComponent<MoveComponent>(out var movable))
                    movable.Destination = location;
            }
        }
    }
}