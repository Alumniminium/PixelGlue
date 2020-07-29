using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pixel.ECS;
using Pixel.ECS.Components;
using Pixel.Helpers;
using Pixel.Scenes;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.zero
{
    public class MapShaderRenderer : PixelSystem
    {
        public override string Name => "Map Shader Renderer";
        public DateTime LastUpdate { get; set; }

        public const int VERTICES_PER_QUAD = 6;
        public const int TRIANGLES_PER_QUAD = 2;
        public static Effect effect;
        public int mapWidth = 100;
        public int mapHeight = 100;
        public List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
        public List<Texture2D> layerTextures = new List<Texture2D>();
        private VertexBuffer vertexBuffer;

        public MapShaderRenderer(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override void Initialize()
        {
            var Projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateOrthographicOffCenter(0, Global.ScreenWidth, Global.ScreenHeight, 0, 0, 1) * Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f);

            CreateTileMapFromSheet(mapWidth, mapHeight);
            CreateTileMapFromSheet(mapWidth, mapHeight);
            CreateTileMapFromSheet(mapWidth, mapHeight);

            var quads = vertexList.ToArray();
            vertexBuffer = new VertexBuffer(Global.Device, typeof(VertexPositionColorTexture), quads.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(quads);

            effect = Global.ContentManager.Load<Effect>("MapRenderer");
            effect.CurrentTechnique = effect.Techniques["QuadDraw"];
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["TextureA"].SetValue(AssetManager.GetTexture("pixel"));
            effect.Parameters["World"].SetValue(Matrix.Identity);

            IsActive = true;
        }

        public override void Update(float deltaTime)
        {
            LastUpdate = DateTime.Now;
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
            effect.Parameters["View"].SetValue(cam.Transform);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Global.Device.Clear(Color.CornflowerBlue);
            Global.Device.BlendState = BlendState.AlphaBlend; // AlphaBlend Opaque NonPremultiplied
            Global.Device.DepthStencilState = DepthStencilState.Default;
            Global.Device.SamplerStates[0] = SamplerState.PointClamp;
            Global.Device.RasterizerState = RasterizerState.CullClockwise;
            Vector3 cameraUp = Vector3.TransformNormal(new Vector3(0, -1, 0), Matrix.CreateRotationZ(0)) * 10f;
            Matrix World = Matrix.Identity;
            var player = Global.Player;
            Matrix View = Matrix.CreateLookAt(new Vector3(0,0, -1), new Vector3(0, 0, 0), cameraUp);

            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);

            Global.Device.SetVertexBuffer(vertexBuffer);
            var playerPos = player.Get<PositionComponent>().Value;
            var x = (int)playerPos.X;
            var y = (int)playerPos.Y;
            DrawTileMap(new Rectangle(x, y, 1000, 1000), 0, 3);
        }
        public void DrawTileMap(Rectangle TileRegionToDraw, int StartingLayer, int LayerLength)
        {
            int mapTilesLength = mapWidth * mapHeight;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                for (int layer = StartingLayer; layer < (StartingLayer + LayerLength); layer++)
                {
                    // Set the texture that this layer uses on the card.
                    effect.Parameters["TextureA"].SetValue(AssetManager.GetTexture("pixel"));

                    // Call apply the set the texture.
                    pass.Apply();

                    // Calculate the layer offset.
                    var layerVerticeOffset = mapTilesLength * layer * VERTICES_PER_QUAD;

                    // Increment down a column as we draw each row of tiles that is in view.
                    for (int y = TileRegionToDraw.Y; y < TileRegionToDraw.Bottom; y++)
                    {
                        // Calculate the starting index position for this row of tiles at the targeted layer within the 1 dimensional triangle buffer and the span of primitives to draw.
                        int startingVerticeIndex = ((y * mapWidth + TileRegionToDraw.X) * VERTICES_PER_QUAD) + layerVerticeOffset;
                        int primitiveSpanLength = TileRegionToDraw.Width * TRIANGLES_PER_QUAD;
                        // Tell the gpu to execute the drawing of this visbile row span of tiles in the shader.
                        Global.Device.DrawPrimitives(PrimitiveType.TriangleList, startingVerticeIndex, primitiveSpanLength);
                    }
                }
            }
        }
        public void CreateTileMapFromSheet(int mapWidth, int mapHeight)
        {
            layerTextures.Add(AssetManager.GetTexture("pixel"));
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    var t = WorldGen.GetTile(x, y);
                    var source = new Rectangle(0, 0, 16, 16);
                    var destination = new Rectangle(x * Global.TileSize, y * Global.TileSize, Global.TileSize, Global.TileSize);
                    Vector2 tl = source.Location.ToVector2() / Global.TileSize;
                    Vector2 tr = new Vector2(source.Right, source.Top) / Global.TileSize;
                    Vector2 br = new Vector2(source.Right, source.Bottom) / Global.TileSize;
                    Vector2 bl = new Vector2(source.Left, source.Bottom) / Global.TileSize;
                    // t1
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), t.Color, tl));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Bottom, 0f), t.Color, bl));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), t.Color, br));
                    // t2
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), t.Color, br));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Top, 0f), t.Color, tr));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), t.Color, tl));
                }
            }
        }
    }
}