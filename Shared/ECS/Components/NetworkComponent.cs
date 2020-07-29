using Shared.ECS;

namespace Shared.ECS.Components
{
    [Component]
    public struct NetworkComponent
    {
        public int UniqueId { get; set; }
        public NetworkComponent(int uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}