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
        void Update(float deltaTime);
        void FixedUpdate(float deltaTime);

        public void Draw(Scene scene,SpriteBatch spriteBatch)
        {
        }
    }
}