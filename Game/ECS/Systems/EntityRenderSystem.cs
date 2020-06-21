using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Enums;
using PixelGlueCore.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using static PixelGlueCore.Loaders.FastNoise;

namespace PixelGlueCore.ECS.Systems
{
    public class ProceduralEntityRenderSystem : IEntitySystem
    {
        public string Name { get; set; } = "Procedural Entity Rendering System";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }
        public GameScene Scene { get; set; }
        public static ConcurrentDictionary<(int x, int y), DrawableComponent> Tiles = new ConcurrentDictionary<(int x, int y), DrawableComponent>();
        public static Dictionary<(int x, int y), bool> Tiles2 = new Dictionary<(int x, int y), bool>();
        public Thread[] Prefetcher = new Thread[128];
        public ConcurrentStack<(int x, int y)>[] Queue = new ConcurrentStack<(int x, int y)>[128];
        public ProceduralEntityRenderSystem(GameScene scene)
        {
            AssetManager.LoadTexture(TextureGen.Pixel("#124E89"), "deep_water");
            AssetManager.LoadTexture(TextureGen.Pixel("#0099DB"), "water");
            AssetManager.LoadTexture(TextureGen.Pixel("#4CB7E5"), "shallow_water");
            AssetManager.LoadTexture(TextureGen.Pixel("#EAD4AA"), "sand");
            AssetManager.LoadTexture(TextureGen.Pixel("#E8B796"), "sand2");
            AssetManager.LoadTexture(TextureGen.Pixel("#E4A672"), "sand3");
            AssetManager.LoadTexture(TextureGen.Pixel("#C28569"), "dirt");
            AssetManager.LoadTexture(TextureGen.Pixel("#63C74D"), "plains");
            AssetManager.LoadTexture(TextureGen.Pixel("#3E8948"), "grass");
            AssetManager.LoadTexture(TextureGen.Pixel("#265C42"), "trees");
            AssetManager.LoadTexture(TextureGen.Pixel("#B86F50"), "rock");
            AssetManager.LoadTexture(TextureGen.Pixel("#C0CBDC"), "snow");
            Scene = scene;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            for (int i = 0; i < Prefetcher.Length; i++)
            {
                Queue[i]=new ConcurrentStack<(int x, int y)>();
                Prefetcher[i] = new Thread(new ParameterizedThreadStart(Load))
                {
                    IsBackground = true,
                    Priority=ThreadPriority.Lowest,
                };
                Prefetcher[i].Start(i);
            }
        }
        public void Load(object idobj)
        {
            int id = (int)idobj;
            while (true)
            {
                while (Queue[id].TryPop(out var t))
                {
                    var (x,y) = t;
                    if (Tiles.ContainsKey((x, y)))
                        continue;
                        
                    Tiles.TryAdd((x, y), GenerateTerrain(x, y));
                    Thread.Sleep(1);
                }
                Thread.Sleep(1);
            }
        }

        private static DrawableComponent GenerateTerrain(int x, int y)
        {
            DrawableComponent d;
            var srcRect = new Rectangle(0, 0, 1, 1);
            var dstRect = new Rectangle(x, y, 16, 16);
            PixelGlue.Noise.SetSeed(1337);
            PixelGlue.Noise.SetNoiseType(NoiseType.Simplex);
            PixelGlue.Noise.SetInterp(Interp.Linear);

            var val = PixelGlue.Noise.GetNoise(x / 32, y / 32, PixelGlue.Z / 32);
            val += 0.5f * PixelGlue.Noise.GetNoise(x / 16, y / 16, PixelGlue.Z / 32);
            val += 0.15f * PixelGlue.Noise.GetNoise(x / 4, y / 4, PixelGlue.Z / 32);

            if (val > 0.94)
                d = new DrawableComponent(0, "snow", srcRect, dstRect);
            else if (val > 0.70)
                d = new DrawableComponent(0, "rock", srcRect, dstRect);
            else if (val > 0.4)
                d = new DrawableComponent(0, "trees", srcRect, dstRect);
            else if (val > 0.30)
                d = new DrawableComponent(0, "grass", srcRect, dstRect);
            else if (val > -0.20)
                d = new DrawableComponent(0, "plains", srcRect, dstRect);
            else if (val > -0.4f)
                d = new DrawableComponent(0, "dirt", srcRect, dstRect);
            else if (val > -0.41 && val < 0.42)
                d = new DrawableComponent(0, "sand", srcRect, dstRect);
            else if (val > -0.5f)
                d = new DrawableComponent(0, "shallow_water", srcRect, dstRect);
            else if (val > -0.7)
                d = new DrawableComponent(0, "water", srcRect, dstRect);
            else
                d = new DrawableComponent(0, "deep_water", srcRect, dstRect);
            return d;
        }

