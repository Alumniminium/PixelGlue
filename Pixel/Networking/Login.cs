using Pixel.ECS.Systems;
using Pixel.ECS.Components;
using Pixel.Enums;
using Pixel.Helpers;
using PixelShared.TerribleSockets.Packets;
using Pixel.Entities;
using Pixel.Scenes;
using Microsoft.Xna.Framework;
using PixelShared.IO;

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

            if (!player.Has<Networked>())
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " failed to authenticate! (not implemented)");
                scene.Destroy(player);
            }
            else
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " authenticated! (not implemented)");
                player.Add(new Networked(scene,player.EntityId,packet.UniqueId));
                NetworkSystem.ConnectionState = ConnectionState.Authenticated;
            }
        }
    }
}