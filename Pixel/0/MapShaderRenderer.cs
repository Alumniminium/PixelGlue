using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pixel.ECS;
using Pixel.Enums;
using Pixel.Scenes;
using PixelShared;

namespace Pixel.zero
{
    public class MapShaderRenderer : IEntitySystem
    {
        public string Name => "Map Shader Renderer";
        public bool IsActive { get; set; }
        public bool IsReady { get; set; }

        public const int VERTICES_PER_QUAD = 6;
        public const int TRIANGLES_PER_QUAD = 2;
        public static Effect effect;
        public int mapWidth = 1000;
        public int mapHeight = 1000;
        public List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();       
        public List<Texture2D> layerTextures = new List<Texture2D>();
        VertexBuffer vertexBuffer;

        public void Initialize()
        {
            AssetManager.LoadTexture("rivers");
            AssetManager.LoadTexture("height_mountain_mono");
            var mountain = AssetManager.GetTexture("height_mountain_mono");
            var rivers = AssetManager.GetTexture("rivers");
            // layer 0 tiles
            var mountainWidth = mountain.Width / 8;
            var mountainHeight = mountain.Height / 8;

            // layer 1 tiles 
            var riverSourceTileWidth = rivers.Width / 8;
            var riverSourceTileHeight = rivers.Height / 8;

            CreateTileMapFromSheet(0, mountain, mapWidth, mapHeight, mountainWidth, mountainHeight);
            CreateTileMapFromSheet(1, rivers, mapWidth, mapHeight, riverSourceTileWidth, riverSourceTileHeight);
            CreateTileMapFromSheet(2, rivers, mapWidth, mapHeight, riverSourceTileWidth, riverSourceTileHeight);
            
            var quads = vertexList.ToArray();
            vertexBuffer = new VertexBuffer(Global.Device, typeof(VertexPositionColorTexture), quads.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(quads);
            Global.Device.SetVertexBuffer(vertexBuffer);
            effect = Global.ContentManager.Load<Effect>("MapRenderer");
            effect.Parameters["TextureA"].SetValue(mountain);
            effect.CurrentTechnique = effect.Techniques["QuadDraw"];
            IsActive=true;
            IsReady=true;
        }

        public void FixedUpdate(float deltaTime)
        {
            
        }

        public void Update(float deltaTime)
        {
            var camera = SceneManager.ActiveScene.Camera;   
            effect.Parameters["Projection"].SetValue(camera.Transform.ViewMatrix);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Global.Device.BlendState = BlendState.AlphaBlend; // AlphaBlend Opaque NonPremultiplied
            Global.Device.DepthStencilState = DepthStencilState.Default;
            Global.Device.SamplerStates[0] = SamplerState.PointClamp;
            Global.Device.RasterizerState = RasterizerState.CullClockwise;

            var player = SceneManager.ActiveScene.Player;
            var camera = SceneManager.ActiveScene.Camera;
            
            Vector3 cameraUp = Vector3.TransformNormal(new Vector3(0, -1, 0), Matrix.CreateRotationZ(0)) * 10f;
            Matrix World = Matrix.Identity;
            Matrix View = Matrix.CreateLookAt(new Vector3(player.PositionComponent.Position,0), Vector3.Zero, cameraUp);

            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);

            // Draw all tiles that are valid from start to end layers.
            DrawTileMap(camera.ServerScreenRect, 0, 1);

        }
        public void DrawTileMap(Rectangle TileRegionToDraw, int StartingLayer, int LayerLength)
        {
            int mapTilesLength = mapWidth * mapHeight;
            Global.Device.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                for (int layer = StartingLayer; layer < (StartingLayer + LayerLength); layer++)
                {
                    // Set the texture that this layer uses on the card.
                    effect.Parameters["TextureA"].SetValue(layerTextures[layer]);

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
        public void CreateTileMapFromSheet(int seed, Texture2D texture, int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            // put the textures into a layer list to keep things orderly later in the draw loop.
            layerTextures.Add(texture);

            // we pass in a seed to randomize the map im not going to design a actual map for the example.
            Random rnd = new Random(seed);
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    var rx = rnd.Next(0, 8);
                    var ry = rnd.Next(0, 8);
                    var source = new Rectangle(rx * tileWidth, ry * tileHeight, tileWidth, tileHeight);
                    var destination = new Rectangle(x * Global.TileSize, y * Global.TileSize, Global.TileSize, Global.TileSize);
                    Vector2 tl = source.Location.ToVector2() / texture.Bounds.Size.ToVector2();
                    Vector2 tr = new Vector2(source.Right, source.Top) / texture.Bounds.Size.ToVector2();
                    Vector2 br = new Vector2(source.Right, source.Bottom) / texture.Bounds.Size.ToVector2();
                    Vector2 bl = new Vector2(source.Left, source.Bottom) / texture.Bounds.Size.ToVector2();
                    // t1
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), Color.White, tl));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Bottom, 0f), Color.White, bl));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), Color.White, br));
                    // t2
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), Color.White, br));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Top, 0f), Color.White, tr));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), Color.White, tl));
                }
            }
        }
    }
    public class Example05TileLayeredCustomBufferedDraw : Game
    {
        public const int VERTICES_PER_QUAD = 6;
        public const int TRIANGLES_PER_QUAD = 2;
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static Effect effect;
        Matrix Projection;

        public List<Texture2D> layerTextures = new List<Texture2D>();

        public int mountainWidth;
        public int mountainHeight;

        public int riverSourceTileWidth;
        public int riverSourceTileHeight;

        Vector2 mapViewedScrollPos = new Vector2();
        private Vector3 camera2DScrollPosition = new Vector3(0, 0, -1);
        private Vector3 camera2DScrollLookAt = new Vector3(0, 0, 0);
        public float camera2dRotationZ = 0f;

        public int mapWidth = 1000;
        public int mapHeight = 1000;

        // for our buffer
        public List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
        VertexBuffer vertexBuffer;

        public Example05TileLayeredCustomBufferedDraw()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = Global.ScreenWidth;
            graphics.PreferredBackBufferHeight = Global.ScreenHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Global.ContentManager = Content;
            Global.Device=GraphicsDevice;
            Projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateOrthographicOffCenter(0, Global.Device.Viewport.Width, Global.Device.Viewport.Height, 0, 0, 1) ;//* Matrix.CreateScale(Global.ScreenWidth / Global.VirtualScreenWidth, Global.ScreenHeight / Global.VirtualScreenHeight, 1f);
            
            var mountain = AssetManager.GetTexture("height_mountain_mono");
            var rivers = AssetManager.GetTexture("rivers");
            // layer 0 tiles
            mountainWidth = mountain.Width / 8;
            mountainHeight = mountain.Height / 8;

            // layer 1 tiles 
            riverSourceTileWidth = rivers.Width / 8;
            riverSourceTileHeight = rivers.Height / 8;

            CreateTileMapFromSheet(0, mountain, mapWidth, mapHeight, mountainWidth, mountainHeight);
            CreateTileMapFromSheet(1, rivers, mapWidth, mapHeight, riverSourceTileWidth, riverSourceTileHeight);
            CreateTileMapFromSheet(2, rivers, mapWidth, mapHeight, riverSourceTileWidth, riverSourceTileHeight);
            
            var quads = vertexList.ToArray();
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorTexture), quads.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(quads);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            effect = Content.Load<Effect>("MapRenderer");
            effect.CurrentTechnique = effect.Techniques["QuadDraw"];
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["TextureA"].SetValue(mountain);
        }
        public void SetTile(Texture2D texture, Rectangle destination, Rectangle source, int mapTilePosX, int mapTilePosY)
        {
            int layer = -1;
            for (int i = 0; i < layerTextures.Count; i++)
                if (layerTextures[i] == texture)
                    layer = i;

            int mapTilesLength = mapWidth * mapHeight;
            var layerVerticeOffset = mapTilesLength * layer * VERTICES_PER_QUAD;
            int startingVerticeIndex = ((mapTilePosY * mapWidth + mapTilePosX) * VERTICES_PER_QUAD) + layerVerticeOffset;
            
            Vector2 tl = source.Location.ToVector2() / texture.Bounds.Size.ToVector2();
            Vector2 tr = new Vector2(source.Right, source.Top) / texture.Bounds.Size.ToVector2();
            Vector2 br = new Vector2(source.Right, source.Bottom) / texture.Bounds.Size.ToVector2();
            Vector2 bl = new Vector2(source.Left, source.Bottom) / texture.Bounds.Size.ToVector2();

            vertexList[startingVerticeIndex + 0] = new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), vertexList[startingVerticeIndex + 0].Color, tl);
            vertexList[startingVerticeIndex + 1] = new VertexPositionColorTexture(new Vector3(destination.Left, destination.Bottom, 0f), vertexList[startingVerticeIndex + 1].Color, bl);
            vertexList[startingVerticeIndex + 2] = new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), vertexList[startingVerticeIndex + 2].Color, br);
        
            vertexList[startingVerticeIndex + 3] = new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), vertexList[startingVerticeIndex + 3].Color, br);
            vertexList[startingVerticeIndex + 4] = new VertexPositionColorTexture(new Vector3(destination.Right, destination.Top, 0f), vertexList[startingVerticeIndex + 4].Color, tr);
            vertexList[startingVerticeIndex + 5] = new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), vertexList[startingVerticeIndex + 5].Color, tl);
        }


        public void SetTile(Texture2D texture, Rectangle source, int mapTilePosX, int mapTilePosY)
        {
            int layer = -1;
            for (int i = 0; i < layerTextures.Count; i++)
                if (layerTextures[i] == texture)
                {
                    layer = i;
                }
            if (layer < 0)
            {
                throw new Exception("Error This function doesn't explicitly handle adding a entirely new layer eg a new texture.");
            }
            int mapTilesLength = mapWidth * mapHeight;
            var layerVerticeOffset = mapTilesLength * layer * VERTICES_PER_QUAD;
            int startingVerticeIndex = ((mapTilePosY * mapWidth + mapTilePosX) * VERTICES_PER_QUAD) + layerVerticeOffset;
            //
            Vector2 tl = source.Location.ToVector2() / texture.Bounds.Size.ToVector2();
            Vector2 tr = new Vector2(source.Right, source.Top) / texture.Bounds.Size.ToVector2();
            Vector2 br = new Vector2(source.Right, source.Bottom) / texture.Bounds.Size.ToVector2();
            Vector2 bl = new Vector2(source.Left, source.Bottom) / texture.Bounds.Size.ToVector2();

            // t1
            vertexList[startingVerticeIndex + 0] = new VertexPositionColorTexture(vertexList[startingVerticeIndex + 0].Position, vertexList[startingVerticeIndex + 0].Color, tl);
            vertexList[startingVerticeIndex + 1] = new VertexPositionColorTexture(vertexList[startingVerticeIndex + 1].Position, vertexList[startingVerticeIndex + 1].Color, bl);
            vertexList[startingVerticeIndex + 2] = new VertexPositionColorTexture(vertexList[startingVerticeIndex + 2].Position, vertexList[startingVerticeIndex + 2].Color, br);
            // t2
            vertexList[startingVerticeIndex + 3] = new VertexPositionColorTexture(vertexList[startingVerticeIndex + 3].Position, vertexList[startingVerticeIndex + 3].Color, br);
            vertexList[startingVerticeIndex + 4] = new VertexPositionColorTexture(vertexList[startingVerticeIndex + 4].Position, vertexList[startingVerticeIndex + 4].Color, tr);
            vertexList[startingVerticeIndex + 5] = new VertexPositionColorTexture(vertexList[startingVerticeIndex + 5].Position, vertexList[startingVerticeIndex + 5].Color, tl);
        }
        public void CreateTileMapFromSheet(int seed, Texture2D texture, int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            // put the textures into a layer list to keep things orderly later in the draw loop.
            layerTextures.Add(texture);

            // we pass in a seed to randomize the map im not going to design a actual map for the example.
            Random rnd = new Random(seed);
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    var rx = rnd.Next(0, 8);
                    var ry = rnd.Next(0, 8);
                    var source = new Rectangle(rx * tileWidth, ry * tileHeight, tileWidth, tileHeight);
                    var destination = new Rectangle(x * Global.TileSize, y * Global.TileSize, Global.TileSize, Global.TileSize);
                    Vector2 tl = source.Location.ToVector2() / texture.Bounds.Size.ToVector2();
                    Vector2 tr = new Vector2(source.Right, source.Top) / texture.Bounds.Size.ToVector2();
                    Vector2 br = new Vector2(source.Right, source.Bottom) / texture.Bounds.Size.ToVector2();
                    Vector2 bl = new Vector2(source.Left, source.Bottom) / texture.Bounds.Size.ToVector2();
                    // t1
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), Color.White, tl));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Bottom, 0f), Color.White, bl));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), Color.White, br));
                    // t2
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Bottom, 0f), Color.White, br));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Right, destination.Top, 0f), Color.White, tr));
                    vertexList.Add(new VertexPositionColorTexture(new Vector3(destination.Left, destination.Top, 0f), Color.White, tl));
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.AlphaBlend; // AlphaBlend Opaque NonPremultiplied
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            Vector3 cameraUp = Vector3.TransformNormal(new Vector3(0, -1, 0), Matrix.CreateRotationZ(camera2dRotationZ)) * 10f;
            Matrix World = Matrix.Identity;
            Matrix View = Matrix.CreateLookAt(camera2DScrollPosition, camera2DScrollLookAt, cameraUp);

            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);

            // Draw all tiles that are valid from start to end layers.
            DrawTileMap(new Rectangle(0,0,Global.VirtualScreenWidth/Global.TileSize,Global.VirtualScreenHeight/Global.TileSize), 0, layerTextures.Count);

            base.Draw(gameTime);
        }

        public void DrawTileMap(Rectangle TileRegionToDraw, int StartingLayer, int LayerLength)
        {
            int mapTilesLength = mapWidth * mapHeight;
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                for (int layer = StartingLayer; layer < (StartingLayer + LayerLength); layer++)
                {
                    // Set the texture that this layer uses on the card.
                    effect.Parameters["TextureA"].SetValue(layerTextures[layer]);

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
                        GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, startingVerticeIndex, primitiveSpanLength);
                    }
                }
            }
        }
    }
}