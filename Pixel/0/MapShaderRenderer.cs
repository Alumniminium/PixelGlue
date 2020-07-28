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

namespace Pixel.zero
{
    public class MapShaderRenderer : PixelSystem
    {
        public override string Name => "Map Shader Renderer";
        public DateTime LastUpdate { get; set; }

        public const int VERTICES_PER_QUAD = 6;
        public const int TRIANGLES_PER_QUAD = 2;
        public static Effect effect;
        public int mapWidth = 500;
        public int mapHeight = 500;
        public List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
        public List<Texture2D> layerTextures = new List<Texture2D>();
        private VertexBuffer vertexBuffer;
        public Matrix Projection;
        private Vector3 camera2DScrollPosition;
        private Vector3 camera2DScrollLookAt;

        public MapShaderRenderer(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public override void Initialize()
        {
            CreateTileMapFromSheet(mapWidth, mapHeight);
            CreateTileMapFromSheet(mapWidth, mapHeight);
            CreateTileMapFromSheet(mapWidth, mapHeight);

            CreateBuffers();
            SetBuffers();

            effect = Global.ContentManager.Load<Effect>("MapRenderer");
            effect.CurrentTechnique = effect.Techniques["QuadDraw"];

            SetProjectionMatrix();
            SetTexture(AssetManager.GetTexture("pixel"));

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
            spriteBatch.End();
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
            spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: cam.Transform);

            Global.Device.Clear( Color.CornflowerBlue  );

            // We view the current scrolled position in pixels at.
            SetCameraTileMapPixelPosition2D(cam.ScreenRect.Location.ToVector2(), Global.TileSize,  Global.TileSize);

            // Were are we on the tile map and how many tiles do we draw in relation to the scrolled world position.
            var mapVisibleDrawRect = cam.ScreenRect;//new Rectangle((int)cam.X, (int)mapViewedScrollPos.Y, Global.ScreenWidth/ Global.TileSize,Global.ScreenHeight/ Global.TileSize);

            // set states and effect up.
            SetStates();
            SetUpEffect();

            // Draw all tiles that are valid from start to end layers.
            DrawTileMap(mapVisibleDrawRect, 0 , layerTextures.Count);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred,samplerState: SamplerState.PointClamp, transformMatrix: cam.Transform);
        }
        public void SetCameraTileMapPixelPosition2D(Vector2 tilePosition, int currentTileMapDestinationWidth, int currentTileMapDestinationHeight)
        {
            var x = tilePosition.X * (float)currentTileMapDestinationWidth;
            var y = tilePosition.Y * (float)currentTileMapDestinationHeight;
            SetCameraPixelPosition2D( x, y);
        }
        private void SetCameraPixelPosition2D(float x, float y)
        {
            camera2DScrollPosition.X = x;
            camera2DScrollPosition.Y = y;
            camera2DScrollPosition.Z = -1;
            camera2DScrollLookAt.X = x;
            camera2DScrollLookAt.Y = y;
            camera2DScrollLookAt.Z = 0;
        }
        public void DrawTileMap(Rectangle TileRegionToDraw, int StartingLayer, int LayerLength)
        {
            // The total map tiles
            int mapTilesLength = mapWidth * mapHeight;

            // Ensure the buffer is set we could have different buffers for each layer but instead i put it all in one buffer.
            SetBuffers();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Loop thru the layers we are going to draw.
                for (int layer = StartingLayer; layer < (StartingLayer + LayerLength); layer++)
                {
                    // Set the texture that this layer uses on the card.
                    SetTexture(layerTextures[layer]);

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
        public void SetTexture(Texture2D texture)
        {
            effect.Parameters["TextureA"].SetValue(texture);
        }

        public void SetBuffers()
        {
            Global.Device.SetVertexBuffer(vertexBuffer);
        }
        public void SetProjectionMatrix()
        {
            Viewport viewport = Global.Device.Viewport;
            Projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            effect.Parameters["Projection"].SetValue(Projection);
        }
        public void SetStates()
        {
            Global.Device.BlendState = BlendState.AlphaBlend; // AlphaBlend Opaque NonPremultiplied
            Global.Device.DepthStencilState = DepthStencilState.Default;
            Global.Device.SamplerStates[0] = SamplerState.PointClamp;
            Global.Device.RasterizerState = RasterizerState.CullClockwise;
        }
        public void SetUpEffect()
        {
            ref readonly var cam = ref ComponentArray<CameraComponent>.Get(1);
            var x = cam.ScreenRect.Location.X;
            var y = cam.ScreenRect.Location.Y;
            Vector3 cameraUp = Vector3.TransformNormal(new Vector3(0, -1, 0), Matrix.CreateRotationZ(0)) * 10f;
            Matrix World = Matrix.Identity;
            Matrix View = Matrix.CreateLookAt(new Vector3(x, y, -1), new Vector3(x, y, 0), cameraUp);

            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);
        }
        
        public void CreateBuffers()
        {
            var quads = vertexList.ToArray();
            vertexBuffer = new VertexBuffer(Global.Device, typeof(VertexPositionColorTexture), quads.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(quads);
        }
    }
}