using Pixel.ECS.Components;
using Pixel.ECS;

namespace Pixel.Entities
{
    public class NameTag : Entity
    {
        public ref TextComponent TextComponent => ref Get<TextComponent>();
        public ref PositionComponent PositionComponent=>ref Get<PositionComponent>();
    }
}