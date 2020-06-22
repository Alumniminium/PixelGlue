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
        }
        public void Update(float timeSinceLastFrame) { }
        public void FixedUpdate(float timeSinceLastFrame) { }
        public void Draw(SpriteBatch sb)
        {
            if (Scene.Camera == null)
                return;
            var overdraw = Scene.Map.TileWidth;
            for (int x = Scene.Camera.ScreenRect.Left - overdraw; x < Scene.Camera.ScreenRect.Right + overdraw; x += 128)//Scene.Map.TileWidth)
                for (int y = Scene.Camera.ScreenRect.Top - overdraw; y < Scene.Camera.ScreenRect.Bottom + overdraw; y += 128)//Scene.Map.TileHeight)
                {
                    DrawableComponent? terrainTile = WorldGen.GetTileLayerZero(x, y);
                    DrawableComponent? riverTile = WorldGen.GetTileLayerOne(x, y);
                    if (terrainTile.HasValue)
                    {
                        sb.Draw(AssetManager.GetTexture(terrainTile.Value.TextureName), terrainTile.Value.DestRect, terrainTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        PixelGlue.DrawCalls++;
                    }
                    if (riverTile.HasValue && terrainTile.HasValue && !(terrainTile.Value.TextureName == "water" || terrainTile.Value.TextureName == "shallow_water" || terrainTile.Value.TextureName == "deep_water" || terrainTile.Value.TextureName == riverTile.Value.TextureName))
                    {
                        sb.Draw(AssetManager.GetTexture(riverTile.Value.TextureName), riverTile.Value.DestRect, riverTile.Value.SrcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        PixelGlue.DrawCalls++;
                    }
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
    }
    public static class WorldGen
    {
        public static ConcurrentDictionary<(int x, int y), bool> Tiles2 = new ConcurrentDictionary<(int x, int y), bool>();
        public static Thread[] Prefetcher = new Thread[2];
        public static ConcurrentStack<(int x, int y)>[] Queue = new ConcurrentStack<(int x, int y)>[2];
        public static ConcurrentDictionary<(int x, int y), DrawableComponent?> LayerZero = new ConcurrentDictionary<(int x, int y), DrawableComponent?>();
        public static ConcurrentDictionary<(int x, int y), DrawableComponent?> LayerOne = new ConcurrentDictionary<(int x, int y), DrawableComponent?>();
        public static Loaders.FastNoise BiomeNoise;
        public static Loaders.FastNoise PlainNoise;
        public static Loaders.FastNoise DesertNoise;
        public static Loaders.FastNoise RiverNoise;
        public static Rectangle srcRect = new Rectangle(0, 0, 1, 1);
        static WorldGen()
        {
            BiomeNoise = new Loaders.FastNoise(203414084);
            BiomeNoise.SetNoiseType(NoiseType.Cellular);
            BiomeNoise.SetInterp(Interp.Linear);
            BiomeNoise.SetFrequency(0.00004f);
            BiomeNoise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            BiomeNoise.SetCellularReturnType(CellularReturnType.CellValue);
            BiomeNoise.SetCellularJitter(0.3f);
            BiomeNoise.SetGradientPerturbAmp(1000);
            BiomeNoise.SetGradientFrequency(0.0004f);

            RiverNoise = new Loaders.FastNoise(203414084);
            RiverNoise.SetNoiseType(NoiseType.Cellular);
            RiverNoise.SetInterp(Interp.Linear);
            RiverNoise.SetFrequency(0.00004f);
            RiverNoise.SetCellularDistanceFunction(CellularDistanceFunction.Natural);
            RiverNoise.SetCellularReturnType(CellularReturnType.Distance2Div);
            RiverNoise.SetCellularJitter(0.3f);
            RiverNoise.SetGradientPerturbAmp(1000);
            RiverNoise.SetGradientFrequency(0.0004f);

            PlainNoise = new Loaders.FastNoise(1337);
            PlainNoise.SetNoiseType(NoiseType.SimplexFractal);
            PlainNoise.SetInterp(Interp.Quintic);
            PlainNoise.SetFrequency(0.001f);

            DesertNoise = new Loaders.FastNoise(30);
            DesertNoise.SetNoiseType(NoiseType.SimplexFractal);
            DesertNoise.SetInterp(Interp.Quintic);
            DesertNoise.SetFrequency(0.0015f);

            for (int i = 0; i < Prefetcher.Length; i++)
            {
                Queue[i] = new ConcurrentStack<(int x, int y)>();
                Prefetcher[i] = new Thread(new ParameterizedThreadStart(Load))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                };
                Prefetcher[i].Start(i);
            }
        }
        public static void Load(object idobj)
        {
            int id = (int)idobj;
            while (true)
            {
                while (Queue[id].TryPop(out var t))
                {
                    var (x, y) = t;
                    if (LayerZero.ContainsKey((x, y)))
                        continue;
                    var (terrain,river) = WorldGen.Generate(x, y);
                    LayerZero.TryAdd((x, y), terrain);
                    LayerOne.TryAdd((x, y), river);
                    Tiles2.TryRemove((x, y), out _);
                    //Thread.Sleep(1);
                }
                Thread.Sleep(1);
            }
        }
        public static (DrawableComponent? terrain,DrawableComponent? river) Generate(int x, int y)
        {
            var dstRect = new Rectangle(x, y, 16, 16);
            float x2, y2;
            x2 = x;
            y2 = y;
            RiverNoise.GradientPerturbFractal(ref x2, ref y2);
            var terrain = GenerateBiome(x2, y2, dstRect);
            var river = GenerateRiver(x2, y2, dstRect);
            return (terrain,river);
        }
        public static DrawableComponent? GenerateRiver(float x, float y, Rectangle dstRect)
        {
            var val = RiverNoise.GetNoise(x, y);

            if (val >= 0.98f)
                return new DrawableComponent(0, "water", srcRect, dstRect);
            else if (val > 0.96f)
                return new DrawableComponent(0, "shallow_water", srcRect, dstRect);
            else if (val > 0.95f)
                return new DrawableComponent(0, "dirt", srcRect, dstRect);
            else
                return null;
        }
        public static DrawableComponent? GenerateBiome(float x, float y, Rectangle dstRect)
        {
            var biome = BiomeNoise.GetNoise(x, y);

            if (biome > 0.5)
                return GeneratePlains(x, y, dstRect);
            else if (biome > 0)
                return GenerateDesert(x, y, dstRect);
            else if (biome < 0)
                return GenerateMountains(x, y, dstRect);
            else
                return GenerateMountains(x, y, dstRect);
        }

        private static DrawableComponent? GenerateMountains(float x, float y, Rectangle dstRect)
        {
            return null;
        }
        private static DrawableComponent? GenerateDesert(float x, float y, Rectangle dstRect)
        {
            var val = DesertNoise.GetNoise(x / 32, y / 32);
            val += 0.5f * DesertNoise.GetNoise(x / 16, y / 16);
            val += 0.15f * DesertNoise.GetNoise(x / 8, y / 8);
            //val += 0.75f * RiverNoise.GetNoise(x, y);

            if (val > 0.4f)
                return new DrawableComponent(0, "sand", srcRect, dstRect);
            else if (val > 0f)
                return new DrawableComponent(0, "sand2", srcRect, dstRect);
            else if (val > -0.6)
                return new DrawableComponent(0, "sand3", srcRect, dstRect);
            else
                return new DrawableComponent(0, "dirt", srcRect, dstRect);
        }
        private static DrawableComponent? GenerateSwamp(float x, float y)
        {
            return null;
        }

        private static DrawableComponent? GeneratePlains(float x, float y, Rectangle dstRect)
        {
            var val = PlainNoise.GetNoise(x / 32, y / 32);
            val += 0.75f * PlainNoise.GetNoise(x / 16, y / 16);
            val += 0.25f * PlainNoise.GetNoise(x / 8, y / 8);
            //val += 0.75f * RiverNoise.GetNoise(x, y);

            if (val > 0.8)
                return new DrawableComponent(0, "trees", srcRect, dstRect);
            else if (val > 0.2)
                return new DrawableComponent(0, "grass", srcRect, dstRect);
            else if (val > -0.4)
                return new DrawableComponent(0, "plains", srcRect, dstRect);
            else if (val > -0.45)
                return new DrawableComponent(0, "dirt", srcRect, dstRect);
            else if (val > -0.5)
                return new DrawableComponent(0, "shallow_water", srcRect, dstRect);
            else
                return new DrawableComponent(0, "water", srcRect, dstRect);
        }
        public static int last;
        internal static DrawableComponent? GetTileLayerZero(int x, int y)
        {
            x /= 16;
            x *= 16;
            y /= 16;
            y *= 16;
            if (!LayerZero.TryGetValue((x, y), out var terrainTile))
            {
                if (Tiles2.TryGetValue((x, y), out _))
                    return null;
                Tiles2.TryAdd((x, y), false);
                Queue[last].Push((x, y));
                last++;
                if (last == Queue.Length)
                    last = 0;
            }
            return terrainTile;
        }
        internal static DrawableComponent? GetTileLayerOne(int x, int y)
        {
            x /= 16;
            x *= 16;
            y /= 16;
            y *= 16;
            if (!LayerOne.TryGetValue((x, y), out var terrainTile))
            {
                if (Tiles2.TryGetValue((x, y), out _))
                    return null;
                Tiles2.TryAdd((x, y), false);
                Queue[last].Push((x, y));
                last++;
                if (last == Queue.Length)
                    last = 0;
            }
            return terrainTile;
        }
    }
}