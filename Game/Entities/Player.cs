using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework;
using PixelGlueCore.Networking;
using System;

namespace PixelGlueCore.Entities
{
    public class Player : GameObject
    {
        public Player(int x = 16 * 20, int y = 16 * 20)
        {
            AddComponent(new PositionComponent(x, y, 0));
            AddComponent(new InputComponent());
            AddComponent(new MoveComponent(64, x, y));
            AddComponent(new DrawableComponent("character.png", new Rectangle(0, 2, 16, 16)));
            AddComponent(new CameraFollowTagComponent(1));
            AddComponent(new Networked());
        }
    }
}