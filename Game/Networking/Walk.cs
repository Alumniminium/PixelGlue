using PixelGlueCore.ECS.Components;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using TerribleSockets.Packets;
using PixelGlueCore.Scenes;
using PixelGlueCore.Entities;

namespace PixelGlueCore.Networking.Handlers
{
    public static class Walk
    {
        public static void Handle(MsgWalk packet)
        {
            var uniqueId = packet.UniqueId;
            var location = new Vector2(packet.X, packet.Y);
            var tickCount = packet.TickCount;
            var scene = SceneManager.ActiveScenes[^1];

            if (!scene.Entities.TryGetValue(uniqueId, out var entity))
            {
                var srcEntity = Database.Entities[PixelGlue.Random.Next(0, Database.Entities.Count)];
                var drawable = new DrawableComponent(uniqueId, srcEntity.TextureName, srcEntity.SrcRect);
                var position = new PositionComponent(uniqueId, packet.X, packet.Y, 0);
                var movable = new MoveComponent(uniqueId, 50, packet.X, packet.Y);

                scene.CreateEntity<PixelEntity>(uniqueId);
                scene.AddDrawable(drawable);
                scene.AddMovable(movable);
                scene.AddPosition(position);
            }
            else
            {
                if (!entity.HasMoveComponent())
                    return;
                ref var movable = ref scene.GetMoveComponentRef(uniqueId);
                if (movable.UniqueId == packet.UniqueId)
                    movable.Destination = location;
            }
        }
    }
}