using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Pixel.TerribleSockets.Client;
using Pixel.TerribleSockets.Queues;

namespace Pixel.TerribleSockets.Server
{
    public static class ServerSocket
    {
        internal static Socket Socket;
        internal static SocketAsyncEventArgs AcceptArgs;
        internal static AutoResetEvent AcceptSync = new AutoResetEvent(true);
        public static void Start(ushort port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true,
            };
            Socket.Bind(new IPEndPoint(IPAddress.Any, port));
            Socket.Listen(100);

            AcceptArgs = new SocketAsyncEventArgs();
            AcceptArgs.Completed += Accepted;
            AcceptArgs.UserToken = new ClientSocket(null);
            StartAccepting();
        }

        private static void StartAccepting()
        {
            AcceptSync.WaitOne();
            if (!Socket.AcceptAsync(AcceptArgs))
                Accepted(null, AcceptArgs);
        }

        private static void Accepted(object sender, SocketAsyncEventArgs e)
        {
            var connection = (ClientSocket)e.UserToken;
            ((ClientSocket)connection.ReceiveArgs.UserToken).Socket = e.AcceptSocket;

            try
            {
                if (!e.AcceptSocket.ReceiveAsync(connection.ReceiveArgs))
                    Received(null, connection.ReceiveArgs);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                CloseClientSocket(e);
                AcceptSync.Set();
            }
            e.AcceptSocket = null;
            e.UserToken = new ClientSocket(null);
            AcceptSync.Set();
            StartAccepting();
        }

        internal static void Received(object sender, SocketAsyncEventArgs e)
        {
            var token = (ClientSocket)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                try
                {
                    ReceiveQueue.Add(e);
                    token.ReceiveSync.WaitOne();
                    if (!token.Socket.ReceiveAsync(e))
                        Received(null,e);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    CloseClientSocket(e);
                }
            }
            else
                CloseClientSocket(e);
        }

        private static void CloseClientSocket(SocketAsyncEventArgs e)
        {
            var token = (ClientSocket)e.UserToken;
            token?.Socket?.Dispose();
        }
    }
}
