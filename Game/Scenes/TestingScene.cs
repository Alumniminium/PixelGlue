using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.ECS.Systems;
using System.Collections.Generic;
using PixelGlueCore.Networking;
using System.Linq;

namespace PixelGlueCore.Scenes
{
    public class TestingScene : Scene
    {
        public UIRectangle RedSquare = new UIRectangle(10,10,16,16,Color.Red);
        public override void Initialize()
        {
            Camera = new Camera();
            Entities.TryAdd(0, (Camera)Camera);
            Systems.Add(new MoveSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new SmartFramerate(4));
            Systems.Add(new EntityRenderSystem());
            Systems.Add(new DbgBoundingBoxRenderSystem());
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {            
            Map = TmxMapRenderer.Load("../Build/Content/RuntimeContent/island.tmx",cm);
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt",cm);
            AssetManager.LoadTexture("selectionrect4x4",cm);
            base.LoadContent(cm);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(Scene scene, SpriteBatch sb)
        {
            sb.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);

            if(TryGetComponent<InputComponent>(out var input))
            {
                var pos = Camera.ScreenToWorld(input.Mouse.Position.ToVector2());
                RedSquare.RenderRect.X = (int)pos.X;
                RedSquare.RenderRect.Y=(int)pos.Y;
                sb.Draw(RedSquare.Texture,RedSquare.RenderRect,RedSquare.SourceRect,Color.White,0,Vector2.Zero,SpriteEffects.None, 0f);
            }
            
            sb.End();
            sb.Begin(samplerState: SamplerState.PointClamp);
            //AssetManager.Fonts["profont"].Draw($"PixelGlue Engine (Objects: {(Map.TileArray[0].Length * Map.TileArray.Length) + Entities.Count + Components.Values.Sum(p=>p.Count)}, Rendered: {renderedObjectsCounter})", new Vector2(16, 16), sb);
            AssetManager.Fonts["profont"].Draw($"Position: {Camera.ScreenRect.X},{Camera.ScreenRect.Y}", new Vector2(16, 164), sb);
            sb.End();
            base.Draw(this, sb);
        }
    }
}