using PixelGlueCore.ECS;
using PixelGlueCore.Entities;
using PixelGlueCore.World;
using Microsoft.Xna.Framework.Content;
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
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadTexture("selectionrect3");
            AssetManager.LoadTexture("tree");
            AssetManager.LoadTexture("dawn");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#124E89", NoisePattern.None), "deep_water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#0099DB", NoisePattern.None), "water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water1");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water4");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water5");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#4CB7E5", NoisePattern.None), "shallow_water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#EAD4AA", NoisePattern.Rough), "sand");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#E8B796", NoisePattern.Rough), "sand2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#E4A672", NoisePattern.Rough), "sand3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#C28569", NoisePattern.Rough), "dirt");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#63C74D", NoisePattern.Rough), "plains");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains1");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains4");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains5");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#3E8948", NoisePattern.None), "grass");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#265c42", NoisePattern.None), "grass2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#193c3e", NoisePattern.None), "grass3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#265C42", NoisePattern.None), "trees");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#B86F50", NoisePattern.Rough), "rock");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#733e39", NoisePattern.Rough), "rock2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#3e2731", NoisePattern.Rough), "rock3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelGlue.Device,Pixel.Pixel.TileSize,Pixel.Pixel.TileSize,"#C0CBDC", NoisePattern.None), "snow");
            base.LoadContent(cm);
        }
    }
}