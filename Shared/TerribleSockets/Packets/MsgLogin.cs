using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Shared.TerribleSockets.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgLogin
    {
        public int Length;
        public ushort Id;
        public int UniqueId;
        public fixed byte Username[32];
        public fixed byte Password[32];

        public string GetUsername()
        {
            fixed (byte* p = Username)
                return Encoding.ASCII.GetString(p, 32).Trim('\0');
        }
        public string GetPassword()
        {
            fixed (byte* p = Password)
                return Encoding.ASCII.GetString(p, 32).Trim('\0');
        }

        public (string user, string pass) GetUserPass()
        {
            var stringBuilder = new StringBuilder();
            var pair = ValueTuple.Create(string.Empty, string.Empty);
            fixed (byte* p = Username)
            {
                for (var i = 0; i < 64; i++)
                {
                    if (i == 32)
                    {
                        pair.Item1 = stringBuilder.ToString().Trim('\0');
                        stringBuilder.Clear();
                    }
                    stringBuilder.Append((char)p[i]);
                }
                pair.Item2 = stringBuilder.ToString().Trim('\0');
            }
            return pair;
        }

        public void SetUsername(string username)
        {
            fixed (byte* p = Username)
            {
                for (var i = 0; i < username.Length; i++)
                    p[i] = (byte)username[i];
            }
        }
        public void SetPassword(string password)
        {
            fixed (byte* p = Password)
            {
                for (var i = 0; i < password.Length; i++)
                    p[i] = (byte)password[i];
            }
        }

        public static MsgLogin Create(string user, string pass)
        {
            var msg = stackalloc MsgLogin[1];
            msg->Length = sizeof(MsgLogin);
            msg->Id = 1000;

            msg->SetUsername(user);
            msg->SetPassword(pass);
            return *msg;
        }
        public static implicit operator byte[](MsgLogin msg)
        {
            var buffer = new byte[sizeof(MsgLogin)];
            fixed (byte* p = buffer)
                *(MsgLogin*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte*(MsgLogin msg)
        {
            var buffer = stackalloc byte[sizeof(MsgLogin)];
            *(MsgLogin*)buffer = msg;
            return buffer;
        }
        public static implicit operator MsgLogin(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgLogin*)p;
        }
        public static implicit operator MsgLogin(byte* msg) => *(MsgLogin*)msg;
        public static implicit operator MsgLogin*(MsgLogin msg) => *(MsgLogin*)msg;
    }
}