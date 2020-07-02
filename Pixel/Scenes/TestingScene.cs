using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Pixel.ECS;
using Pixel.ECS.Components;
using Pixel.ECS.Systems;
using Pixel.ECS.Systems.UI;
using Pixel.Entities;
using Pixel.Entities.UI;
using Pixel.Helpers;
using Pixel.World;
using Pixel.zero;
using PixelShared;
using PixelShared.Enums;

namespace Pixel.Scenes
{
    public class TestingScene : Scene
    {
        public override void Initialize()
        {
            var redbox = CreateEntity<UIRectangle>();
            redbox.Setup(0,0,Global.ScreenWidth,74,Color.IndianRed);

            Systems.Add(new NetworkSystem());
            Systems.Add(new InputSystem());
            Systems.Add(new MoveSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new MapShaderRenderer());
            //Systems.Add(new ProceduralWorldRenderSystem());
            Systems.Add(new EntityRenderSystem());
            Systems.Add(new NameTagRenderSystem());
            Systems.Add(new DialogSystem());
            Systems.Add(new DbgBoundingBoxRenderSystem());

            Systems.Add(new UIRenderSystem());
            Systems.Add(new SmartFramerate(4));
            Systems.Add(new GCMonitor());
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadTexture("selectionrect4");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont_12.fnt", "profont_12");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt", "profont");
            AssetManager.LoadTexture("tree");
            AssetManager.LoadTexture("dawn");
            AssetManager.LoadTexture("pixel",TextureGen.Pixel(Global.Device,"000000"));           
            base.LoadContent(cm);
        }
    }
}