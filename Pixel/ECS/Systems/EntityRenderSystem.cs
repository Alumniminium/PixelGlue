using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using Pixel.Scenes;
using PixelShared;

namespace Pixel.ECS.Systems
{

    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public Scene Scene => SceneManager.ActiveScene;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(float deltaTime) 
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FixedUpdate(float timeSinceLastFrame) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(SpriteBatch sb)
        {
            var origin = new Vector2(8);
            foreach (var entityId in CompIter<DrawableComponent, PositionComponent>.Get())
            {
                ref readonly var pos = ref ComponentArray<PositionComponent>.Get(entityId);
                ref readonly var drawable = ref ComponentArray<DrawableComponent>.Get(entityId);

                if (pos.Value.X < Scene.Camera.ServerScreenRect.Left || pos.Value.X > Scene.Camera.ServerScreenRect.Right
                || pos.Value.Y < Scene.Camera.ServerScreenRect.Top || pos.Value.Y > Scene.Camera.ServerScreenRect.Bottom)
                    {
                        Scene.Destroy(entityId);
                        continue;
                    }

                sb.Draw(drawable.Texture, pos.Value + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
        }
    }
}