using PixelGlueCore.ECS;
using PixelGlueCore.Entities;
using PixelGlueCore.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PixelGlueCore.Loaders.TiledSharp;
using PixelGlueCore.ECS.Systems;
using PixelGlueCore.ECS.Systems.UI;
using PixelGlueCore.Entities.UI;
using PixelGlueCore.Helpers;

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
            Systems.Add(new ProceduralEntityRenderSystem(this));
            Systems.Add(new NameTagRenderSystem(this));
            Systems.Add(new DbgBoundingBoxRenderSystem(this));
            
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {
            Map = TmxMapRenderer.Load("../Build/Content/RuntimeContent/island.tmx");
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadTexture("selectionrect3");

            AssetManager.LoadTexture(TextureGen.Pixel("#124E89"), "deep_water");
            AssetManager.LoadTexture(TextureGen.Pixel("#0099DB"), "water");
            AssetManager.LoadTexture(TextureGen.Pixel("#4CB7E5"), "shallow_water");
            AssetManager.LoadTexture(TextureGen.Pixel("#EAD4AA"), "sand");
            AssetManager.LoadTexture(TextureGen.Pixel("#E8B796"), "sand2");
            AssetManager.LoadTexture(TextureGen.Pixel("#E4A672"), "sand3");
            AssetManager.LoadTexture(TextureGen.Pixel("#C28569"), "dirt");
            AssetManager.LoadTexture(TextureGen.Pixel("#63C74D"), "plains");
            AssetManager.LoadTexture(TextureGen.Pixel("#3E8948"), "grass");
            AssetManager.LoadTexture(TextureGen.Pixel("#265c42"), "grass2");
            AssetManager.LoadTexture(TextureGen.Pixel("#193c3e"), "grass3");
            AssetManager.LoadTexture(TextureGen.Pixel("#265C42"), "trees");
            AssetManager.LoadTexture(TextureGen.Pixel("#B86F50"), "rock");
            AssetManager.LoadTexture(TextureGen.Pixel("#733e39"), "rock2");
            AssetManager.LoadTexture(TextureGen.Pixel("#3e2731"), "rock3");
            AssetManager.LoadTexture(TextureGen.Pixel("#C0CBDC"), "snow");
            base.LoadContent(cm);
        }
    }
}