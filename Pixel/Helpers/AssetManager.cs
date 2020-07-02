using Microsoft.Xna.Framework.Graphics;
using Pixel.Loaders;
using PixelShared;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pixel.ECS
{
    public static class AssetManager
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, BmpFont> Fonts = new Dictionary<string, BmpFont>();
        public static void LoadTexture(string name,Texture2D texture) => Textures.TryAdd(name, texture);
        public static void LoadTexture(string textureName) => Textures.TryAdd(textureName, Global.ContentManager.Load<Texture2D>(textureName.Split('.')[0].Replace(" ", "_")));
        public static void LoadFont(string path, string name) => Fonts.TryAdd(name, new BmpFont(path, Global.ContentManager));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Texture2D GetTexture(string name)
        {
            if (!Textures.TryGetValue(name, out _))
                LoadTexture(name);
            return Textures[name];
        }
    }
}