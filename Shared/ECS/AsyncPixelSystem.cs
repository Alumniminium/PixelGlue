using System;
using System.Collections.Generic;
using System.Threading;
using Shared.IO;

namespace Shared.ECS
{
    public class WorkerThread
    {
        public bool Ready;
        public Thread Worker;
        public AutoResetEvent Block;
        public List<int> Entities;
        public Action<WorkerThread> Target;

        public WorkerThread(Action<WorkerThread> targetMethod, ThreadPriority priority)
        {
            Target = targetMethod;
            Block = new AutoResetEvent(false);
            Entities = new List<int>();
            Worker = new Thread(WorkLoop)
            {
                IsBackground = true,
                Priority = priority
            };
        }
        public void Start() => Worker.Start();
        public void WorkLoop()
        {
            while (true)
            {
                Ready = true;
                Block.WaitOne();
                Target.Invoke(this);
                Entities.Clear();
            }
        }
    }
    public class AsyncPixelSystem : PixelSystem
    {
        public WorkerThread[] WorkerThreads;
        public AsyncPixelSystem(bool doUpdate, bool doDraw) : base(doUpdate, doDraw) { }

        public void StartWorkerThreads(int count, ThreadPriority priority)
        {
            WorkerThreads = new WorkerThread[count];
            for (int i = 0; i < count; i++)
            {
                WorkerThreads[i] = new WorkerThread(ThreadedUpdate, priority);
                WorkerThreads[i].Start();
            }
        }
        public void UnblockThreads()
        {
            var idx = 0;

            foreach (var entity in Entities)
            {
                var wt = WorkerThreads[idx];

                if (wt.Ready)
                    wt.Entities.Add(entity);
                else
                    FConsole.WriteLine("this shouldnt happen");
                    
                idx++;

                if (idx == WorkerThreads.Length)
                    idx = 0;
            }
            foreach (var wt in WorkerThreads)
            {
                if (wt.Ready)
                {   
                    wt.Ready=false;
                    wt.Block.Set();
                }
            }
            foreach (var thread in WorkerThreads)
            {
                while (!thread.Ready)
                    Thread.Yield();
            }
        }
        public virtual void ThreadedUpdate(WorkerThread wt)
        {
            
        }
    }
}