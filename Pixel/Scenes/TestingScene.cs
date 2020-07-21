using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Pixel.ECS.Components;
using Pixel.ECS.Systems;
using Pixel.Helpers;
using Pixel.zero;
using Shared;
using Shared.ECS;

namespace Pixel.Scenes
{
    public class TestingScene : Scene
    {
        public override void Initialize()
        {
            World.Systems.Add(new GCMonitor(true, false));
            World.Systems.Add(new NetworkSystem(true, false));
            World.Systems.Add(new MouseInputSystem(true, false));
            World.Systems.Add(new PlayerInputSystem(true, false));
            World.Systems.Add(new CursorMoveSystem(true, false));
            World.Systems.Add(new MoveSystem(true, false));
            World.Systems.Add(new CameraSystem(true, false));
            //World.Systems.Add(new MapShaderRenderer(true,true));
            World.Systems.Add(new WorldRenderSystem(false, true));
            World.Systems.Add(new DbgEntitySpawnSystem(true, false));
            World.Systems.Add(new EntityRenderSystem(true, true));
            World.Systems.Add(new NameTagRenderSystem(false, true));
            //Systems.Add(new DialogSystem());
            World.Systems.Add(new DbgBoundingBoxRenderSystem(false, true));

            World.Systems.Add(new SmartFramerate(true, false));
            base.Initialize();

            var cursor = World.CreateEntity();
            cursor.Add<MouseComponent>();
            cursor.Add<PositionComponent>();
            var cursorDrw = new DrawableComponent(Color.IndianRed, new Rectangle(0, 0, 64, 64));
            cursor.Add(cursorDrw);
            World.Register(ref cursor);
        }
        public override void LoadContent(ContentManager cm)
        {
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont_12.fnt", "profont_12");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            AssetManager.LoadTexture("selectionrect4");
            AssetManager.LoadTexture("tree");
            AssetManager.LoadTexture("dawn");
            AssetManager.LoadTexture("pixel", TextureGen.Pixel(Global.Device, "ffffff"));
            base.LoadContent(cm);
        }
    }
}