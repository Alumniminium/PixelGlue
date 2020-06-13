using System;
using System.Runtime.CompilerServices;
using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Helpers;

namespace PixelGlueCore.Entities
{
    public class PixelEntity
    {
        public Scene Scene;
        public int UniqueId;
        public ComponentCollection Components;

        public PixelEntity()
        {
            Components = new ComponentCollection();
        }

        public bool HasInputComponent() => Components.InputComponents.Count > 0;
        public bool HasDrawableComponent() => Components.DrawableComponents.Count > 0;
        public bool HasMoveComponent() => Components.MoveComponents.Count > 0;
        public bool HasPositionComponent() => Components.PositionComponents.Count > 0;
        public bool HasCameraFollowTagComponent() => Components.CameraFollowTags.Count > 0;
        public void AddDrawable(DrawableComponent component) => Components.DrawableComponents.Add(component);
        public void AddMovable(MoveComponent component) => Components.MoveComponents.Add(component);
        public void AddPosition(PositionComponent component) => Components.PositionComponents.Add(component);
        public void AddDbgBoundingBox(DbgBoundingBoxComponent component) => Components.DbgBoundingBoxComponents.Add(component);
        public void AddInput(InputComponent component) => Components.InputComponents.Add(component);
        public void AddCameraFollowTag(CameraFollowTagComponent component) => Components.CameraFollowTags.Add(component);
        public void AddNetworked(Networked component) => Components.Networkeds.Add(component);
        public void AddTextComponent(TextComponent component)=> Components.TextComponents.Add(component);
        public ref DrawableComponent GetDrawableComponentRef() => ref Components.DrawableComponents[0];
        public ref InputComponent GetInputComponentRef() => ref Components.InputComponents[0];
        public ref TextComponent GetTextComponentRef() => ref Components.TextComponents[0];

        public ref PositionComponent GetPositionComponentRef() => ref Components.PositionComponents[0];
        public ref MoveComponent GetMoveComponentRef() => ref Components.MoveComponents[0];
        public ref CameraFollowTagComponent GetCamreaFollowRef() => ref Components.CameraFollowTags[0];

        public override string ToString()
        {
            var ret = string.Empty;

            ret += "UID: " + UniqueId;
            ret += Environment.NewLine;
            return ret;
        }
    }
}