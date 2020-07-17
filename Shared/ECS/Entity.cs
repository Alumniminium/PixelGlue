using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shared.ECS
{
    public partial struct Entity
    {
        public int EntityId;
        public int Parent;
        public List<int> Children;
    }
    public partial struct Entity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>(T component) where T : struct => ComponentArray<T>.AddFor(EntityId, component);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>() where T : struct => ComponentArray<T>.AddFor(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct => ComponentArray<T>.HasFor(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct => ref ComponentArray<T>.Get(EntityId);

        public bool Has<T, T2>() where T : struct where T2 : struct => Has<T>() && Has<T2>();
        public bool Has<T, T2,T3>() where T : struct where T2 : struct where T3 : struct => Has<T,T2>() && Has<T3>();
        public bool Has<T, T2,T3,T4>() where T : struct where T2 : struct where T3 : struct where T4 : struct => Has<T,T2,T3>() && Has<T4>();


        public override string ToString()
        {
            var ret = string.Empty;
            ret += "UID: " + EntityId;
            //ret += Environment.NewLine;
            return ret;
        }

        public void AddChild(Entity nt)
        {
            nt.Parent = EntityId;
            Children = new List<int> { nt.EntityId };
        }

        public void Register()
        {
            foreach (var sys in World.Systems)
                sys.AddEntity(EntityId);
        }
    }
}