using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using System.Runtime.CompilerServices;
using Pixel.Scenes;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{

    public class EntityRenderSystem : PixelSystem
    {
        public EntityRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override string Name { get; set; } = "Entity Rendering System";
        public Scene Scene => SceneManager.ActiveScene;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(int entityId)
        {
            var entity = World.Entities[entityId];
            if (entity.Has<PositionComponent>() && entity.Has<DrawableComponent>())
                base.AddEntity(entityId);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            var origin = new Vector2(8);
            foreach (var entityId in Entities)
            {
                var entity = World.Entities[entityId];
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (OutOfRange(pos.Value + drawable.SrcRect.Size.ToVector2()))
                {
                    //Scene.World.Destroy(entity.EntityId);
                    continue;
                }

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Value + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
            }
        }

        private bool OutOfRange(Vector2 pos)
        {
            var bounds = Scene.Camera.WorldBounds();
            return pos.X < bounds.Left || pos.X > bounds.Right || pos.Y <= bounds.Top || pos.Y >= bounds.Bottom;
        }
    }
}