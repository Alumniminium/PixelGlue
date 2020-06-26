﻿using System;
using PixelShared.TerribleSockets.Client;

using PixelShared.TerribleSockets.Packets;
namespace Server
{
    public static class PacketHandler
    {
        public static void Handle(ClientSocket socket, byte[] buffer)
        {
            var packetId = BitConverter.ToUInt16(buffer, 4);
            switch (packetId)
            {
                case 1000:
                    {
                        var msgLogin = (MsgLogin)buffer;
                        var user = msgLogin.GetUsername();
                        var pass = msgLogin.GetPassword();

                        Console.WriteLine($"Login request for {user} using password {pass}");

                        msgLogin.UniqueId = Core.Random.Next(1_000_000, 10_000_000);
                        var player = new Player(socket)
                        {
                            UniqueId = msgLogin.UniqueId,
                            Username = user,
                            Password = pass
                        };
                        socket.StateObject = player;

                        foreach (var (uniqueId,entity) in Collections.Players)
                        {
                            if ( entity.Username == user && entity.Password == pass)
                            {
                                player.UniqueId = uniqueId;
                                break;
                            }
                        }

                        Collections.Players.TryAdd(player.UniqueId, player);

                        if (msgLogin.UniqueId != 0)
                            Console.WriteLine("Authentication successful. Your customer Id is: " + player.UniqueId);
                        else
                            Console.WriteLine("Authentication failed.");

                        player.Socket.Send(msgLogin);
                        player.Socket.Send(MsgWalk.Create(player.UniqueId,player.Location));
                        break;
                    }
                case 1001:
                    {
                        var msgWalk = (MsgWalk)buffer;
                        var player = (Player)socket.StateObject;
                        if (player == null)
                            break;
                        player.Location.X = msgWalk.X;
                        player.Location.Y = msgWalk.Y;
                        msgWalk.TickCount = Environment.TickCount;
                        Console.WriteLine($"Player: {player.Username} ({msgWalk.UniqueId}) moved to: {player.Location.X},{player.Location.Y}");

                        foreach (var (_,entity) in Collections.Players)
                        {
                             entity.Socket.Send(msgWalk);
                        }

                        break;
                    }
                case 1002:
                    {
                        var msgPing = (MsgPing)buffer;
                        var delta = DateTime.UtcNow.Ticks - msgPing.TickCount;
                        var ms = delta / 10000;
                        msgPing.Ping = (short)ms;
                        socket.Send(msgPing);
                        Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Received Ping from {socket.Socket.RemoteEndPoint} - {ms}ms.");
                        break;
                    }
            }
        }
    }
}