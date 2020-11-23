using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Helpers;
using Pixel.Scenes;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.ECS.Systems
{
    public class BackgroundRenderSystem : PixelSystem
    {
        public override string Name { get; set; } = "Background Rendering System";
        public Point Overdraw = new Point(Global.HalfVirtualScreenWidth, Global.HalfVirtualScreenHeight);
        public BackgroundRenderSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }
        public Texture2D Pixel;
        public Color[] Colors;

        public override void Initialize()
        {
            Pixel = AssetManager.GetTexture("pixel");
            Colors = new[] { Color.Green, Color.DarkGreen, Color.SaddleBrown, Color.Gray };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw(SpriteBatch sb)
        {
            ExtendBounds(out int xs, out int ys, out int xe, out int ye);
            var color = Colors[0];

            sb.GraphicsDevice.Clear(color);

            for (int x = xs; x < xe; x += Global.TileSize)
            {
                if (x % 2 == 0)
                    continue;
                for (int y = ys; y < ye; y += Global.TileSize)
                {
                    if (y % 2 == 0)
                        continue;
                    var xx = x / Global.TileSize;
                    var yy = y / Global.TileSize;

                    var noise = NoiseGen.WhiteNoise.GetWhiteNoiseInt(xx, yy);

                    if (noise > 0.95)
                        color = Colors[2];
                    else if (noise > 0.9)
                        color = Colors[1];
                    else
                        continue;

                    var dst = new Rectangle(x, y, Global.TileSize, Global.TileSize);
                    sb.Draw(Pixel, dst, Pixel.Bounds, color, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
            }
        }

        private void ExtendBounds(out int xs, out int ys, out int xe, out int ye)
        {
            ref readonly var cam = ref TestingScene.Player.Get<CameraComponent>();
            var bounds = cam.ScreenRect;
            var overdrawVect = Overdraw.ToVector2() / cam.Zoom;
            var overdraw = overdrawVect.ToPoint();
            xs = bounds.Left - overdraw.X;
            ys = bounds.Top - overdraw.Y;
            xs /= Global.TileSize;
            xs *= Global.TileSize;
            ys /= Global.TileSize;
            ys *= Global.TileSize;

            xe = bounds.Right + overdraw.X;
            ye = bounds.Bottom + overdraw.Y;
            xe /= Global.TileSize;
            xe *= Global.TileSize;
            ye /= Global.TileSize;
            ye *= Global.TileSize;
        }
    }
}