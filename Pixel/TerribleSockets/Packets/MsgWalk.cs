using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Pixel.TerribleSockets.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgWalk
    {
        public int Length;
        public ushort Id;
        public int TickCount;
        public int UniqueId;
        public int X, Y;

        public static MsgWalk Create(int uniqueId, int x, int y)
        {
            var msg = stackalloc MsgWalk[1];
            msg->Length = sizeof(MsgWalk);
            msg->Id = 1001;
            msg->TickCount = Environment.TickCount;
            msg->UniqueId = uniqueId;
            msg->X = x;
            msg->Y = y;
            return *msg;
        }
        public static MsgWalk Create(int uniqueId, Vector2 location)
        {
            var msg = stackalloc MsgWalk[1];
            msg->Length = sizeof(MsgWalk);
            msg->Id = 1001;
            msg->TickCount = Environment.TickCount;
            msg->UniqueId = uniqueId;
            msg->X = (int)location.X;
            msg->Y = (int)location.Y;
            return *msg;
        }
        public static implicit operator byte[](MsgWalk msg)
        {
            var buffer = new byte[sizeof(MsgWalk)];
            fixed (byte* p = buffer)
                *(MsgWalk*)p = *&msg;
            return buffer;
        }
        public static implicit operator MsgWalk(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgWalk*)p;
        }
    }
}