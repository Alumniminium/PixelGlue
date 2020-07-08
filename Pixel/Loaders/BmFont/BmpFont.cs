using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pixel.Loaders.BmFont.Models;
using Shared;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Pixel.Loaders
{
    public class BmpFont
    {
        private readonly Texture2D fontTexture;
        private readonly Dictionary<char, BmpFontChar> _characterMap;

        public BmpFont(string fontName, ContentManager contentManager)
        {
            _characterMap = new Dictionary<char, BmpFontChar>();
            var fontFilePath = Path.Combine(contentManager.RootDirectory, fontName);
            fontTexture = contentManager.Load<Texture2D>(Path.GetFileName(fontName).Replace(".fnt", "_0"));
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
        public void DrawText(SpriteBatch spriteBatch, float x, float y, string text, float scale = 0.5f)
        {
            int dx = (int)x;
            int dy = (int)y;
            foreach (char c in text)
            {
                if (_characterMap.TryGetValue(c, out var fc))
                {
                    var dst = new Rectangle(dx + (int)(fc.FontChar.XOffset*scale),dy +(int)(fc.FontChar.YOffset*scale),(int)(fc.SrcRect.Width * scale), (int)(fc.SrcRect.Height * scale));
                    spriteBatch.Draw(fontTexture, dst, fc.SrcRect, Color.White);
                    dx += (int)(fc.FontChar.XAdvance*scale);
                    Global.DrawCalls++;
                }
            }
        }
    }
}