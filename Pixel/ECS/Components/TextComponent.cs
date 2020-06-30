using Pixel.Enums;

namespace Pixel.ECS.Components
{
    public struct TextComponent : IEntityComponent
    {
        public int EntityId { get; set; }
        public string Text;
        public string FontName;

        public TextComponent(int ownerId, string text, string font = "profont")
        {
            EntityId = ownerId;
            Text = text;
            FontName = font;
        }

    }
}