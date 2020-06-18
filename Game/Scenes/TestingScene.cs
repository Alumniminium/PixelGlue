using PixelGlueCore.ECS;
using PixelGlueCore.Entities;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.ECS.Systems.UI;
using PixelGlueCore.Entities.UI;

namespace PixelGlueCore.Scenes
{
    public class TestingUIScene :UIScene
    {
        public override void Initialize()
        {
            Systems.Add(new UIRenderSystem(this));
            Systems.Add(new SmartFramerate(this,4));
            //var redbox = CreateUIEntity<UIRectangle>();
            //var textblock = CreateUIEntity<Textblock>();
            //var textblock2 = CreateUIEntity<Textblock>();
            //textblock.Setup(10,10,1,1,Color.Transparent);
            //textblock2.Setup(10,40,1,1,Color.Transparent);
            //redbox.Setup(5,200,200,100,Color.Red);
            //textblock.Parent=redbox;
            //textblock2.Parent=redbox;
            //redbox.Children.Add(textblock);
            //redbox.Children.Add(textblock2);
            base.Initialize();
        }
    }
    public class TestingScene : GameScene
    {
        public override void Initialize()
        {
            Camera = new Camera();
            Entities.TryAdd(0, Camera);
            Systems.Add(new MoveSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new EntityRenderSystem(this));
            Systems.Add(new NameTagRenderSystem(this));
            Systems.Add(new DbgBoundingBoxRenderSystem(this));
            
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {
            Map = TmxMapRenderer.Load("../Build/Content/RuntimeContent/island.tmx");
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadTexture("selectionrect3");
            base.LoadContent(cm);
        }
    }
}