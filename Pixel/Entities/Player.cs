using Pixel.ECS.Components;

namespace Pixel.Entities
{
    public class Player : Entity
    {
        public ref NetworkComponent Networked => ref Get<NetworkComponent>();
        public ref PositionComponent PositionComponent => ref Get<PositionComponent>();
        public ref VelocityComponent MoveComponent => ref Get<VelocityComponent>();
        public ref DrawableComponent DrawableComponent => ref Get<DrawableComponent>();
    }
    public class Npc : Entity
    {
        public ref NetworkComponent Networked => ref Get<NetworkComponent>();
        public ref PositionComponent PositionComponent => ref Get<PositionComponent>();
        public ref VelocityComponent MoveComponent => ref Get<VelocityComponent>();
        public ref DrawableComponent DrawableComponent => ref Get<DrawableComponent>();
    }
    public class Monster : Entity
    {
        public ref NetworkComponent Networked => ref Get<NetworkComponent>();
        public ref PositionComponent PositionComponent => ref Get<PositionComponent>();
        public ref VelocityComponent MoveComponent => ref Get<VelocityComponent>();
        public ref DrawableComponent DrawableComponent => ref Get<DrawableComponent>();
    }
}