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
            var player = scene.CreateEntity<Player>(packet.UniqueId,new PositionComponent(1,256,256,0), new InputComponent(),new MoveComponent(1,64, 256, 256),new CameraFollowTagComponent(1,1),new Networked(1));
            scene.AddDrawable(new DrawableComponent(packet.UniqueId,"character.png", new Rectangle(0, 2, 16, 16)));
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