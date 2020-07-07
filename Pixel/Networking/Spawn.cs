using Pixel.Entities;
using Pixel.Scenes;
using Pixel.World;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking
{
    public static class Spawn
    {
        public static void Handle(MsgSpawn packet)
        {
            if (!SceneManager.ActiveScene.UniqueIdToEntityId.ContainsKey(packet.UniqueId))
            {
                var entity = SceneManager.ActiveScene.CreateEntity<Npc>(packet.UniqueId);
                var srcEntity = Database.Entities[packet.Model];
                entity.DrawableComponent.SrcRect = srcEntity.SrcRect;
                entity.DrawableComponent.TextureName = srcEntity.TextureName;
            }
        }
    }
}