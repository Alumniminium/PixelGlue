using System;
using PixelGlueCore.ECS;

namespace PixelGlueCore.Entities
{
    public class PixelEntity
    {
        public Scene Scene;
        public int UniqueId = 0;

        //public PixelEntity() => UniqueId = Helpers.UniqueIdGen.GetNextUID();
        public override string ToString()
        {
            var ret = string.Empty;

            ret += "UID: " + UniqueId;
            ret += Environment.NewLine;
            return ret;
        }

        internal bool HasInputComponent()
        {
            if(Scene == null)
                return false;
            if (Scene.Components.TryGetValue(UniqueId, out var c))
            {
                return c.InputComponentsCount > 0;
            }
            return false;
        }

        internal bool HasDrawableComponent()
        {
            if(Scene == null)
                return false;
            if (Scene.Components.TryGetValue(UniqueId, out var c))
            {
                return c.DrawablesCount > 0;
            }
            return false;
        }

        internal bool HasMoveComponent()
        {
            if(Scene == null)
                return false;
            if (Scene.Components.TryGetValue(UniqueId, out var c))
            {
                return c.MovesCount > 0;
            }
            return false;
        }

        internal bool HasPositionComponent()
        {
            if(Scene == null)
                return false;
            if (Scene.Components.TryGetValue(UniqueId, out var c))
            {
                return c.PositionsCount > 0;
            }
            return false;
        }

        internal bool HasCameraFollowTagComponent()
        {
            if(Scene == null)
                return false;
            if (Scene.Components.TryGetValue(UniqueId, out var c))
            {
                return c.CameraFollowTagsCount > 0;
            }
            return false;
        }
    }
}