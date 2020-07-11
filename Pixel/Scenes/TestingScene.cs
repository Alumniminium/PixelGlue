using Microsoft.Xna.Framework.Content;
using Pixel.ECS;
using Pixel.ECS.Systems;
using Pixel.Helpers;
using Shared;
using Shared.ECS;

namespace Pixel.Scenes
{
    public class TestingScene : Scene
    {
        public override void Initialize()
        {
            World.Systems.Add(new NetworkSystem());
            World.Systems.Add(new PlayerInputSystem());
            World.Systems.Add(new MoveSystem());
            World.Systems.Add(new CameraSystem());
            //Systems.Add(new MapShaderRenderer());
            World.Systems.Add(new WorldRenderSystem());
            //Systems.Add(new PretendSystem());
            World.Systems.Add(new EntityRenderSystem());
            World.Systems.Add(new NameTagRenderSystem());
            //Systems.Add(new DialogSystem());
            World.Systems.Add(new DbgBoundingBoxRenderSystem());

            //Systems.Add(new UIRenderSystem());
            World.Systems.Add(new SmartFramerate());
            World.Systems.Add(new GCMonitor());
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont_12.fnt", "profont_12");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            AssetManager.LoadTexture("selectionrect4");
            AssetManager.LoadTexture("tree");
            AssetManager.LoadTexture("dawn");
            AssetManager.LoadTexture("pixel", TextureGen.Pixel(Global.Device, "000000"));
            base.LoadContent(cm);
        }
    }
}