using System.Reflection;
using Pixel.ECS.Components;
using Pixel.Helpers;

namespace Pixel.Entities
{
    public class Player : Entity
    {
        public ref NetworkComponent Networked => ref Get<NetworkComponent>();
        public ref PositionComponent PositionComponent => ref Get<PositionComponent>();
        public ref VelocityComponent MoveComponent => ref Get<VelocityComponent>();
        public ref DrawableComponent DrawableComponent => ref Get<DrawableComponent>();
        public ref InputComponent InputComponent => ref Get<InputComponent>();
        public ref TextComponent NameTag => ref Children.Find(e=> e.Has<TextComponent>()).Get<TextComponent>();
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