        public void Update(float timeSinceLastFrame) { }
        public void FixedUpdate(float timeSinceLastFrame) { }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;

            var overdraw = Scene.Map.TileWidth * 2;
            int last=0;
            for (int x = Scene.Camera.ScreenRect.Left - overdraw; x < Scene.Camera.ScreenRect.Right + overdraw; x += 16)//Scene.Map.TileWidth)
                for (int y = Scene.Camera.ScreenRect.Top - overdraw; y < Scene.Camera.ScreenRect.Bottom + overdraw; y += 16)//Scene.Map.TileHeight)
                {
                    if (!Tiles.TryGetValue((x, y), out var terrainTile))
                    {
                        if(Tiles2.TryGetValue((x,y),out _))
                            continue;
                        Queue[last].Push((x, y));
                        last++;
                        if(last==Queue.Length)
                        last=0;
                        continue;
                    }
                    //  var  terrainTile = Terrain(x, y);
                    sb.Draw(AssetManager.GetTexture(terrainTile.TextureName), terrainTile.DestRect, terrainTile.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    PixelGlue.DrawCalls++;

                    //if (!Tiles2.TryGetValue((x, y), out var riverTile))
                    //    riverTile = River(x, y);
                    //if (!riverTile.HasValue)
                    //    continue;

                    //sb.Draw(AssetManager.GetTexture(riverTile.Value.TextureName), riverTile.Value.DestRect, riverTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    //PixelGlue.DrawCalls++;
                }
            RenderEntities(sb, overdraw);
        }

        private void RenderEntities(SpriteBatch sb, int overdraw)
        {
            var origin = new Vector2(8, 8);
            foreach (var (_, entity) in Scene.Entities)
            {
                if (!entity.Has<DrawableComponent>() || !entity.Has<PositionComponent>())
                    continue;

                ref readonly var pos = ref entity.Get<PositionComponent>();
                ref readonly var drawable = ref entity.Get<DrawableComponent>();

                if (pos.Position.X < Scene.Camera.ScreenRect.Left - overdraw || pos.Position.X > Scene.Camera.ScreenRect.Right + overdraw)
                    continue;
                if (pos.Position.Y < Scene.Camera.ScreenRect.Top - overdraw || pos.Position.Y > Scene.Camera.ScreenRect.Bottom + overdraw)
                    continue;

                sb.Draw(AssetManager.GetTexture(drawable.TextureName), pos.Position + origin, drawable.SrcRect, Color.White, pos.Rotation, origin, Vector2.One, SpriteEffects.None, 0f);
                PixelGlue.DrawCalls++;
            }
        }
        public float Biome(int x, int y)
        {
            float x2 = x;
            float y2 = y;
            PixelGlue.Noise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            PixelGlue.Noise.SetCellularReturnType(CellularReturnType.CellValue);
            PixelGlue.Noise.SetCellularJitter(0.4f);
            PixelGlue.Noise.SetSeed(965678457);
            PixelGlue.Noise.SetGradientPerturbAmp(22);
            PixelGlue.Noise.SetFrequency(0.02f);
            PixelGlue.Noise.GradientPerturbFractal(ref x2, ref y2);
            var val = PixelGlue.Noise.GetCellular(x2 * 0.004f, y2 * 0.004f);

            if (val > 0.5)
                return 1;
            else if (val > 0)
                return 2;
            else if (val > -0.5)
                return 4;
            else
                return 5;
        }
    }
}