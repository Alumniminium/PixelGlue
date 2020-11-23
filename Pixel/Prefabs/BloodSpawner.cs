using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;

public class BloodSpawner : Prefab, IInteractable
{
    public Vector2 Position => Entity.Get<PositionComponent>().Value;
    public bool Active 
    { 
        get => Entity.Get<ParticleEmitterComponent>().Active; 
        set 
        {
            ref var pem = ref Entity.Get<ParticleEmitterComponent>();
            pem.Active=value;
            pem.Particles=0;
        }
    }

    public BloodSpawner(int x, int y)
    {
        Entity = World.CreateEntity();
        Entity.Add(new PositionComponent(x, y));
        Entity.Add(new DbgBoundingBoxComponent());
        Entity.Add(new ParticleEmitterComponent(50000,8,500));

        ref var nameTag = ref World.CreateEntity();
        nameTag.Add(new TextComponent($"{Entity.EntityId}: Blood Spawner {x},{y}, Active: {Active}"));
        nameTag.Add(new PositionComponent(-32, 24, 0));

        Entity.AddChild(ref nameTag);
        Global.Prefabs.TryAdd(Entity, this);
    }

    public void Start()
    {
        Active = true;
        
    }
    public void Stop()
    {
        Active = false;
    }
    public void Activate()
    {
        if (Active)
            Stop();
        else
            Start();
    }

    public override string ToString() => $"[{Id}] {nameof(BloodSpawner)}, Active: {Active}";
}
