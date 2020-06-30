using Microsoft.Xna.Framework;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct NetworkComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public int UniqueId { get; set; }
        public NetworkComponent(Scene scene, int ownerId, int uniqueId)
        {
            EntityId = ownerId;
            UniqueId = uniqueId;
            scene.UniqueIdToEntityId.TryAdd(uniqueId, ownerId);
            scene.EntityIdToUniqueId.TryAdd(ownerId, uniqueId);
        }
    }
}