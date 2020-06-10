namespace PixelGlueCore.ECS.Components
{
    public struct TextComponent
    {
        public string Text;
        public string FontName;

        public TextComponent(string text,string font="profont")
        {
            Text=text;
            FontName=font;
        }
    }
}