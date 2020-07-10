using Shared.TerribleSockets.Client;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace Shared.TerribleSockets.Queues
{

    public static class SendQueue
    {
        private static readonly ConcurrentQueue<SocketAsyncEventArgs> Queue = new ConcurrentQueue<SocketAsyncEventArgs>();
        private static readonly AutoResetEvent Sync = new AutoResetEvent(false);
        private static Thread _workerThread;

        public static void Start()
        {
            if (_workerThread == null)
                _workerThread = new Thread(WorkLoop) { IsBackground = true };
            if (!_workerThread.IsAlive)
                _workerThread.Start();
        }

        public static void Add(SocketAsyncEventArgs e)
        {
            Queue.Enqueue(e);
            Sync.Set();
        }

        private static void WorkLoop()
        {
            while (true)
            {
                Sync.WaitOne();
                while (Queue.TryDequeue(out var e))
                {
                    var connection = (ClientSocket)e.UserToken;

                    if(connection.Socket.SendAsync(e))
                        connection.SendSync.Set();
                }
            }
        }
    }
}
