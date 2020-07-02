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

            scene.Player.NameTag.Text = $"Name: {packet.GetUsername()}";
            FConsole.WriteLine("[Net][MsgLogin] " + packet.GetUsername() + " authenticated! (not implemented)");
            scene.Player.Add(new NetworkComponent(scene, scene.Player.EntityId, packet.UniqueId));
            NetworkSystem.ConnectionState = ConnectionState.Authenticated;
        }
    }
}