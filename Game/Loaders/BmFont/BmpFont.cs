using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using PixelGlueCore.Loaders.BmFont.Models;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PixelGlueCore.Loaders
{
    public class BmpFont
    {
        private readonly Texture2D fontTexture;
        private readonly Dictionary<char, BmpFontChar> _characterMap;

        public BmpFont(string fontName, ContentManager contentManager)
        {
            _characterMap = new Dictionary<char, BmpFontChar>();
            var fontFilePath = Path.Combine(contentManager.RootDirectory, fontName);
            fontTexture = contentManager.Load<Texture2D>(Path.GetFileName(fontName).Replace(".fnt","_0"));
            Load(fontFilePath);
        }
        private void Load(string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            TextReader textReader = new StreamReader(filename);
            FontFile file = (FontFile)deserializer.Deserialize(textReader);
            textReader.Close();
            foreach (var fontCharacter in file.Chars)
            {
                char c = (char)fontCharacter.ID;
                _characterMap.Add(c, new BmpFontChar(fontCharacter));
            }
        }
        public void Draw(string message, Vector2 pos, SpriteBatch _spriteBatch)
        {
            DrawText(_spriteBatch, (int)pos.X, (int)pos.Y, message);
        }
        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text)
        {
            int dx = x;
            int dy = y;
            foreach (char c in text)
            {
                if (_characterMap.TryGetValue(c, out var fc))
                {
                    var position = new Vector2(dx + fc.FontChar.XOffset, dy + fc.FontChar.YOffset);

                    spriteBatch.Draw(fontTexture, position, fc.SrcRect, Color.White);
                    dx += fc.FontChar.XAdvance;
                    PixelGlue.DrawCalls++;
                }
            }
        }
    }
}