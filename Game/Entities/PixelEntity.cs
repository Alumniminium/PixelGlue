using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Helpers;

namespace PixelGlueCore.Entities
{
    public class PixelEntity
    {
        public Scene Scene;
        public int EntityId;
        public int UniqueId;
        public PixelEntity Parent;
        public List<PixelEntity> Children;

        public PixelEntity()
        {
            Children=new List<PixelEntity>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add<T>(T component) where T : struct => ComponentList<T>.Items[EntityId] = component;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct, IEntityComponent => ComponentList<T>.Items[EntityId].UniqueId == EntityId;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct => ref ComponentList<T>.Items[EntityId];

        public override string ToString()
        {
            var ret = string.Empty;

            ret += "UID: " + EntityId;
            ret += Environment.NewLine;
            return ret;
        }
    }
}