using System;

namespace PixelGlueCore.Entities
{
    public class PixelEntity
    {
        public int UniqueId = 0;
        //public PixelEntity() => UniqueId = Helpers.UniqueIdGen.GetNextUID();
        public override string ToString()
        {
            var ret = string.Empty;

            ret += "UID: " + UniqueId;
            ret += Environment.NewLine;
            return ret;
        }
    }
}