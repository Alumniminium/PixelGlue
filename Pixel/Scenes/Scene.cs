using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shared;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Pixel.Enums;
using Shared.Diagnostics;
using Shared.ECS;
using Pixel.ECS.Components;
using Pixel.Helpers;
using Shared.ECS.Components;

namespace Pixel.Scenes
{
    public class Scene
    {
        public int Id;
        public bool IsActive;
        public bool IsReady;

        public virtual void Initialize()
        {
            for (int i = 0; i < World.Systems.Count; i++)
                World.Systems[i].Initialize();
        }

        public virtual void LoadContent(ContentManager cm)
        {
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            Global.Names = File.ReadAllText("../Build/Content/RuntimeContent/Names.txt").Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(GameTime deltaTime)
        {
            for (int i = 0; i < World.Systems.Count; i++)
            {
                var system = World.Systems[i];
                system.PreUpdate();
            }
            for (int i = 0; i < World.Systems.Count; i++)
            {
                var preUpdateTicks = DateTime.UtcNow.Ticks;
                var system = World.Systems[i];

                if (!system.WantsUpdate)
                    continue;
                if (system.IsActive)
                    system.Update((float)deltaTime.ElapsedGameTime.TotalSeconds);

                var postUpdateTicks = DateTime.UtcNow.Ticks;
                Profiler.AddUpdate(system.Name, (postUpdateTicks - preUpdateTicks) / 10000f);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < World.Systems.Count; i++)
                if (World.Systems[i].IsActive)
                    World.Systems[i].FixedUpdate(deltaTime);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(SpriteBatch sb)
        {
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
            sb.Begin(SpriteSortMode.Deferred, transformMatrix: cam.Transform, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < World.Systems.Count; i++)
            {
                var preDrawTicks = DateTime.UtcNow.Ticks;
                var system = World.Systems[i];
                if (!system.WantsDraw)
                    continue;
                system.PreUpdate();
                if (system.IsActive)
                    system.Draw(sb);
                var postDrawTicks = DateTime.UtcNow.Ticks;
                Profiler.AddDraw(system.Name, (postDrawTicks - preDrawTicks) / 10000f);
            }
            sb.End();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyArchetype(ref Entity entity, EntityType et)
        {
            switch (et)
            {
                case EntityType.Player:
                    entity.Add<KeyboardComponent>();
                    entity.Add<MouseComponent>();
                    entity.Add<DbgBoundingBoxComponent>();
                    entity.Add(new SpeedComponent(128));
                    entity.Add(new CameraComponent(1));
                    entity.Add(new DrawableComponent("character.png", new Rectangle(0, 2, 16, 16)));

                    ref var nameTag = ref World.CreateEntity();
                    nameTag.Add(new TextComponent($"{entity.EntityId}: {entity}"));
                    nameTag.Add(new PositionComponent(-64, -24, 0));
                    entity.AddChild(ref nameTag);
                    break;
                case EntityType.Npc:
                    var srcEntity = Database.Entities[Global.Random.Next(0, Database.Entities.Count)];
                    var name = Global.Names[Global.Random.Next(0, Global.Names.Length)];

                    entity.Add(new DrawableComponent(srcEntity.TextureName, srcEntity.SrcRect));
                    entity.Add<DbgBoundingBoxComponent>();
                    entity.Add(new SpeedComponent(32));

                    nameTag = ref World.CreateEntity();
                    nameTag.Add(new TextComponent($"{entity.EntityId}: {name}"));
                    nameTag.Add(new PositionComponent(-16, -16, 0));
                    entity.AddChild(ref nameTag);
                    break;
            }
        }
    }
}