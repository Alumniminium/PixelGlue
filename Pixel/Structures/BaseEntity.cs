using Microsoft.Xna.Framework;

namespace Pixel
{
    public class BaseEntity
    {
        public int Id;
        public string Name;
        public Rectangle SrcRect;
        public string TextureName;
        public BaseEntity(int id, string name, int x, int y, int width, int height, string textureName)
        {
            Id = id;
            Name = name;
            SrcRect = new Rectangle(x, y, width, height);
            TextureName = textureName;
        }
    }
}