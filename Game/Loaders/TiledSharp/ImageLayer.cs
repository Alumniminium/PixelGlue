// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0
using System.Xml.Linq;

namespace TiledSharp
{
    public class TmxImageLayer : ITmxLayer
    {
        public string Name { get;   }

        // TODO: Legacy (Tiled Java) attributes (x, y, width, height)
        public int? Width { get;   }
        public int? Height { get;   }

        public bool Visible { get;   }
        public double Opacity { get;   }
        public double OffsetX { get;   }
        public double OffsetY { get;   }

        public TmxImage Image { get;   }

        public PropertyDict Properties { get;   }

        double? ITmxLayer.OffsetX => OffsetX;
        double? ITmxLayer.OffsetY => OffsetY;

        public TmxImageLayer(XElement xImageLayer, string tmxDir = "")
        {
            Name = (string)xImageLayer.Attribute("name");

            Width = (int?)xImageLayer.Attribute("width");
            Height = (int?)xImageLayer.Attribute("height");
            Visible = (bool?)xImageLayer.Attribute("visible") ?? true;
            Opacity = (double?)xImageLayer.Attribute("opacity") ?? 1.0;
            OffsetX = (double?)xImageLayer.Attribute("offsetx") ?? 0.0;
            OffsetY = (double?)xImageLayer.Attribute("offsety") ?? 0.0;

            Image = new TmxImage(xImageLayer.Element("image"), tmxDir);

            Properties = new PropertyDict(xImageLayer.Element("properties"));
        }
    }
}
