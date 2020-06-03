using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Entities;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.Loaders.TiledSharp;

namespace PixelGlueCore.Scenes
{
    public class TestingScene2 : Scene
    {
        public override void Initialize()
        {
            Camera = new Camera();
            GameObjects.Add(0, (Camera)Camera);

            var player = new Player(256,256);
            GameObjects.Add(player.UniqueId, player);
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {            
            Map = TmxMapRenderer.Load("../Build/Content/RuntimeContent/indoorstest.tmx",cm);
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt",cm);
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
            var origin = new Vector2(-23, 8);
            var overdraw = Map.TileWidth * 2;

            renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 0, Camera);
            renderedObjectsCounter += TmxMapRenderer.Draw(sb, Map, 1, Camera);
            foreach (var kvp in GameObjects)
            {
                if (!kvp.Value.TryGetComponent<DrawableComponent>(out var drawable))
                    continue;
                if (!kvp.Value.TryGetComponent<PositionComponent>(out var pos))
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
            
            base.Draw(sb);
        }
    }
}