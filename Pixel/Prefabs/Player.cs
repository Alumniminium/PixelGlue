using System;
using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;

public class Player : Prefab
{
    public string Name {
        get=> World.GetEntity(Entity.Children[0]).Get<TextComponent>().Value;
        set=> World.GetEntity(Entity.Children[0]).Get<TextComponent>().Value=value;
    }
    public Vector2 Position
    {
        get => Entity.Get<PositionComponent>().Value;
        set => Entity.Get<PositionComponent>().Value = value;
    }
    public float Speed 
    {
        get => Entity.Get<SpeedComponent>().Speed;
        set => Entity.Get<SpeedComponent>().Speed=value;
    }
    public ref Entity NameTag => ref World.GetEntity(Entity.Children[0]);
    public ref CameraComponent Camera => ref Entity.Get<CameraComponent>();
    public ref DrawableComponent DrawableComponent => ref Entity.Get<DrawableComponent>();

    public Player(int x, int y)
    {
        Entity = World.CreateEntity();
        Entity.Add<KeyboardComponent>();
        Entity.Add<MouseComponent>();
        Entity.Add(new PositionComponent(x, y));
        Entity.Add(new DbgBoundingBoxComponent());
        Entity.Add(new DrawableComponent(Color.Magenta));
        Entity.Add(new CameraComponent(1));
        Entity.Add(new SpeedComponent(128));

        ref var nameTag = ref World.CreateEntity();
        nameTag.Add(new TextComponent($"Player :D"));
        nameTag.Add(new PositionComponent(-32, 24, 0));

        Entity.AddChild(ref nameTag);
        Global.Prefabs.TryAdd(Entity, this);
    }

    public override string ToString() => $"[{Id}] {nameof(Player)}: {Name}";
}
