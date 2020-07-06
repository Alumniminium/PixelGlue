namespace Pixel.ECS.Components
{
    public struct NetworkComponent
    {
        public int UniqueId { get; set; }
        public NetworkComponent(Scene scene, int ownerId, int uniqueId)
        {
            UniqueId = uniqueId;
            scene.UniqueIdToEntityId.TryAdd(uniqueId, ownerId);
            scene.EntityIdToUniqueId.TryAdd(ownerId, uniqueId);
        }
    }
}