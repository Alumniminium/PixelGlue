using System.Net.Mime;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using Shared.ECS;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking
{
    public static class Spawn
    {
        public static void Handle(MsgSpawn packet)
        {
            if (!World.UniqueIdToEntityId.TryGetValue(packet.UniqueId, out var id))
            {
                var entity = World.CreateEntity();
                SceneManager.ActiveScene.ApplyArchetype(ref entity, EntityType.Npc);
                entity.Add(new NetworkComponent(packet.UniqueId));
                World.UniqueIdToEntityId.TryAdd(packet.UniqueId, entity.EntityId);
                World.EntityIdToUniqueId.TryAdd(entity.EntityId, packet.UniqueId);
                var srcEntity = Database.Entities[packet.Model];
                entity.Get<PositionComponent>().Value = new Vector2(packet.X, packet.Y);
                entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
                entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
                World.Entities[entity.Children[0]].Get<TextComponent>().Value = packet.GetName();
                entity.Register();
            }
            else
            {
                var entity = World.Entities[id];
                var srcEntity = Database.Entities[packet.Model];
                entity.Get<PositionComponent>().Value = new Vector2(packet.X, packet.Y);
                entity.Get<DestinationComponent>().Value = new Vector2(packet.X, packet.Y);
                entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
                entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
                if(entity.Children != null)
                World.Entities[entity.Children[0]].Get<TextComponent>().Value = packet.GetName();
            }
        }
    }
}