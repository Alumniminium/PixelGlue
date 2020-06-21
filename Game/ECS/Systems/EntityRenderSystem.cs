using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.Enums;
using PixelGlueCore.Loaders;
using PixelGlueCore.Helpers;
using System;

namespace PixelGlueCore.ECS.Systems
{
    public class EntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene {get;set;}

        public EntityRenderSystem(GameScene scene)
        {
            Scene=scene;
        }
        public void Update(float timeSinceLastFrame)
        {
        }
        public void FixedUpdate(float timeSinceLastFrame)
        {
        }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;

            var overdraw = Scene.Map.TileWidth * 2;
            var origin = new Vector2(8, 8);
            PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 0, Scene.Camera);
            PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 1, Scene.Camera);
            foreach (var (_, entity) in Scene.Entities)
            {
                if (!entity.Has<DrawableComponent>() || !entity.Has<PositionComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.DrawCalls++;
            }
            if (Scene.Map?.Layers?.Count >= 2)
                PixelGlue.DrawCalls += TmxMapRenderer.Draw(sb, Scene.Map, 2, Scene.Camera);
        }
    }
    public class ProceduralEntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene {get;set;}

        public ProceduralEntityRenderSystem(GameScene scene)
        {
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(18,78,137,255)),"deep_water");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(0,153,219,255)),"water");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(44,232,245,255)),"shallow_water");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(234,212,170,255)),"sand");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(194,133,105,255)),"dirt");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(99,199,77,255)),"plains");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(62,137,72,255)),"grass");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(38,92,66,255)),"trees");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(184,111,80,255)),"rock");
            AssetManager.LoadTexture(Texture2DExt.Blank(1,1,Color.FromNonPremultiplied(192,203,220,255)),"snow");
            Scene=scene;
        }
        public void Update(float timeSinceLastFrame)
        {
        }
        public void FixedUpdate(float timeSinceLastFrame)
        {
        }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;

            var overdraw = Scene.Map.TileWidth * 2;
            var origin = new Vector2(8, 8);

            //for(int x = Scene.Camera.ScreenRect.Left; x < Scene.Camera.ScreenRect.Right; x+=16)
            for(int x = Scene.Camera.ScreenRect.Left-16; x < Scene.Camera.ScreenRect.Right+16; x+=16)
            {
                //for(int y = Scene.Camera.ScreenRect.Top; y < Scene.Camera.ScreenRect.Bottom; y+=16)
                for(int y =Scene.Camera.ScreenRect.Top-16; y < Scene.Camera.ScreenRect.Bottom+16; y+=16)
                {
                    var val = PixelGlue.Noise.GetSimplexFractal(x/32,y/32);
                    val += (float)(0.75 * PixelGlue.Noise.GetSimplexFractal(x/16,y/16));
                    val += (float)(0.50 * PixelGlue.Noise.GetSimplexFractal(x/8,y/8));
                    val += (float)(0.25 * PixelGlue.Noise.GetSimplexFractal(x/4,y/4));
                    val += (float)(0.125 * PixelGlue.Noise.GetSimplexFractal(x/2,y/2));
                    //val = (float)Math.Pow(val,2.4);
                    if(val > 0.97)
                    {     
                        sb.Draw(AssetManager.GetTexture("snow"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if( val > 0.75)
                    {     
                        sb.Draw(AssetManager.GetTexture("rock"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if( val > 0.6)
                    {     
                        sb.Draw(AssetManager.GetTexture("trees"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if( val > 0.33)
                    {     
                        sb.Draw(AssetManager.GetTexture("grass"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if( val > -0.23)
                    {     
                        sb.Draw(AssetManager.GetTexture("plains"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if (val > -0.5f)
                    {
                        sb.Draw(AssetManager.GetTexture("dirt"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if (val > -0.6)
                    {
                        sb.Draw(AssetManager.GetTexture("sand"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if (val > -0.7f)
                    {
                        sb.Draw(AssetManager.GetTexture("shallow_water"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else if (val > -0.8)
                    {
                        sb.Draw(AssetManager.GetTexture("water"),new Rectangle(x,y,16,16),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    else
                    {
                        sb.Draw(AssetManager.GetTexture("deep_water"),new Rectangle(x,y,8,8),new Rectangle(0,0,1,1),Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                    }
                    PixelGlue.DrawCalls++;
                }
            }

            
            foreach (var (_, entity) in Scene.Entities)
            {
                if (!entity.Has<DrawableComponent>() || !entity.Has<PositionComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.DrawCalls++;
            }
        }
    }
}