using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking
{
    public struct Npc
    {
        public int EntityId;
    }
    public static class Spawn
    {
        public static void Handle(MsgSpawn packet)
        {
            if (!World.UidExists(packet.UniqueId))
            {
                ref var entity = ref World.CreateEntity(packet.UniqueId);
                var srcEntity = Database.Entities[packet.Model];

                if (packet.UniqueId >= 1_000_000)
                {
                    entity = TestingScene.Player;
                    SceneManager.ActiveScene.ApplyArchetype(ref entity, EntityType.Player);
                }
                else
                {
                    entity.Add(new NetworkComponent(packet.UniqueId));
                    SceneManager.ActiveScene.ApplyArchetype(ref entity, EntityType.Npc);
                }
                
                entity.Add(new PositionComponent(packet.X, packet.Y));

                ref var drw = ref entity.Add<DrawableComponent>();
                drw.SrcRect = srcEntity.SrcRect;
                drw.TextureName = srcEntity.TextureName;

                ref var nameTag = ref World.GetEntity(entity.Children[0]);
                ref var txt = ref nameTag.Get<TextComponent>();
                txt.Value = packet.GetName();
            }
            else
            {
                return;
                ref var entity = ref World.GetEntityByUniqueId(packet.UniqueId);
                var srcEntity = Database.Entities[packet.Model];
                entity.Get<PositionComponent>().Value = new Vector2(packet.X, packet.Y);

                if (entity.Has<DestinationComponent>())
                    entity.Get<DestinationComponent>().Value = new Vector2(packet.X, packet.Y);
                else
                    entity.Add(new DestinationComponent(packet.X, packet.Y));

                entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
                entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
                if (entity.Children != null)
                    World.GetEntity(entity.Children[0]).Get<TextComponent>().Value = packet.GetName();
            }
        }
    }
}