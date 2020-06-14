using PixelGlueCore.ECS.Systems;

namespace PixelGlueCore.ECS.Components
{
    public struct TextComponent : IEntityComponent
    {
        public int UniqueId {get;set;}
        public string Text;
        public string FontName;

        public TextComponent(int ownerId, string text,string font="profont")
        {
            UniqueId=ownerId;
            Text=text;
            FontName=font;
        }

    }
}