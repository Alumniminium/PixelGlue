using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.Helpers;
using PixelGlueCore.Networking;

namespace PixelGlueCore.Scenes
{
    public class TestingScene2 : Scene
    {
        public override void Initialize()
        {
            Camera = new Camera();
            Entities.TryAdd(0, (Camera)Camera);

            //var entity = CreateEntity<Player>(1,new PositionComponent(1,256,256,0), new InputComponent(),new MoveComponent(1,64, 256, 256),new CameraFollowTagComponent(1,1),new Networked(1));
            //var drawabel = new DrawableComponent(1,"character.png", new Rectangle(0, 2, 16, 16));

            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {
            Map = TmxMapRenderer.Load("../Build/Content/RuntimeContent/indoorstest.tmx",cm);
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt",cm);
            base.LoadContent(cm);
        }

        public override void Draw(Scene scene, SpriteBatch sb)
        {
            sb.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);

            int renderedObjectsCounter = 0;
            var origin = new Vector2(-23, 8);
            var overdraw = Map.TileWidth * 2;

            renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 0, Camera);
            renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 1, Camera);
            foreach (var kvp in Entities)
            {
                if (!TryGetDrawableComponent(kvp.Key,out var drawable))
                    continue;
                if (!TryGetComponent<PositionComponent>(kvp.Key,out var pos))
                    continue;

                if (pos.Position.X < Camera.ScreenRect.Left - overdraw || pos.Position.X > Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Camera.ScreenRect.Top - overdraw || pos.Position.Y > Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.Textures[drawable.TextureName], pos.Position, drawable.SrcRect, Color.White, 0f,origin, Vector2.One, SpriteEffects.None, 0f);
                renderedObjectsCounter++;
            }
            //renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 2, Camera);

            sb.End();

            base.Draw(this,sb);
        }
    }
}