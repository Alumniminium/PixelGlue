using System.Runtime.CompilerServices;
using Pixel.ECS.Components;
using Pixel.ECS.Systems;
using Pixel.Enums;
using Pixel.Scenes;
using Shared.ECS;
using Shared.IO;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Login
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Handle(MsgLogin packet)
        {
            var scene = SceneManager.ActiveScene;

            var child = World.Entities[scene.Player.Children.Find(c => World.Entities[c].Has<TextComponent>())];
            ref var txt = ref child.Get<TextComponent>();
            txt.Value = $"Name: {packet.GetUsername()}";
            
            FConsole.WriteLine("[Net][MsgLogin] " + packet.GetUsername() + " authenticated! (not implemented)");

            scene.Player.Add(new NetworkComponent(packet.UniqueId));
            World.UniqueIdToEntityId.TryAdd(packet.UniqueId, scene.Player.EntityId);
            World.EntityIdToUniqueId.TryAdd(scene.Player.EntityId, packet.UniqueId);
            scene.Player.Register();
            NetworkSystem.ConnectionState = ConnectionState.Authenticated;
        }
    }
}