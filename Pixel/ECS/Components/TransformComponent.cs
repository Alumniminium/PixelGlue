using Microsoft.Xna.Framework;
using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct TransformComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix InverseTransform => Matrix.Invert(ViewMatrix);

        public TransformComponent(int ownerId)
        {
            EntityId=ownerId;
            ViewMatrix = Matrix.Identity;
        }
    }
}
