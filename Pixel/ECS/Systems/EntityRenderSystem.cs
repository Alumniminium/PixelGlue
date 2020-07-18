using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using System.Runtime.CompilerServices;
using Pixel.Scenes;
using Pixel.Helpers;
using System;

namespace Pixel.ECS.Systems
{

    public class EntityRenderSystem : PixelSystem
    {

        public override string Name { get; set; } = "Entity Rendering System";
        public EntityRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

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

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Value + origin, drawable.SrcRect, drawable.Color, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                if (entityId == 1)
                    continue;
                var entity = World.Entities[entityId];
                ref readonly var pos = ref entity.Get<PositionComponent>();
                if (OutOfRange(pos.Value))
                    World.Destroy(entity.EntityId);
            }
        }

        private bool OutOfRange(Vector2 pos)
        {
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
            var bounds = cam.ScreenRect;
            return pos.X < bounds.Left || pos.X > bounds.Right || pos.Y <= bounds.Top || pos.Y >= bounds.Bottom;
        }
    }
}