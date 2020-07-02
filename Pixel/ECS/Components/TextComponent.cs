namespace Pixel.ECS.Components
{
    public struct TextComponent
    {
        public string Text;
        public string FontName;

        public TextComponent(string text, string font = "profont_12")
        {
            Text = text;
            FontName = font;
        }

    }
}