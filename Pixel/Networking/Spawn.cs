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
    public static class Spawn
    {
        public static void Handle(MsgSpawn packet)
        {
            if (!World.UidExists(packet.UniqueId))
            {
                var srcEntity = Database.Entities[packet.Model];

                if (packet.UniqueId >= 1_000_000)
                {
                    var player = TestingScene.Player;

                    player.Speed = 128;
                    player.Position = new Vector2(packet.X, packet.Y);
                    player.DrawableComponent.TextureName = srcEntity.TextureName;
                    player.DrawableComponent.SrcRect = srcEntity.SrcRect;
                }
                else
                {
                    ref var entity = ref World.CreateEntity(packet.UniqueId);
                    entity.Add(new NetworkComponent(packet.UniqueId));
                    entity.Add<DbgBoundingBoxComponent>();
                    entity.Add(new SpeedComponent(32));
                    entity.Add(new PositionComponent(packet.X, packet.Y));
                    entity.Add(new DrawableComponent(srcEntity.TextureName,srcEntity.SrcRect));
                    var name = Global.Names[Global.Random.Next(0, Global.Names.Length)];
                    ref var npcNameTag = ref World.CreateEntity();
                    npcNameTag.Add(new TextComponent($"{entity.EntityId}: {name}"));
                    npcNameTag.Add(new PositionComponent(-16, -16, 0));
                    entity.AddChild(ref npcNameTag);
                }

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