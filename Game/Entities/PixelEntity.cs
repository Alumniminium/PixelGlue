using System;
using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Helpers;

namespace PixelGlueCore.Entities
{
    public class PixelEntity
    {
        public Scene Scene;
        public int EntityId;

        public void Add<T>() where T : struct, IEntityComponent
        {
            ComponentList<T>.Items[EntityId] = new T
            {
                UniqueId = EntityId
            };
        }
        public void Add<T>(T component) where T : struct => ComponentList<T>.Items[EntityId] = component;
        public bool Has<T>() where T : struct, IEntityComponent => ComponentList<T>.Items[EntityId].UniqueId == EntityId;
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