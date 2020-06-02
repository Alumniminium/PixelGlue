using PixelGlueCore.ECS.Components;
using System;
using System.Collections.Generic;

namespace PixelGlueCore.ECS
{
    public class GameObject
    {
        public uint UniqueId = 0;
        public HashSet<IEntityComponent> Components { get; set; } = new HashSet<IEntityComponent>();
        public GameObject() => UniqueId = Helpers.UniqueIdGen.GetNextUID();
        public void AddComponent(IEntityComponent component) => Components.Add(component);
        public bool TryGetComponent<T>(out T component) where T : IEntityComponent
        {
            component = default;
            foreach (var comp in Components)
            {
                if (comp is T)
                {
                    component = (T)comp;
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            var ret = string.Empty;

            ret += "UID: " + UniqueId;
            ret += Environment.NewLine;
            ret += "Components: " + Components.Count;
            return ret;
        }
    }
}