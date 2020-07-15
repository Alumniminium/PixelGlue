using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shared.ECS
{
    public struct Entity
    {
        public int EntityId;
        public int Parent;
        public List<int> Children;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>(T component) where T : struct => ComponentArray<T>.AddFor(EntityId, component);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>() where T : struct => ComponentArray<T>.AddFor(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct => ComponentArray<T>.HasFor(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct => ref ComponentArray<T>.Get(EntityId);


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