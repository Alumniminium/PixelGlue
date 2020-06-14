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
                entity = scene.CreateEntity<PixelEntity>(uniqueId);
                var drawable = new DrawableComponent(entity.EntityId,srcEntity.TextureName, srcEntity.SrcRect);
                var position = new PositionComponent(entity.EntityId,packet.X, packet.Y, 0);
                var movable = new MoveComponent(entity.EntityId,8, packet.X, packet.Y);
                var text = new TextComponent(entity.EntityId,$"{position.Position.X},{position.Position.Y}");

                entity.Add(drawable);
                entity.Add(movable);
                entity.Add(position);
                entity.Add(text);
            }
            else
            {
                if (!entity.Has<MoveComponent>())
                    return;
                ref var movable = ref entity.Get<MoveComponent>();
                ref var position = ref entity.Get<PositionComponent>();
                ref var text = ref entity.Get<TextComponent>();
                text.Text = uniqueId.ToString();
                movable.Destination = location;
            }
        }
    }
}