using PixelGlueCore.ECS.Components;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using TerribleSockets.Packets;
using PixelGlueCore.Scenes;
using PixelGlueCore.Entities;
using PixelGlueCore.ECS;

namespace PixelGlueCore.Networking.Handlers
{
    public static class Walk
    {
        public static void Handle(MsgWalk packet)
        {
            var uniqueId = packet.UniqueId;
            var location = new Vector2(packet.X, packet.Y);
            var tickCount = packet.TickCount;
            var scene = SceneManager.ActiveGameScenes[^1];
            if(!scene.UniqueIdToEntityId.TryGetValue(uniqueId,out var entityId))
            {
                var srcEntity = Database.Entities[PixelGlue.Random.Next(0, Database.Entities.Count)];
                var entity = scene.CreateEntity<PixelEntity>(uniqueId);
                var name = PixelGlue.Names[PixelGlue.Random.Next(0,PixelGlue.Names.Length)];
                
                entity.Add(new DrawableComponent(entity.EntityId,srcEntity.TextureName, srcEntity.SrcRect));
                entity.Add(new PositionComponent(entity.EntityId,packet.X, packet.Y, 0));
                entity.Add(new MoveComponent(entity.EntityId,8, packet.X, packet.Y));
                entity.Add(new DbgBoundingBoxComponent(entity.EntityId));

                var nameTag = scene.CreateEntity<NameTag>(entity.UniqueId);
                nameTag.Parent=entity;
                nameTag.Add(new TextComponent(nameTag.EntityId,name,"profont_12"));
                nameTag.Add(new PositionComponent(nameTag.EntityId,-8,-16,0));
                entity.Children.Add(nameTag);
            }
            else
            {
                var entity = scene.Entities[entityId];
                if (!entity.Has<MoveComponent>())
                    return;
                ref var movable = ref entity.Get<MoveComponent>();
                //ref var position = ref entity.Get<PositionComponent>();
                //ref var text = ref entity.Get<TextComponent>();
                //text.Text = uniqueId.ToString();
                movable.Destination = location;
            }
        }
    }
}