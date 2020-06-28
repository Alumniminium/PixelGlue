using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Entities;
using System;
using System.Runtime.CompilerServices;

namespace Pixel.ECS
{
    public class UIScene : Scene
     {  
        public override void Initialize()
        {            
            for (int i = 0; i < Systems.Count; i++)
                Systems[i].Initialize();
            IsReady = true;
        }        
        public override void LoadContent(ContentManager cm)
        {
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont_12.fnt", "profont_12");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/emoji.fnt", "emoji");
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Update(GameTime deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Update((float)deltaTime.ElapsedGameTime.TotalSeconds);
            }
       }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].FixedUpdate(deltaTime);
            }
        } 
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(samplerState: SamplerState.PointClamp);            
            for (int i = 0; i < Systems.Count; i++)
            {
                if (Systems[i].IsActive && Systems[i].IsReady)
                    Systems[i].Draw(sb);
            }
            sb.End();   
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public override T CreateUIEntity<T>()
        {
            var entity = new T
            {
                EntityId = LastEntityId,
                UIScene = this
            };
            Entities.TryAdd(entity.EntityId, entity);
            LastEntityId++;
            return entity;
        } 
        public override T GetSystem<T>()
        {
            foreach (var sys in Systems)
                if (sys is T t)
                    return t;
            return default;
        }
        public override int GetHashCode() => Id;
        public override bool Equals(object obj) => (obj as UIScene)?.Id == Id;

        public override T CreateEntity<T>(int uniqueId)=> throw new NotSupportedException();

        public override void Destroy(Entity entity)
        {
            Entities.TryRemove(entity.EntityId, out _);
        }
    }
}