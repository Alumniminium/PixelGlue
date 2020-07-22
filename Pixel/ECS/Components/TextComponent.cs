using Shared.ECS;

namespace Pixel.ECS.Components
{
    [Component]
    public struct TextComponent
    {
        public string Value;
        public string FontName;

        public TextComponent(string text, string font = "profont")
        {
            Value = text;
            FontName = font;
        }

    }
}