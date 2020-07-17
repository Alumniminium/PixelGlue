using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS.Components;
using Pixel.Helpers;
using Pixel.Scenes;
using Shared;
using Shared.ECS;

namespace Pixel.ECS.Systems
{
    public class WorldRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "World Rendering System";
        public Point Overdraw = new Point(Global.TileSize * 4, Global.TileSize * 2);

        public WorldRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public Scene Scene => SceneManager.ActiveScene;
        public Texture2D Pixel;

        public override void Initialize()
        {
            Pixel = AssetManager.GetTexture("pixel");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            ExtendBounds(out int xs, out int ys, out int xe, out int ye);

            for (int x = xs; x < xe; x += Global.TileSize)
                for (int y = ys; y < ye; y += Global.TileSize)
                {
                    var tile = Chunkinator.GetTile(x, y);
                    if (tile == null)
                        continue;
                    sb.Draw(Pixel, tile.Dst, Pixel.Bounds, tile.Color, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
        }

        private void ExtendBounds(out int xs, out int ys, out int xe, out int ye)
        {
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
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