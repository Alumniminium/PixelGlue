using Microsoft.Xna.Framework;
using PixelShared.Enums;
using System.Runtime.InteropServices;

namespace PixelShared.TerribleSockets.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgSpawn
    {
        public int Length;
        public ushort Id;
        public int UniqueId;
        public int X, Y;
        public int Model;
        public Direction Direction;
        
        public static MsgSpawn Create(int uniqueId, int x, int y, int model, string name)
        {
            var msg = stackalloc MsgSpawn[1];
            msg->Length = sizeof(MsgSpawn);
            msg->Id = 1003;
            msg->Model = model;
            msg->UniqueId = uniqueId;
            msg->X = x;
            msg->Y = y;
            msg->Direction = Direction.North;
            return *msg;
        }
        public static MsgSpawn Create(int uniqueId, Vector2 pos, int model)
        {
            var msg = stackalloc MsgSpawn[1];
            msg->Length = sizeof(MsgSpawn);
            msg->Id = 1003;
            msg->Model = model;
            msg->UniqueId = uniqueId;
            msg->X = (int)pos.X;
            msg->Y = (int)pos.Y;
            msg->Direction = Direction.North;
            return *msg;
        }
        public static implicit operator byte[](MsgSpawn msg)
        {
            var buffer = new byte[sizeof(MsgSpawn)];
            fixed (byte* p = buffer)
                *(MsgSpawn*)p = *&msg;
            return buffer;
        }
        public static implicit operator MsgSpawn(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgSpawn*)p;
        }
    }
}