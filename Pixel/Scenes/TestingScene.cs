using Microsoft.Xna.Framework.Content;
using Pixel.ECS.Systems;
using Pixel.Helpers;
using Shared;
using Shared.ECS;
using Shared.ECS.Systems;

namespace Pixel.Scenes
{
    public class TestingScene : Scene
    {
        public BloodSpawner Spawner;
        public static Player Player;
        public override void Initialize()
        {
            World.Systems.Add(new GCMonitor(true, false));
            World.Systems.Add(new NetworkSystem(true, false));
            World.Systems.Add(new MouseInputSystem(true, false));
            World.Systems.Add(new PlayerInputSystem(true, false));
            //World.Systems.Add(new CursorMoveSystem(true, false));
            World.Systems.Add(new MoveSystem(true, false));
            World.Systems.Add(new CameraSystem(true, false));
            World.Systems.Add(new ParticleEmitterSystem(true, false));
            World.Systems.Add(new ParticleSystem(true, false));
            //World.Systems.Add(new MapShaderRenderer(true,true));
            //World.Systems.Add(new WorldRenderSystem(false, true));
            World.Systems.Add(new BackgroundRenderSystem(false, true));
            //World.Systems.Add(new DbgEntitySpawnSystem(true, false));
            World.Systems.Add(new EntityRenderSystem(false, true));
            //World.Systems.Add(new DialogSystem(true,true));

            World.Systems.Add(new TextRenderSystem(false, true));
            World.Systems.Add(new DbgBoundingBoxRenderSystem(false, true));
            World.Systems.Add(new SmartFramerate(true, true));
            base.Initialize();

            //Player = World.CreateEntity();
            Player= new Player(1024,1008);
            Spawner = new BloodSpawner(1024,1024);

            
            IsReady=true;
            //var cursor = World.CreateEntity();
            //cursor.Add<MouseComponent>();
            //cursor.Add<PositionComponent>();
            //var cursorDrw = new DrawableComponent(Color.IndianRed, 64);
            //cursor.Add(cursorDrw);           
            //World.Register(ref cursor);
        }
        public override void LoadContent(ContentManager cm)
        {
            //var worldTexture = Texture2D.FromFile(Global.Device,"../Build/Content/RuntimeContent/Biomes.bmp");
            //AssetManager.LoadTexture("world",worldTexture);

            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont_12.fnt", "profont_12");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            AssetManager.LoadTexture("selectionrect4");
            //AssetManager.LoadTexture("tree");
            //AssetManager.LoadTexture("dawn");
            AssetManager.LoadTexture("pixel", TextureGen.Pixel(Global.Device, "ffffff"));
            base.LoadContent(cm);
        }
    }
}