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
            if (!World.UidExists(packet.UniqueId))
            {
                ref var entity = ref World.CreateEntity(packet.UniqueId);
                SceneManager.ActiveScene.ApplyArchetype(ref entity, EntityType.Npc);
                entity.Add(new NetworkComponent(packet.UniqueId));
                var srcEntity = Database.Entities[packet.Model];
                entity.Get<PositionComponent>().Value = new Vector2(packet.X, packet.Y);
                entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
                entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
                World.GetEntity(entity.Children[0]).Get<TextComponent>().Value = packet.GetName();
                entity.Register();
            }
            else
            {
                ref var entity = ref World.GetEntityByUniqueId(packet.UniqueId);
                var srcEntity = Database.Entities[packet.Model];
                entity.Get<PositionComponent>().Value = new Vector2(packet.X, packet.Y);
                entity.Get<DestinationComponent>().Value = new Vector2(packet.X, packet.Y);
                entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
                entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
                if (entity.Children != null)
                    World.GetEntity(entity.Children[0]).Get<TextComponent>().Value = packet.GetName();
            }
        }
    }
}