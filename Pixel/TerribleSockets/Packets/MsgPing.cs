using System;
using System.Runtime.InteropServices;

namespace Pixel.TerribleSockets.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgPing
    {
        public int Length;
        public ushort Id;
        public int UniqueId;
        public long TickCount;
        public short Ping;

        public static MsgPing Create(int uniqueId)
        {
            var msg = stackalloc MsgPing[1];
            msg->Length = sizeof(MsgPing);
            msg->Id = 1002;
            msg->TickCount = DateTime.UtcNow.Ticks;
            msg->UniqueId = uniqueId;
            return *msg;
        }
        public static implicit operator byte[](MsgPing msg)
        {
            var buffer = new byte[sizeof(MsgPing)];
            fixed (byte* p = buffer)
                *(MsgPing*)p = *&msg;
            return buffer;
        }
        public static implicit operator MsgPing(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgPing*)p;
        }
    }
}