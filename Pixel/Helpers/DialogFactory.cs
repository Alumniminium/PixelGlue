using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;

namespace Pixel.Helpers
{
    public static class DialogFactory
    {
        public static ref Entity Create(ref Entity owner, string text, string[] options)
        {
            ref var ownerPos = ref owner.Get<PositionComponent>();

            ref var root = ref World.CreateEntity();
            root.Add(new PositionComponent(ownerPos.Value.X -128, ownerPos.Value.Y -128));
            root.Add(new DrawableComponent(Color.IndianRed, new Rectangle((int)ownerPos.Value.X -128, (int)ownerPos.Value.Y -128, 256, 128)));
            ref var foreground = ref World.CreateEntity();
            foreground.Add(new DrawableComponent(Color.Red, new Rectangle((int)ownerPos.Value.X-124, (int)ownerPos.Value.Y-124, 252, 124)));
            root.AddChild(ref foreground);

            ref var textlayer = ref World.CreateEntity();
            textlayer.Add(new PositionComponent(16,8));
            textlayer.Add(new TextComponent(text));
            root.AddChild(ref textlayer);

            for (int i = 0; i < options.Length; i++)
            {
                var option = options[i];
                ref var optionlayer = ref World.CreateEntity();
                optionlayer.Add(new PositionComponent(16, 16 + (i * Global.TileSize + 8)));
                optionlayer.Add(new TextComponent(option));
                root.AddChild(ref optionlayer);
            }

            return ref root;
        }
    }
}