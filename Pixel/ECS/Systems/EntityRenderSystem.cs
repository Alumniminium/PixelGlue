using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Shared.ECS;
using Pixel.Helpers;
using Shared.ECS.Components;
using Shared;

namespace Pixel.ECS.Systems
{
    public class EntityRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Entity Rendering System";
        public Point Overdraw = new Point(Global.HalfVirtualScreenWidth, Global.HalfVirtualScreenHeight);

        public EntityRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public override void AddEntity(int entityId)
        {
            ref readonly var entity = ref World.GetEntity(entityId);
            if (entity.Has<PositionComponent, DrawableComponent>())
                base.AddEntity(entityId);
        }
        public override void Update(float deltaTime)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref readonly var pos = ref entity.Get<PositionComponent>();

                if (entityId == 1 || entity.Parent != 0)
                    continue;

                //if (OutOfRange(pos.Value))
                //    World.DestroyAsap(entity.EntityId);
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            foreach (var entityId in Entities)
            {
                ref readonly var entity = ref World.GetEntity(entityId);
                ref readonly var pos = ref entity.Get<PositionComponent>();
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
            ref readonly var cam = ref Global.Player.Get<CameraComponent>();
            var bounds = cam.ScreenRect;
            xs = bounds.Left - Overdraw.X;
            ys = bounds.Top - Overdraw.Y;
            xs /= Global.TileSize;
            xs *= Global.TileSize;
            ys /= Global.TileSize;
            ys *= Global.TileSize;

            xe = bounds.Right + Overdraw.X;
            ye = bounds.Bottom + Overdraw.Y;
            xe /= Global.TileSize;
            xe *= Global.TileSize;
            ye /= Global.TileSize;
            ye *= Global.TileSize;
        }
    }
}