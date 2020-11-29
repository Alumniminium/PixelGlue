using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Pixel.Helpers;
using Shared.ECS.Components;
using Shared;
using Pixel.Scenes;
using Shared.Maths;

namespace Pixel.ECS.Systems
{
    public class EntityRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Entity Rendering System";
        public Point Overdraw = new Point(Global.HalfVirtualScreenWidth, Global.HalfVirtualScreenHeight);

        public EntityRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override bool MatchesFilter(Entity entity) 
        {
            if(entity.Has<PositionComponent, DrawableComponent>())
            {
                ref readonly var pos = ref entity.Get<PositionComponent>();
                return true;//!OutOfRange(pos.Value);
            }
            return false;
        }
        public override void Draw(SpriteBatch sb)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref readonly var pos = ref entity.Get<PositionComponent>();

                //if (OutOfRange(pos.Value))
                //    continue;

                ref readonly var drw = ref entity.Get<DrawableComponent>();

                if (drw.DestRect.IsEmpty)
                    sb.Draw(AssetManager.GetTexture(drw.TextureName), pos.Value + drw.Origin, drw.SrcRect, drw.Color, pos.Rotation, drw.Origin, drw.Scale, SpriteEffects.None, 0f);
                else
                    sb.Draw(AssetManager.GetTexture(drw.TextureName), drw.DestRect, drw.SrcRect, drw.Color, 0f, drw.Origin, SpriteEffects.None, 0f);
            }
        }

        private bool OutOfRange(Vector2 pos)
        {
            ExtendBounds(out var xs, out var ys, out var xe, out var ye);
            return pos.X < xs || pos.X > xe || pos.Y <= ys || pos.Y >= ye;
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