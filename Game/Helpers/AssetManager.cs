using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelGlueCore.Loaders;
using System;
using System.Collections.Generic;

namespace PixelGlueCore.ECS
{
    public static class AssetManager
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, BmpFont> Fonts = new Dictionary<string, BmpFont>();
        public static void LoadTexture(Texture2D texture,string name) => Textures.TryAdd(name,texture);
        public static void LoadTexture(string textureName) => Textures.TryAdd(textureName, PixelGlue.ContentManager.Load<Texture2D>(textureName.Split('.')[0].Replace(" ", "_")));
        public static void LoadFont(string path, string name) => Fonts.TryAdd(name, new BmpFont(path, PixelGlue.ContentManager));
        public static Texture2D GetTexture(string name)
        {
            if (!Textures.TryGetValue(name, out _))
                LoadTexture(name);
            return Textures[name];
        }
    }
}