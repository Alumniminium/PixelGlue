using System;
using System.Collections;
using System.Runtime.CompilerServices;
using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.Entities
{
    public class ComponentList<T>
    {
        public T[] items;
        public int Count => items.Length;
        public int current=-1;

        public ref T this[int index]
        {
            get
            {
                if (items.Length >= index)
                    return ref items[index];
                throw new IndexOutOfRangeException($"Only have {items.Length} but you wanted {index}");
            }
        }

        public void Add(T val)
        {
            if(items.Length > current)
            {
                items[current] = val;
            }
            else
            throw new IndexOutOfRangeException("Fuck");
        }
        public void Clear() => items = new T[0];
    }
    public class PixelEntity
    {
        public Scene Scene;
        public int UniqueId;
        public ComponentCollection Components;

        public PixelEntity()
        {
            Components = new ComponentCollection();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasInputComponent() => Components.InputComponentsCount > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasDrawableComponent() => Components.DrawablesCount > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasMoveComponent() => Components.MovesCount > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasPositionComponent() => Components.PositionsCount > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasCameraFollowTagComponent() => Components.CameraFollowTagsCount > 0;


        public void AddDrawable(DrawableComponent component)
        {
            if (Components.DrawableComponents.Length == 0)
                Components.DrawableComponents = new DrawableComponent[1];
            Components.DrawableComponents[Components.DrawablesCount] = component;
            Components.DrawablesCount++;
        }

        public void AddMovable(MoveComponent component)
        {
            if (Components.MoveComponents.Length == 0)
                Components.MoveComponents = new MoveComponent[1];
            Components.MoveComponents[Components.MovesCount] = component;
            Components.MovesCount++;
        }

        public void AddPosition(PositionComponent component)
        {
            if (Components.PositionComponents.Length == 0)
                Components.PositionComponents = new PositionComponent[1];
            Components.PositionComponents[Components.PositionsCount] = component;
            Components.PositionsCount++;
        }

        public void AddDbgBoundingBox(DbgBoundingBoxComponent component)
        {
            if (Components.DbgBoundingBoxComponents.Length == 0)
                Components.DbgBoundingBoxComponents = new DbgBoundingBoxComponent[1];
            Components.DbgBoundingBoxComponents[Components.DbgBoundingBoxCount] = component;
            Components.DbgBoundingBoxCount++;
        }
        public void AddInput(InputComponent component)
        {
            if (Components.InputComponents.Length == 0)
                Components.InputComponents = new InputComponent[1];
            Components.InputComponents[Components.InputComponentsCount] = component;
            Components.InputComponentsCount++;
        }

        public void AddCameraFollowTag(CameraFollowTagComponent component)
        {
            if (Components.CameraFollowTags.Length == 0)
                Components.CameraFollowTags = new CameraFollowTagComponent[1];
            Components.CameraFollowTags[Components.CameraFollowTagsCount] = component;
            Components.CameraFollowTagsCount++;
        }

        public void AddNetworked(Networked component)
        {
            if (Components.Networkeds.Length == 0)
                Components.Networkeds = new Networked[1];
            Components.Networkeds[Components.NetworkedsCount] = component;
            Components.NetworkedsCount++;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref DrawableComponent GetDrawableComponentRef() => ref Components.DrawableComponents[0];
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref InputComponent GetInputComponentRef() => ref Components.InputComponents[0];
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref PositionComponent GetPositionComponentRef() => ref Components.PositionComponents[0];
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public ref MoveComponent GetMoveComponentRef() => ref Components.MoveComponents[0];
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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