using Microsoft.Xna.Framework;
using Shared.Enums;
using System.Runtime.InteropServices;
using System.Text;

namespace Shared.TerribleSockets.Packets
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
        public fixed byte Name[32];

        public string GetName()
        {
            fixed (byte* p = Name)
                return Encoding.ASCII.GetString(p, 32).Trim('\0');
        }

        public void SetName(string username)
        {
            fixed (byte* p = Name)
            {
                for (var i = 0; i < username.Length; i++)
                    p[i] = (byte)username[i];
            }
        }
        
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
            msg->SetName(name);
            return *msg;
        }
        public static MsgSpawn Create(int uniqueId, Vector2 pos, int model,string name) => Create(uniqueId,(int)pos.X,(int)pos.Y,model,name);
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