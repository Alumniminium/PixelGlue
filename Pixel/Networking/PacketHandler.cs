using Pixel.Networking.Handlers;
using Shared;
using Shared.IO;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Pixel.Networking
{
    public static class PacketHandler
    {
        public static void Handle(byte[] buffer)
        {
            var packetId = BitConverter.ToUInt16(buffer, 4);
            if (Global.Verbose)
                FConsole.WriteLine($"Got Packet {packetId}");
            switch (packetId)
            {
                case 1000:
                    Login.Handle(buffer);
                    break;
                case 1001:
                    Walk.Handle(buffer);
                    break;
                case 1002:
                    Ping.Handle(buffer);
                    break;
                case 1003:
                    Spawn.Handle(buffer);
                    break;
            }
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}