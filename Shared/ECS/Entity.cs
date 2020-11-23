using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shared.ECS
{
    public partial struct Entity
    {
        public int EntityId;
        public int Parent;
    }
    public partial struct Entity
    {
        public List<int> Children => World.GetChildren(ref this);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>(T component) where T : struct 
        {
           ref var t = ref ComponentArray<T>.AddFor(EntityId, component);
           World.Register(EntityId);
           return ref t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct 
        {
           ref var t = ref ComponentArray<T>.AddFor(EntityId);
           World.Register(EntityId);
           return ref t;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct => ComponentArray<T>.HasFor(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct => ref ComponentArray<T>.Get(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>()
        {
           ReflectionHelper.Remove<T>(EntityId);
           World.Register(EntityId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T, T2>() where T : struct where T2 : struct => Has<T>() && Has<T2>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T, T2, T3>() where T : struct where T2 : struct where T3 : struct => Has<T, T2>() && Has<T3>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T, T2, T3, T4>() where T : struct where T2 : struct where T3 : struct where T4 : struct => Has<T, T2, T3>() && Has<T4>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddChild(ref Entity nt) => World.AddChildFor(ref this, ref nt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Recycle() => ReflectionHelper.RecycleComponents(EntityId);

        public override string ToString() => $"ID: {EntityId}, Parent: {Parent}, Children: {Children?.Count}";

        public override int GetHashCode() => EntityId;

    }
}