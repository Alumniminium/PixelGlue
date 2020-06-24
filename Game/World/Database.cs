using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Pixel.IO;
using PixelGlueCore;
using PixelGlueCore.ECS;
using PixelGlueCore.Helpers;
using System;
using System.Collections.Generic;

namespace PixelGlueCore.World
{
    public static class Database
    {
        public static Dictionary<int, BaseEntity> Entities = new Dictionary<int, BaseEntity>();
        public static void Load(string path)
        {
            FConsole.WriteLine("Loading " + Environment.CurrentDirectory + path);
            var iniFile = new IniFile(path);
            iniFile.Load();
            var contents = iniFile.GetDictionary();
            var textureName = "Name= not set in [Atlas]";
            var tileSize = 0;
            foreach (var header in contents)
            {
                var name = header.Key[1..^1];
                if (name == "Atlas")
                {
                    textureName = header.Value["Name"];
                    AssetManager.LoadTexture(textureName);
                    tileSize = int.Parse(header.Value["TileSize"]);
                }
                else
                {
                    if (header.Value["Type"] == "Human")
                    {
                        var position = header.Value["Position"];
                        var xy = position.Split(',');
                        int x = int.Parse(xy[0]);
                        int y = int.Parse(xy[1]);

                        var id = int.Parse(header.Value["Id"]);
                        var entity = new BaseEntity(id, name, x, y, tileSize, tileSize, textureName);
                        Entities.TryAdd(entity.Id, entity);
                    }
                }
            }
        }
    }
}