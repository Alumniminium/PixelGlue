using Microsoft.Xna.Framework.Graphics;
using Pixel.Loaders;
using Shared;
using System.Collections.Generic;

namespace Pixel.Helpers
{
    public static class AssetManager
    {
        public static int TextureCounter,FontCounter;
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<int, string> TextureIds = new Dictionary<int, string>();
        public static Dictionary<string, BmpFont> Fonts = new Dictionary<string, BmpFont>();
        public static Dictionary<int, string> FontIds = new Dictionary<int, string>();
        public static void LoadTexture(string name, Texture2D texture)
        {
            TextureIds.TryAdd(TextureCounter++,name);
            Textures.TryAdd(name, texture);
        }

        public static void LoadTexture(string textureName)
        {
            var texture = Global.ContentManager.Load<Texture2D>(textureName.Split('.')[0].Replace(" ", "_"));
            TextureIds.TryAdd(TextureCounter++,textureName);
            Textures.TryAdd(textureName, texture);
        }

        public static void LoadFont(string path, string name)
        {
            var font = new BmpFont(path, Global.ContentManager);
            Fonts.TryAdd(name, font);
            FontIds.TryAdd(FontCounter++, name);
        }

        public static Texture2D GetTexture(string name)
        {
            if (!Textures.TryGetValue(name, out _))
                LoadTexture(name);
            return Textures[name];
        }
        public static Texture2D GetTexture(int id)
        {
            if (!TextureIds.TryGetValue(id, out var name))
                LoadTexture(name);
            return Textures[name];
        }
    }
}