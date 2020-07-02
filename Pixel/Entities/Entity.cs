using Pixel.ECS;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pixel.Entities
{
    public class Entity
    {
        public Scene Scene;
        public int EntityId;
        public int UniqueId => Has<NetworkComponent>() ? Get<NetworkComponent>().UniqueId : -1;
        public Entity Parent;
        public List<Entity> Children;

        public Entity()
        {
            Children = new List<Entity>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>(T component) where T : struct => ComponentArray<T>.Add(EntityId,component);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct, IEntityComponent => ComponentArray<T>.HasFrom(EntityId);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct => ref ComponentArray<T>.Get(EntityId);

        public override string ToString()
        {
            var ret = string.Empty;
            ret += "UID: " + EntityId;
            //ret += Environment.NewLine;
            return ret;
        }
    }
}