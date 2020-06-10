using System;
using System.Runtime.CompilerServices;
using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;

namespace PixelGlueCore.Entities
{
    public class ComponentCollection
    {
        public ComponentList<DrawableComponent> DrawableComponents;
        public ComponentList<PositionComponent> PositionComponents;
        public ComponentList<InputComponent> InputComponents;
        public ComponentList<Networked> Networkeds;
        public ComponentList<CameraFollowTagComponent> CameraFollowTags;
        public ComponentList<MoveComponent> MoveComponents;
        public ComponentList<DbgBoundingBoxComponent> DbgBoundingBoxComponents;

        public ComponentCollection()
        {
            DrawableComponents = new ComponentList<DrawableComponent>(0);
            PositionComponents = new ComponentList<PositionComponent>(0);
            DbgBoundingBoxComponents = new ComponentList<DbgBoundingBoxComponent>(0);
            MoveComponents = new ComponentList<MoveComponent>(0);
            Networkeds = new ComponentList<Networked>(0);
            InputComponents = new ComponentList<InputComponent>(0);
            CameraFollowTags = new ComponentList<CameraFollowTagComponent>(0);
        }
    }
    public class ComponentList<T>
    {
        private T[] _items;
        public int Count => _items.Length;
        private int _curIndex;

        public ComponentList(int size)
        {
            _items = new T[size];
        }

        public ref T this[int index]
        {
            get
            {
                if (_items.Length >= index)
                    return ref _items[index];
                throw new IndexOutOfRangeException($"Only have {_items.Length} but you wanted {index}");
            }
        }

        public void Add(T val)
        {
            if(_items.Length == _curIndex)
                Grow();
            
            _items[_curIndex] = val;
            _curIndex++;
        }
        public void Clear()
        {
            _items = new T[1];
            _curIndex=0;
        }
        private void Grow()
        {
            var newCount = Count+1;
            var newItems = new T[newCount];
            for (int i = 0; i < Count;i++)
                newItems[i] = _items[i];
            _items=newItems;
        }
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
        public bool HasInputComponent() => Components.InputComponents.Count > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasDrawableComponent() => Components.DrawableComponents.Count > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasMoveComponent() => Components.MoveComponents.Count > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasPositionComponent() => Components.PositionComponents.Count > 0;
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public bool HasCameraFollowTagComponent() => Components.CameraFollowTags.Count > 0;
        public void AddDrawable(DrawableComponent component) => Components.DrawableComponents.Add(component);
        public void AddMovable(MoveComponent component) => Components.MoveComponents.Add(component);
        public void AddPosition(PositionComponent component) => Components.PositionComponents.Add(component);
        public void AddDbgBoundingBox(DbgBoundingBoxComponent component) => Components.DbgBoundingBoxComponents.Add(component);
        public void AddInput(InputComponent component) => Components.InputComponents.Add(component);
        public void AddCameraFollowTag(CameraFollowTagComponent component) => Components.CameraFollowTags.Add(component);
        public void AddNetworked(Networked component) => Components.Networkeds.Add(component);
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