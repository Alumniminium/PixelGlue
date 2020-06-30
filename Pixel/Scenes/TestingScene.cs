using Pixel.ECS;
using Pixel.Entities;
using Pixel.World;
using Microsoft.Xna.Framework.Content;
using Pixel.ECS.Systems;
using Pixel.ECS.Systems.UI;
using Pixel.Helpers;
using PixelShared.Enums;
using Pixel.ECS.Components;
using Microsoft.Xna.Framework;

namespace Pixel.Scenes
{
    public class TestingUIScene : Scene
    {
        public override void Initialize()
        {
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
    public class TestingScene : Scene
    {
        public override void Initialize()
        {
            Camera = CreateEntity<Camera>(-1);
            var player = CreateEntity<Player>(-1);
            player.Add(new DrawableComponent(player.EntityId,"character.png", new Rectangle(0, 2, 16, 16)));
            player.Add(new VelocityComponent(player.EntityId,64));
            player.Add(new PositionComponent(player.EntityId,0,0,0));
            player.Add(new InputComponent(player.EntityId));
            player.Add(new CameraFollowTagComponent(player.EntityId,1));

            //Systems.Add(new NetworkSystem());
            //Systems.Add(new InputSystem());
            Systems.Add(new MoveSystem());
            //Systems.Add(new CameraSystem());
            //Systems.Add(new ProceduralEntityRenderSystem(this));
            Systems.Add(new NameTagRenderSystem(this));
            //Systems.Add(new DialogSystem());
            //Systems.Add(new DbgBoundingBoxRenderSystem(this));

            //Systems.Add(new UIRenderSystem(this));
            //Systems.Add(new SmartFramerate(this,4));
            Systems.Add(new GCMonitor());
            base.Initialize();
        }
        public override void LoadContent(ContentManager cm)
        {
            Database.Load("../Build/Content/RuntimeContent/Equipment.txt");
            AssetManager.LoadTexture("selectionrect4");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont_12.fnt","profont_12");
            AssetManager.LoadFont("../Build/Content/RuntimeContent/profont.fnt","profont");
            AssetManager.LoadTexture("tree");
            AssetManager.LoadTexture("dawn");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#124E89", NoisePattern.None), "deep_water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#0099DB", NoisePattern.None), "water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water1");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water4");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#0099DB", NoisePattern.Waves), "water5");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#4CB7E5", NoisePattern.None), "shallow_water");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#EAD4AA", NoisePattern.Rough), "sand");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#E8B796", NoisePattern.Rough), "sand2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#E4A672", NoisePattern.Rough), "sand3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#C28569", NoisePattern.Rough), "dirt");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#63C74D", NoisePattern.Rough), "plains");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains1");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains4");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#63C74D", NoisePattern.Flowers), "plains5");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#3E8948", NoisePattern.None), "grass");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#265c42", NoisePattern.None), "grass2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#193c3e", NoisePattern.None), "grass3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#265C42", NoisePattern.None), "trees");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#B86F50", NoisePattern.Rough), "rock");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#733e39", NoisePattern.Rough), "rock2");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#3e2731", NoisePattern.Rough), "rock3");
            AssetManager.LoadTexture(TextureGen.Noise(PixelShared.Pixel.TileSize,PixelShared.Pixel.TileSize,"#C0CBDC", NoisePattern.None), "snow");
            base.LoadContent(cm);
        }
    }
}