using Pixel.ECS.Components;
using Pixel.ECS.Systems;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Scenes;
using PixelShared.IO;
using PixelShared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Login
    {
        internal static void Handle(MsgLogin packet)
        {
            var (user, pass) = packet.GetUserPass();
            var scene = SceneManager.ActiveScene;

            var player = scene.Find<Player>();

            FConsole.WriteLine("[Net][MsgLogin] Login Packet for Player " + user + " using password: " + pass);

            if (player.Has<NetworkComponent>())
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " failed to authenticate! (not implemented)");
                scene.Destroy(player);
            }
            else
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " authenticated! (not implemented)");
                player.Add(new NetworkComponent(scene, player.EntityId, packet.UniqueId));
                NetworkSystem.ConnectionState = ConnectionState.Authenticated;
            }
        }
    }
}