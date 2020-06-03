using PixelGlueCore.ECS.Systems;
using PixelGlueCore;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Enums;
using PixelGlueCore.Helpers;
using PixelGlueCore.Networking;
using System.Linq;
using TerribleSockets.Packets;
using PixelGlueCore.Entities;
using PixelGlueCore.Scenes;

namespace PixelGlueCore.Networking.Handlers
{
    public static class Login
    {
        internal static void Handle(MsgLogin packet)
        {
            var (user, pass) = packet.GetUserPass();
            //Networked networked;

            var player = SceneManager.Find<Player>();//PixelGlue.CurrentScenes.Where(ob=> ob.GameObjects.First(g => g.Value.TryGetComponent<Networked>(out networked)))).Value;

            FConsole.WriteLine("[Net][MsgLogin] Login Packet for Player " + user + " using password: " + pass);

            if (player.UniqueId == 0)
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " failed to authenticate! (not implemented)");
            }
            else
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " authenticated! (not implemented)");
                player.UniqueId = packet.UniqueId;
                NetworkSystem.ConnectionState = ConnectionState.Authenticated;
            }
        }
    }
}