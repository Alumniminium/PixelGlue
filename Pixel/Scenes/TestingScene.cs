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
using PixelShared;
using PixelShared.Enums;

namespace Pixel.Scenes
{
    public class TestingScene : Scene
    {
        public override void Initialize()
        {
            var player = CreateEntity<Player>();
            player.Add(new DrawableComponent(player.EntityId, "character.png", new Rectangle(0, 2, 16, 16)));
            player.Add(new VelocityComponent(player.EntityId, 64));
            player.Add(new PositionComponent(player.EntityId, 0, 0, 0));
            player.Add(new InputComponent(player.EntityId));
            player.Add(new CameraFollowTagComponent(player.EntityId, 1));

            var redbox = CreateEntity<UIRectangle>();
            redbox.Setup(0,0,Global.ScreenWidth,74,Color.IndianRed);

            Systems.Add(new NetworkSystem());
            Systems.Add(new InputSystem());
            Systems.Add(new MoveSystem());
            Systems.Add(new CameraSystem());
            Systems.Add(new ProceduralEntityRenderSystem());
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
            AssetManager.LoadTexture("deep_water", TextureGen.Noise("#124E89", NoisePattern.None));
            AssetManager.LoadTexture("water", TextureGen.Noise("#0099DB", NoisePattern.None));
            AssetManager.LoadTexture("water1", TextureGen.Noise("#0099DB", NoisePattern.Waves));
            AssetManager.LoadTexture("water2", TextureGen.Noise("#0099DB", NoisePattern.Waves));
            AssetManager.LoadTexture("water3", TextureGen.Noise("#0099DB", NoisePattern.Waves));
            AssetManager.LoadTexture("water4", TextureGen.Noise("#0099DB", NoisePattern.Waves));
            AssetManager.LoadTexture("water5", TextureGen.Noise("#0099DB", NoisePattern.Waves));
            AssetManager.LoadTexture("shallow_water", TextureGen.Noise("#4CB7E5", NoisePattern.None));
            AssetManager.LoadTexture("sand", TextureGen.Noise("#EAD4AA", NoisePattern.Rough));
            AssetManager.LoadTexture("sand2", TextureGen.Noise("#E8B796", NoisePattern.Rough));
            AssetManager.LoadTexture("sand3", TextureGen.Noise("#E4A672", NoisePattern.Rough));
            AssetManager.LoadTexture("dirt", TextureGen.Noise("#C28569", NoisePattern.Rough));
            AssetManager.LoadTexture("plains", TextureGen.Noise("#63C74D", NoisePattern.Rough));
            AssetManager.LoadTexture("plains1", TextureGen.Noise("#63C74D", NoisePattern.Flowers));
            AssetManager.LoadTexture("plains2", TextureGen.Noise("#63C74D", NoisePattern.Flowers));
            AssetManager.LoadTexture("plains3", TextureGen.Noise("#63C74D", NoisePattern.Flowers));
            AssetManager.LoadTexture("plains4", TextureGen.Noise("#63C74D", NoisePattern.Flowers));
            AssetManager.LoadTexture("plains5", TextureGen.Noise("#63C74D", NoisePattern.Flowers));
            AssetManager.LoadTexture("grass", TextureGen.Noise("#3E8948", NoisePattern.None));
            AssetManager.LoadTexture("grass2", TextureGen.Noise("#265c42", NoisePattern.None));
            AssetManager.LoadTexture("grass3", TextureGen.Noise("#193c3e", NoisePattern.None));
            AssetManager.LoadTexture("trees", TextureGen.Noise("#265C42", NoisePattern.None));
            AssetManager.LoadTexture("rock", TextureGen.Noise("#B86F50", NoisePattern.Rough));
            AssetManager.LoadTexture("rock2", TextureGen.Noise("#733e39", NoisePattern.Rough));
            AssetManager.LoadTexture("rock3", TextureGen.Noise("#3e2731", NoisePattern.Rough));
            AssetManager.LoadTexture("snow", TextureGen.Noise("#C0CBDC", NoisePattern.None));
            base.LoadContent(cm);
        }
    }
}