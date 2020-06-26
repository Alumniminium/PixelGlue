using System.Collections.Generic;
using System.Net.Sockets;

namespace PixelShared.TerribleSockets.Pools
{
    public static class SocketAsyncEventArgsPool
    {
        private static readonly Queue<SocketAsyncEventArgs> Pool = new Queue<SocketAsyncEventArgs>();

        public static void Fill(int amount = 16)
        {
            for (var i = 0; i < amount; i++)
                Pool.Enqueue(new SocketAsyncEventArgs());
        }

        public static SocketAsyncEventArgs Get()
        {
            if (Pool.Count == 0)
                Fill();
            return Pool.Dequeue();
        }
        public static void Return(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
            Pool.Enqueue(args);
        }
    }
}