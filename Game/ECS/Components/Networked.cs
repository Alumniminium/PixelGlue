namespace PixelGlueCore.ECS.Components
{
    public struct Networked : IEntityComponent
    {
        public int PixelOwnerId { get; set; }

        public Networked(int ownerId)
        {
            PixelOwnerId=ownerId;
        }
    }
}