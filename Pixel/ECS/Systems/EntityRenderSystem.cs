using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using System.Runtime.CompilerServices;
using Pixel.Helpers;

namespace Pixel.ECS.Systems
{
    public class EntityRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Entity Rendering System";

        public EntityRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent,DrawableComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            ref var cam = ref ComponentArray<CameraComponent>.Get(1);
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref readonly var pos = ref entity.Get<PositionComponent>();
//               //TODO: this makes it so it doesn't destryoy player, camera and cursor
                // implement it with archetypes instead of Ids which are volatile and meaningless af
                if (entityId == 1 || entityId == 2 || entityId == 3)
                    continue;
               if (OutOfRange(pos.Value,ref cam))
                    World.DestroyAsap(entity.EntityId);
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            var origin = new Vector2(8);
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Value + origin, drawable.SrcRect, drawable.Color, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
            }
        }

        private bool OutOfRange(Vector2 pos, ref CameraComponent cam)
        {
            var bounds = cam.ScreenRect;
            return pos.X < bounds.Left || pos.X > bounds.Right || pos.Y <= bounds.Top || pos.Y >= bounds.Bottom;
        }
    }
}