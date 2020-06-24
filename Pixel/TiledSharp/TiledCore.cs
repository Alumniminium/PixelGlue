// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace TiledSharp
{
    public class TmxDocument
    {
        public string TmxDirectory { get; set; }

        protected XDocument ReadXml(string filepath)
        {
            XDocument xDoc;

            var asm = Assembly.GetEntryAssembly();
            var manifest = new string[0];

            if (asm != null)
                manifest = asm.GetManifestResourceNames();

            var fileResPath = filepath.Replace(Path.DirectorySeparatorChar.ToString(), ".");
            var fileRes = Array.Find(manifest, s => s.EndsWith(fileResPath));

            // If there is a resource in the assembly, load the resource
            // Otherwise, assume filepath is an explicit path
            if (fileRes != null)
            {
                using (Stream xmlStream = asm.GetManifestResourceStream(fileRes))
                using (XmlReader reader = XmlReader.Create(xmlStream))
                    xDoc = XDocument.Load(reader);
                TmxDirectory = string.Empty;
            }
            else
            {
                // TODO: Check for existence of file

                xDoc = XDocument.Load(filepath);
                TmxDirectory = Path.GetDirectoryName(filepath);
            }

            return xDoc;
        }
    }

    public interface ITmxElement
    {
        string Name { get; }
    }

    public class TmxList<T> : KeyedCollection<string, T> where T : ITmxElement
    {
        private readonly Dictionary<string, int> nameCount = new Dictionary<string, int>();

        public new void Add(T t)
        {
            var tName = t.Name;

            // Rename duplicate entries by appending a number
            if (this.Contains(tName))
                nameCount[tName]++;
            else
                nameCount.Add(tName, 0);
            base.Add(t);
        }

        protected override string GetKeyForItem(T item)
        {
            var name = item.Name;
            var count = nameCount[name];

            var dupes = 0;

            // For duplicate keys, append a counter
            // For pathological cases, insert underscores to ensure uniqueness
            while (Contains(name))
            {
                name = name + String.Concat(Enumerable.Repeat("_", dupes))
                            + count.ToString();
                dupes++;
            }

            return name;
        }
    }

    [Serializable]
    public class PropertyDict : Dictionary<string, string>
    {
        public PropertyDict(XContainer xmlProp)
        {
            if (xmlProp == null) return;

            foreach (var p in xmlProp.Elements("property"))
            {
                string pname, pval;

                pname = p.Attribute("name").Value;
                try
                {
                    pval = p.Attribute("value").Value;
                }
                catch (System.NullReferenceException)
                {
                    // Fallback to element value if no "value"
                    pval = p.Value;
                }

                Add(pname, pval);
            }
        }
    }

    public class TmxImage
    {
        public string Source { get;   }
        public string Format { get;   }
        public Stream Data { get;   }
        public TmxColor Trans { get;   }
        public int? Width { get;   }
        public int? Height { get;   }

        public TmxImage(XElement xImage, string tmxDir = "")
        {
            if (xImage == null) return;

            var xSource = xImage.Attribute("source");

            if (xSource != null)
                Source = Path.Combine(tmxDir, (string)xSource);
            else
            {
                Format = (string)xImage.Attribute("format");
                var xData = xImage.Element("data");
                var decodedStream = new TmxBase64Data(xData);
                Data = decodedStream.Data;
            }

            Trans = new TmxColor(xImage.Attribute("trans"));
            Width = (int?)xImage.Attribute("width");
            Height = (int?)xImage.Attribute("height");
        }
    }

    public class TmxColor
    {
        public int R { get;   }
        public int G { get;   }
        public int B { get;   }

        public TmxColor(XAttribute xColor)
        {
            if (xColor == null) return;

            var colorStr = ((string)xColor).TrimStart("#".ToCharArray());

            R = int.Parse(colorStr.Substring(0, 2), NumberStyles.HexNumber);
            G = int.Parse(colorStr.Substring(2, 2), NumberStyles.HexNumber);
            B = int.Parse(colorStr.Substring(4, 2), NumberStyles.HexNumber);
        }
    }

    public class TmxBase64Data
    {
        public Stream Data { get;   }

        public TmxBase64Data(XElement xData)
        {
            if ((string)xData.Attribute("encoding") != "base64")
                throw new Exception("TmxBase64Data: Only Base64-encoded data is supported.");

            var rawData = Convert.FromBase64String((string)xData.Value);
            Data = new MemoryStream(rawData, false);

            var compression = (string)xData.Attribute("compression");
            if (compression == "gzip")
                Data = new GZipStream(Data, CompressionMode.Decompress);
            else if (compression == "zlib")
            {
                // Strip 2-byte header and 4-byte checksum
                // TODO: Validate header here
                var bodyLength = rawData.Length - 6;
                byte[] bodyData = new byte[bodyLength];
                Array.Copy(rawData, 2, bodyData, 0, bodyLength);

                var bodyStream = new MemoryStream(bodyData, false);
                Data = new DeflateStream(bodyStream, CompressionMode.Decompress);

                // TODO: Validate checksum?
            }
            else if (compression != null)
                throw new Exception("TmxBase64Data: Unknown compression.");
        }
    }
}
