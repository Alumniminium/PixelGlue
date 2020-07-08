using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared;
using Pixel.Entities;

namespace Pixel.ECS.Systems
{

    public class EntityRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Entity Rendering System";

        public override void AddEntity(Entity entity)
        {
            if (entity.Has<PositionComponent>() && entity.Has<DrawableComponent>())
                base.AddEntity(entity);
        }
        public override void Update(float deltaTime)
        {
            //foreach (var entity in Entities)
            //{
            //    ref readonly var pos = ref entity.Get<PositionComponent>();
            //
            //    if (pos.Value.X < Scene.Camera.ServerScreenRect.Left || pos.Value.X > Scene.Camera.ServerScreenRect.Right || pos.Value.Y < Scene.Camera.ServerScreenRect.Top || pos.Value.Y > Scene.Camera.ServerScreenRect.Bottom)
            //            Scene.Destroy(entity.EntityId);
            //}
        }
        public override void Draw(SpriteBatch sb)
        {
            var origin = new Vector2(8);
            foreach (var entity in Entities)
            {
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (OutOfRange(pos))
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Value + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
        }

        private bool OutOfRange(PositionComponent pos)
        {
            return pos.Value.X < Scene.Camera.ServerScreenRect.Left || pos.Value.X > Scene.Camera.ServerScreenRect.Right || pos.Value.Y < Scene.Camera.ServerScreenRect.Top || pos.Value.Y > Scene.Camera.ServerScreenRect.Bottom;
        }
    }
}