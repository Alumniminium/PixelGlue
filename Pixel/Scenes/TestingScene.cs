using Microsoft.Xna.Framework.Content;
using Pixel.ECS;
using Pixel.ECS.Systems;
using Pixel.Helpers;
using Pixel.World;
using Shared;

namespace Pixel.Scenes
{
    public class TestingScene : Scene
    {
        public override void Initialize()
        {
            Systems.Add(new NetworkSystem());
            Systems.Add(new PlayerInputSystem());
            Systems.Add(new MoveSystem());
            Systems.Add(new CameraSystem());
            //Systems.Add(new MapShaderRenderer());
            Systems.Add(new WorldRenderSystem());
            Systems.Add(new EntityRenderSystem());
            Systems.Add(new NameTagRenderSystem());
            //Systems.Add(new DialogSystem());
            Systems.Add(new DbgBoundingBoxRenderSystem());

            //Systems.Add(new UIRenderSystem());
            Systems.Add(new SmartFramerate());
            Systems.Add(new GCMonitor());
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