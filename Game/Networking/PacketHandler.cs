using PixelGlueCore.ECS;
using PixelGlueCore.Networking.Handlers;
using System;
using System.Buffers;

namespace PixelGlueCore.Networking
{
    public static class PacketHandler
    {
        public static void Handle(byte[] buffer)
        {
            var packetId = BitConverter.ToUInt16(buffer, 4);
            switch (packetId)
            {
                case 1000:
                    {
                        Login.Handle(buffer);
                        break;
                    }
                case 1001:
                    {
                        Walk.Handle(buffer);
                        break;
                    }
                case 1002:
                    {
                        Ping.Handle(buffer);
                        break;
                    }
            }
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}