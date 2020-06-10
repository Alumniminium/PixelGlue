using PixelGlueCore.ECS;
using PixelGlueCore.ECS.Components;
using Microsoft.Xna.Framework;
using PixelGlueCore.Networking;
using System;

namespace PixelGlueCore.Entities
{
    public class Player : PixelEntity
    {
    }    
    public class NameTag : PixelEntity
    {
        public NameTag(string name)
        {
            AddNameTag(new TextComponent(name));    
        }
    }
}