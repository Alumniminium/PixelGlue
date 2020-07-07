using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Entities.UI;
using Pixel.Entities;
using Shared;

namespace Pixel.ECS.Systems.UI
{
    public class UIRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "UI Render System";

        public override void AddEntity(Entity entity)
        {
            if (entity.Has<DrawableComponent>() && entity.Has<PositionComponent>())
                base.AddEntity(entity);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
            //foreach (var entity in Entities)
            //{
            //    Draw(spriteBatch, entity);
            //
            //    foreach (var sub in entity.Children)
            //        Draw(spriteBatch, sub);
            //}
        }

        private static void Draw(SpriteBatch spriteBatch, Entity entity)
        {
            if (entity is Textblock)
            {
                ref readonly var text = ref entity.Get<TextComponent>();
                ref readonly var childDrawable = ref entity.Get<DrawableComponent>();
                AssetManager.Fonts[text.FontName].DrawText(spriteBatch, childDrawable.DestRect.X, childDrawable.DestRect.Y, text.Text);
            }
            else if (entity is UIRectangle)
            {
                ref readonly var drawable = ref entity.Get<DrawableComponent>();
                var dest = new Rectangle(drawable.DestRect.X, drawable.DestRect.Y, drawable.DestRect.Width, drawable.DestRect.Height);
                spriteBatch.Draw(AssetManager.GetTexture(drawable.TextureName), dest, drawable.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
        }
    }
}