using PixelGlueCore.ECS.Systems;
using PixelGlueCore.ECS.Components;
using PixelGlueCore.Enums;
using PixelGlueCore.Helpers;
using TerribleSockets.Packets;
using PixelGlueCore.Entities;
using PixelGlueCore.Scenes;
using Microsoft.Xna.Framework;

namespace PixelGlueCore.Networking.Handlers
{
    public static class Login
    {
        internal static void Handle(MsgLogin packet)
        {
            var (user, pass) = packet.GetUserPass();
            var scene = SceneManager.ActiveScenes[^1];
            var player = scene.CreateEntity<Player>(packet.UniqueId);
            player.Add(new DrawableComponent(player.EntityId,"character.png", new Rectangle(0, 2, 16, 16)));
            player.Add(new MoveComponent(player.EntityId,24, 256, 264));
            player.Add(new PositionComponent(player.EntityId,256,264,0));
            player.Add(new InputComponent(player.EntityId));
            player.Add(new CameraFollowTagComponent(player.EntityId,1));
            player.Add(new Networked(packet.UniqueId));

            FConsole.WriteLine("[Net][MsgLogin] Login Packet for Player " + user + " using password: " + pass);

            if (player.EntityId == 0)
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " failed to authenticate! (not implemented)");
                scene.Destroy(player);
            }
            else
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " authenticated! (not implemented)");
                //player.EntityId = packet.UniqueId;
                NetworkSystem.ConnectionState = ConnectionState.Authenticated;
            }
        }
    }
}