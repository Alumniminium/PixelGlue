using Pixel.ECS.Components;
using Pixel.Helpers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pixel.Entities
{
    public struct Entity
    {
        public bool Valid;
        public int EntityId;
        public int Parent;
        public List<int> Children;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>(T component) where T : struct => ComponentArray<T>.AddFor(this,component);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>() where T : struct => ComponentArray<T>.AddFor(this);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct => ComponentArray<T>.HasFor(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct => ref ComponentArray<T>.Get(EntityId);

        public void DestroyComponents()
        {
            ComponentArray<PositionComponent>.Remove(EntityId);
            ComponentArray<VelocityComponent>.Remove(EntityId);
            ComponentArray<DestinationComponent>.Remove(EntityId);
            ComponentArray<CameraFollowTagComponent>.Remove(EntityId);
            ComponentArray<DbgBoundingBoxComponent>.Remove(EntityId);
            ComponentArray<DialogComponent>.Remove(EntityId);
            ComponentArray<DrawableComponent>.Remove(EntityId);
            ComponentArray<InputComponent>.Remove(EntityId);
            ComponentArray<NetworkComponent>.Remove(EntityId);
            ComponentArray<ParticleComponent>.Remove(EntityId);
            ComponentArray<ParticleEmitterComponent>.Remove(EntityId);
            ComponentArray<SpeedComponent>.Remove(EntityId);
            ComponentArray<TextComponent>.Remove(EntityId);
            ComponentArray<TransformComponent>.Remove(EntityId);
            ComponentArray<VelocityComponent>.Remove(EntityId);
        }

        public override string ToString()
        {
            var ret = string.Empty;
            ret += "UID: " + EntityId;
            //ret += Environment.NewLine;
            return ret;
        }
    }
}