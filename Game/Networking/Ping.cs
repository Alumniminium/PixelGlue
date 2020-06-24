using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Helpers;
using Pixel.TerribleSockets.Packets;
using Pixel.IO;

namespace PixelGlueCore.Networking.Handlers
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