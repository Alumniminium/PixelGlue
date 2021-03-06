using System.Runtime.CompilerServices;
using Pixel.ECS.Components;
using Pixel.ECS.Systems;
using Pixel.Enums;
using Pixel.Scenes;
using Shared;
using Shared.ECS;
using Shared.ECS.Components;
using Shared.IO;
using Shared.TerribleSockets.Packets;

namespace Pixel.Networking.Handlers
{
    public static class Login
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Handle(MsgLogin packet)
        {
            var player = TestingScene.Player;
            player.Entity.Add(new NetworkComponent(packet.UniqueId));
            World.RegisterUniqueIdFor(player.Entity.EntityId, packet.UniqueId);

            var child = World.GetEntity(player.Entity.Children.Find(c => World.GetEntity(c).Has<TextComponent>()));
            ref var txt = ref child.Get<TextComponent>();
            txt.Value = $"Name: {packet.GetUsername()}";
            
            NetworkSystem.ConnectionState = ConnectionState.Authenticated;
            FConsole.WriteLine("[Net][MsgLogin] " + txt.Value + " authenticated! (not implemented)");
        }
    }
}