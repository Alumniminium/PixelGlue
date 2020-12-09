using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Pixel.Helpers;
using Shared.ECS.Components;
using Shared;
using Pixel.Scenes;
using Shared.Maths;

namespace Pixel.ECS.Systems.Rendering
{
    public class EntityRenderSystem : PixelSystem<PositionComponent,DrawableComponent>
    {
        public Point Overdraw = new Point(Global.HalfVirtualScreenWidth, Global.HalfVirtualScreenHeight);
        public EntityRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { Name = "Entity Rendering System";}
        public override void Draw(SpriteBatch sb)
        {
            foreach (var entityList in Entities)
            for(int i =0; i< entityList.Count; i++)
            {
                ref var entity = ref entityList[i];
                ref readonly var pos = ref entity.Get<PositionComponent>();;

                ref readonly var drw = ref entity.Get<DrawableComponent>();

                if (drw.DestRect.IsEmpty)
                    sb.Draw(AssetManager.GetTexture(drw.TextureName), pos.Value + drw.Origin, drw.SrcRect, drw.Color, pos.Rotation, drw.Origin, drw.Scale, SpriteEffects.None, 0f);
                else
                    sb.Draw(AssetManager.GetTexture(drw.TextureName), drw.DestRect, drw.SrcRect, drw.Color, 0f, drw.Origin, SpriteEffects.None, 0f);
            }
        }

        private void ExtendBounds(out int xs, out int ys, out int xe, out int ye)
        {
            ref readonly var cam = ref TestingScene.Player.Camera;
            var bounds = cam.ScreenRect;
            xs = bounds.Left - Overdraw.X;
            ys = bounds.Top - Overdraw.Y;
            xe = bounds.Right + Overdraw.X;
            ye = bounds.Bottom + Overdraw.Y;

            var start = PixelMath.ToGridPosition(xs,ys);
            var end = PixelMath.ToGridPosition(xs,ys);

            xs = (int)start.X;
            xe = (int)end.X;
            ys = (int)start.Y;
            ye = (int)end.Y;
        }
    }
}