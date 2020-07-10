using System.Runtime.CompilerServices;
using Pixel.ECS.Systems;
using Shared.IO;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Ping
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Handle(byte[] buffer)
        {
            var msgPing = (MsgPing)buffer;

            if (msgPing.Ping == 0)
                NetworkSystem.Send(msgPing);
            else
                FConsole.WriteLine("[Net][MsgPing] Ping: " + msgPing.Ping);
        }
    }
}