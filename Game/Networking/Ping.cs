using PixelGlueCore.ECS.Systems;
using PixelGlueCore.Helpers;
using TerribleSockets.Packets;

namespace PixelGlueCore.Networking.Handlers
{
    public class Ping
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