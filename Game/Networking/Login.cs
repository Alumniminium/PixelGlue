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
            scene.AddDrawable(new DrawableComponent(packet.UniqueId,"character.png", new Rectangle(0, 2, 16, 16)));
            scene.AddMovable(new MoveComponent(packet.UniqueId,64, 256, 256));
            scene.AddPosition(new PositionComponent(packet.UniqueId,256,256,0));
            scene.AddInput(new InputComponent(packet.UniqueId));
            scene.AddCameraFollowTag(new CameraFollowTagComponent(packet.UniqueId,2));
            scene.AddNetworked(new Networked(packet.UniqueId));

            FConsole.WriteLine("[Net][MsgLogin] Login Packet for Player " + user + " using password: " + pass);

            if (player.UniqueId == 0)
            {
                FConsole.WriteLine("[Net][MsgLogin] " + user + " failed to authenticate! (not implemented)");
                scene.Destroy(player);
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