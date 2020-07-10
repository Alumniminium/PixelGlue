using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared;
using Pixel.Entities;
using System.Runtime.CompilerServices;

namespace Pixel.ECS.Systems
{

    public class EntityRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Entity Rendering System";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void AddEntity(Entity entity)
        {
            if (entity.Has<PositionComponent>() && entity.Has<DrawableComponent>())
                base.AddEntity(entity);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            var origin = new Vector2(8);
            for(int i = 0; i< Entities.Count; i++)
            {
                var entity = Entities[i];
                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (OutOfRange(pos.Value+drawable.SrcRect.Size.ToVector2()))
                    {
                        Scene.Destroy(entity);
                        continue;
                    }

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Value + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
           }
        }

        private bool OutOfRange(Vector2 pos)
        {
            return pos.X < Scene.Camera.SimulationRect.Left || pos.X > Scene.Camera.SimulationRect.Right || pos.Y <= Scene.Camera.SimulationRect.Top || pos.Y >= Scene.Camera.SimulationRect.Bottom;
        }
    }
}