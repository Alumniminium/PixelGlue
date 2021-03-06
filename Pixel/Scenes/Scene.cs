using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shared;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Shared.Diagnostics;
using Shared.ECS;
using Pixel.Helpers;
using Shared.ECS.Components;
using Shared.IO;

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
            World.Update();
            for (int i = 0; i < World.Systems.Count; i++)
            {
                var preUpdateTicks = DateTime.UtcNow.Ticks;
                var system = World.Systems[i];
                
                if (!system.WantsUpdate)
                    continue;
                if (system.IsActive)
                {
                    if (Global.Verbose)
                    FConsole.WriteLine("Update Starting for "+system.Name);
                    
                    system.Update((float)deltaTime.ElapsedGameTime.TotalSeconds);
                    
                    if (Global.Verbose)
                    FConsole.WriteLine("Done "+system.Name);
                }
                var postUpdateTicks = DateTime.UtcNow.Ticks;
                Profiler.AddUpdate(system.Name, (postUpdateTicks - preUpdateTicks) / 10000f);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(SpriteBatch sb)
        {
            ref readonly var cam = ref ComponentList<CameraComponent>.Get(1);
            sb.Begin(SpriteSortMode.Deferred, transformMatrix: cam.Transform, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < World.Systems.Count; i++)
            {
                var preDrawTicks = DateTime.UtcNow.Ticks;
                var system = World.Systems[i];
                if (!system.WantsDraw)
                    continue;
                if (system.IsActive)
                    system.Draw(sb);
                var postDrawTicks = DateTime.UtcNow.Ticks;
                Profiler.AddDraw(system.Name, (postDrawTicks - preDrawTicks) / 10000f);
            }
            sb.End();
        }
    }
}