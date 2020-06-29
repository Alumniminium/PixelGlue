using Pixel.ECS;
using Pixel.ECS.Components;
using Pixel.Entities;
using Pixel.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Helpers;
using Pixel.Networking;
using PixelShared;

namespace Pixel.Scenes
{
    public class TestingScene2 : Scene
    {
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            base.LoadContent(cm);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(transformMatrix: Camera.Transform, samplerState: SamplerState.PointClamp);
            var origin = new Vector2(-23, 8);
            var overdraw = PixelShared.Pixel.TileSize * 2;

            foreach (var (_,entity) in Entities)
            {
                ref var drawable = ref entity.Get<DrawableComponent>();
                ref var pos = ref entity.Get<PositionComponent>();
               
                if (pos.Position.X < Camera.ScreenRect.Left - overdraw || pos.Position.X > Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Camera.ScreenRect.Top - overdraw || pos.Position.Y > Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position, drawable.SrcRect, Color.White, 0f,origin, Vector2.One, SpriteEffects.None, 0f);
                Global.DrawCalls++;
            }
          
            sb.End();

            base.Draw(sb);
        }
    }
}