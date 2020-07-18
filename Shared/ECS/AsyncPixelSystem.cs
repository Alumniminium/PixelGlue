using System.Threading;

namespace Shared.ECS
{
    public class AsyncPixelSystem : PixelSystem
    {
        private Thread[] Workers;
        private AutoResetEvent[] Blocks;
        public AsyncPixelSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw){}
        
        public void StartWorkerThreads(int count, bool blockState, ThreadPriority priority)
        {
            Workers = new Thread[count];
            Blocks = new AutoResetEvent[count];
            for (int i = 0; i < count; i++)
            {
                Blocks[i] = new AutoResetEvent(blockState);
                Workers[i] = new Thread(AsyncUpdateLoop)
                {
                    IsBackground = true,
                    Priority = priority
                };
                Workers[i].Start(i);
            }
        }
        public void UnblockThreads()
        {
            for (int i = 0; i < Blocks.Length; i++)
                Blocks[i].Set();
        }
        private void AsyncUpdateLoop(object idx)
        {
            int id = (int)idx;
            while (true)
            {
                Blocks[id].WaitOne();
                AsyncUpdate(id);
            }
        }
        public virtual void AsyncUpdate(int id) { }
    }
}