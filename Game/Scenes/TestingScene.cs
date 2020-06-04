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

namespace PixelGlueCore.Scenes
{
    public class TestingScene : Scene
    {
        public UIRectangle RedSquare = new UIRectangle(10,10,100,100,Color.Red);
        public override void Initialize()
        {
            Camera = new Camera();
            Entities.Add(0, (Camera)Camera);
            CreateEntity<Player>(1,new PositionComponent(1,256,256,0), new InputComponent(),new MoveComponent(1,64, 256, 256),new DrawableComponent(1,"character.png", new Rectangle(0, 2, 16, 16)),new CameraFollowTagComponent(1,1),new Networked(1));
            
            Systems.Add(new MoveSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new SmartFramerate(4));
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
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);

            int renderedObjectsCounter = 0;
            var overdraw = Map.TileWidth * 2;

            renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 0, Camera);
            renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 1, Camera);
            foreach (var kvp in Entities)
            {
                if (!TryGetComponent<DrawableComponent>(kvp.Key,out var drawable))
                    continue;
                if (!TryGetComponent<PositionComponent>(kvp.Key,out var pos))
                    continue;

                if (pos.Position.X < Camera.ScreenRect.Left - overdraw || pos.Position.X > Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Camera.ScreenRect.Top - overdraw || pos.Position.Y > Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.Textures[drawable.TextureName], new Rectangle((int)pos.IntegerPosition.X,(int)pos.IntegerPosition.Y,Map.TileWidth,Map.TileHeight), drawable.SrcRect, Color.White, 0f,Vector2.Zero, SpriteEffects.None, 0f);
                renderedObjectsCounter++;
            }
            renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 2, Camera);
            
            if(TryGetComponent<InputComponent>(1,out var input))
            {
                var pos = Camera.ScreenToWorld(input.Mouse.Position.ToVector2());
                RedSquare.RenderRect.X = (int)pos.X;
                RedSquare.RenderRect.Y=(int)pos.Y;
                sb.Draw(RedSquare.Texture,RedSquare.RenderRect,RedSquare.SourceRect,Color.White,0,Vector2.Zero,SpriteEffects.None, 0f);
            }
            
            sb.End();
            sb.Begin(samplerState: SamplerState.PointClamp);
            AssetManager.Fonts["profont"].Draw($"PixelGlue Engine (Objects: {(Map.TileArray[0].Length * Map.TileArray.Length) + Entities.Count}, Rendered: {renderedObjectsCounter})", new Vector2(16, 16), sb);
            AssetManager.Fonts["profont"].Draw($"Position: {Camera.ScreenRect.X},{Camera.ScreenRect.Y}", new Vector2(16, 164), sb);
            AssetManager.Fonts["profont"].Draw($"Camera Rect: {Camera.ScreenRect.X},{Camera.ScreenRect.Y} w{Camera.ScreenRect.Width},h{Camera.ScreenRect.Height}", new Vector2(16, 192), sb);
            AssetManager.Fonts["emoji"].Draw($"", new Vector2(PixelGlue.ScreenWidth/2, PixelGlue.ScreenHeight/2), sb);
            (Systems[^1] as SmartFramerate).Draw(sb);
            sb.End();
            base.Draw(sb);
        }
    }
}