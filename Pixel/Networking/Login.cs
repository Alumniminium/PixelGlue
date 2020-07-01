using Microsoft.Xna.Framework;
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
            var scene = SceneManager.ActiveScene;
            var player = scene.Find<Player>();

            if (player != null)
                scene.Destroy(player);

            player = scene.CreateEntity<Player>(packet.UniqueId);
            player.NameTag.Text = $"Name: {packet.GetUsername()}";
            FConsole.WriteLine("[Net][MsgLogin] " + packet.GetUsername() + " authenticated! (not implemented)");
            player.Add(new NetworkComponent(scene, player.EntityId, packet.UniqueId));
            NetworkSystem.ConnectionState = ConnectionState.Authenticated;
        }
    }
}