using Microsoft.Xna.Framework;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct NetworkComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public int UniqueId { get; set; }
        public Vector2 Position;
        public NetworkComponent(Scene scene, int ownerId, int uniqueId)
        {
            EntityId = ownerId;
            UniqueId = uniqueId;
            Position = Vector2.Zero;
            scene.UniqueIdToEntityId.TryAdd(uniqueId, ownerId);
            scene.EntityIdToUniqueId.TryAdd(ownerId, uniqueId);
        }
    }
}