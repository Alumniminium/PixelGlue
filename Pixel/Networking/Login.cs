using Microsoft.Xna.Framework;
using Pixel.ECS.Components;
using Pixel.ECS.Systems;
using Pixel.Entities;
using Pixel.Enums;
using Pixel.Scenes;
using Shared.IO;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Login
    {
        internal static void Handle(MsgLogin packet)
        {
            var scene = SceneManager.ActiveScene;

            scene.Player.NameTag.Text = $"Name: {packet.GetUsername()}";
            FConsole.WriteLine("[Net][MsgLogin] " + packet.GetUsername() + " authenticated! (not implemented)");
            scene.Player.Add(new NetworkComponent(packet.UniqueId));
            scene.UniqueIdToEntityId.TryAdd(packet.UniqueId, scene.Player.EntityId);
            scene.EntityIdToUniqueId.TryAdd(scene.Player.EntityId, packet.UniqueId);
            NetworkSystem.ConnectionState = ConnectionState.Authenticated;
        }
    }
}