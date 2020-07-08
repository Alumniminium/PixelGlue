namespace Pixel.ECS.Components
{
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