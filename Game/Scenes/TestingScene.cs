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
using Pixel.Enums;

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
            
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#124E89", NoisePattern.None), "deep_water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#0099DB", NoisePattern.None), "water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#0099DB", NoisePattern.Waves), "water1");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#0099DB", NoisePattern.Waves), "water2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#0099DB", NoisePattern.Waves), "water3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#0099DB", NoisePattern.Waves), "water4");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#0099DB", NoisePattern.Waves), "water5");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#4CB7E5", NoisePattern.None), "shallow_water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#EAD4AA", NoisePattern.Rough), "sand");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#E8B796", NoisePattern.Rough), "sand2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#E4A672", NoisePattern.Rough), "sand3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#C28569", NoisePattern.Rough), "dirt");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#63C74D", NoisePattern.Rough), "plains");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#63C74D", NoisePattern.Flowers), "plains1");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#63C74D", NoisePattern.Flowers), "plains2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#63C74D", NoisePattern.Flowers), "plains3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#63C74D", NoisePattern.Flowers), "plains4");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#63C74D", NoisePattern.Flowers), "plains5");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#3E8948", NoisePattern.None), "grass");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#265c42", NoisePattern.None), "grass2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#193c3e", NoisePattern.None), "grass3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#265C42", NoisePattern.None), "trees");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#B86F50", NoisePattern.Rough), "rock");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#733e39", NoisePattern.Rough), "rock2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#3e2731", NoisePattern.Rough), "rock3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,16,16,"#C0CBDC", NoisePattern.None), "snow");
            base.LoadContent(cm);
        }
    }
}