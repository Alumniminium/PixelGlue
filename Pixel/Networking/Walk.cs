using Pixel.ECS.Components;
using Pixel.World;
using Microsoft.Xna.Framework;
using PixelShared.TerribleSockets.Packets;
using Pixel.Scenes;
using Pixel.Entities;
using Pixel.ECS;

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
            if(!scene.UniqueIdToEntityId.TryGetValue(uniqueId,out var entityId))
            {
                var entity = scene.CreateEntity<Npc>(uniqueId);
                var name = Global.Names[Global.Random.Next(0,Global.Names.Length)];
                
                var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                entity.Add(new DrawableComponent(entity.EntityId,srcEntity.TextureName, srcEntity.SrcRect));
                
                entity.Add(new PositionComponent(entity.EntityId,packet.X, packet.Y, 0));
                entity.Add(new VelocityComponent(entity.EntityId,32));
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
                ref var position = ref entity.Get<PositionComponent>();
                //ref var text = ref entity.Get<TextComponent>();
                //text.Text = uniqueId.ToString();
                position.Destination = location;
            }
        }
    }
}