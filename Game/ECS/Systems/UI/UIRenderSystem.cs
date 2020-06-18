using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Enums;
using PixelGlueCore.Entities.UI;

namespace PixelGlueCore.ECS.Systems.UI
{
    public class UIRenderSystem : IEntitySystem
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public UIScene Scene{get;set;}
        public UIRenderSystem(UIScene scene)
        {
            Scene=scene;
        }
        public void Update(float deltaTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch) // this should be scene independent I think
        {
            foreach (var (_, child) in Scene.Entities)
            {
                if (child.Parent != null || !child.Has<DrawableComponent>())
                    continue;

                ref readonly var drawable = ref child.Get<DrawableComponent>();
                spriteBatch.Draw(AssetManager.GetTexture(drawable.TextureName), drawable.DestRect, drawable.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                PixelGlue.DrawCalls++;

                if(child is Textblock)
                {
                    ref readonly var text = ref child.Get<TextComponent>();
                    ref readonly var childDrawable = ref child.Get<DrawableComponent>();
                    AssetManager.Fonts[text.FontName].DrawText(spriteBatch,childDrawable.DestRect.X,childDrawable.DestRect.Y,text.Text);
                }

                foreach (var sub in child.Children)
                {
                    if(sub is Textblock)
                    {
                        if (!sub.Has<TextComponent>())
                         continue;

                    ref readonly var text = ref sub.Get<TextComponent>();
                    ref readonly var pos = ref sub.Get<DrawableComponent>();
                    AssetManager.Fonts[text.FontName].DrawText(spriteBatch,drawable.DestRect.X+pos.DestRect.X,drawable.DestRect.Y+pos.DestRect.Y,text.Text);
                    }
                    else if (sub is UIRectangle)
                    {
                        if (!sub.Has<DrawableComponent>())
                        continue;

                    ref readonly var subDrawable = ref sub.Get<DrawableComponent>();
                    var dest = new Rectangle(drawable.DestRect.X + subDrawable.DestRect.X,drawable.DestRect.Y+subDrawable.DestRect.Y,subDrawable.DestRect.Width,subDrawable.DestRect.Height);
                    spriteBatch.Draw(AssetManager.GetTexture(drawable.TextureName), dest, subDrawable.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                    PixelGlue.DrawCalls++;
                    }                    
                }
            }
        }

        public void FixedUpdate(float deltaTime)
        {
        }
    }
}