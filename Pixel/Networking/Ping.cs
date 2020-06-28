using Pixel.ECS.Systems;
using Pixel.Helpers;
using PixelShared.TerribleSockets.Packets;
using PixelShared.IO;

namespace Pixel.Networking.Handlers
{
    public static class Ping
    {
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