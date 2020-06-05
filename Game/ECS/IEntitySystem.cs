using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PixelGlueCore.ECS.Systems
{
    public interface IEntitySystem
    {
        string Name { get; set; }
        public bool IsActive{get;set;}
        public bool IsReady{get;set;}
        public void Initialize()
        {
            IsReady=true;
            IsActive=true;
        }
        public void Update(double deltaTime)
        {

        }
        public void FixedUpdate(double deltaTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}