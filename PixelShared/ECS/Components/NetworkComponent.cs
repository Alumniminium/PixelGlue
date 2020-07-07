namespace Pixel.ECS.Components
{
    public struct NetworkComponent
    {
        public int UniqueId { get; set; }
        public NetworkComponent(int uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}