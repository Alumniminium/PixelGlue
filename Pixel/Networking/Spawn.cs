using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Scenes;
using Pixel.World;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking
{
    public static class Spawn
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Handle(MsgSpawn packet)
        {
            if (SceneManager.ActiveScene.UniqueIdToEntityId.ContainsKey(packet.UniqueId))
                return;

            var entity = SceneManager.ActiveScene.CreateEntity(packet.UniqueId,EntityType.Npc);
            var srcEntity = Database.Entities[packet.Model];
            entity.Get<PositionComponent>().Value = new Vector2(packet.X, packet.Y);
            entity.Get<DrawableComponent>().SrcRect = srcEntity.SrcRect;
            entity.Get<DrawableComponent>().TextureName = srcEntity.TextureName;
        }
    }
}