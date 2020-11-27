using System;
using System.Collections.Generic;
using System.Threading;
using Shared.IO;

namespace Shared.ECS
{
    public class WorkerThread
    {
        public int Id;
        public bool Ready;
        public Thread Worker;
        public AutoResetEvent Block;
        public List<int> Entities;
        public Action<WorkerThread> Target;
        public DateTime StartTime,StopTime;

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
                Ready=false;
                StartTime=DateTime.UtcNow;
                Target.Invoke(this);
                StopTime=DateTime.UtcNow;
            }
        }
    }
    public class AsyncPixelSystem : PixelSystem
    {
        public int Idx=0;
        public WorkerThread[] WorkerThreads;
        public AsyncPixelSystem(bool doUpdate, bool doDraw, int threads) : base(doUpdate, doDraw)
        {             
            WorkerThreads = new WorkerThread[threads];
            for (int i = 0; i < threads; i++)
            {
                WorkerThreads[i] = new WorkerThread(ThreadedUpdate, ThreadPriority.Normal);
                WorkerThreads[i].Id=i;
                WorkerThreads[i].Start();
            }
        }
        public override void Update(float deltaTime) => UnblockThreads();
        public void UnblockThreads()
        {
            foreach (var entity in Entities)
            {
                var wt = WorkerThreads[Idx];
                wt.Entities.Add(entity);
                    
                Idx++;

                if (Idx == WorkerThreads.Length)
                    Idx = 0;
            }
            foreach (var wt in WorkerThreads)
                wt.Block.Set();
            foreach (var thread in WorkerThreads)
                while (!thread.Ready)
                    Thread.Yield();
        }
        public virtual void ThreadedUpdate(WorkerThread wt) => wt.Entities.Clear();

        public override void EntityChanged(ref Entity entity)
        {
            if (MatchesFilter(entity))
            {
                if(removedEntities.Contains(entity.EntityId))
                    removedEntities.Remove(entity.EntityId);

                for(int i = 0; i < WorkerThreads.Length; i++)
                {
                    if (Entities.Contains(entity.EntityId))
                        return;
                }

                if (addedEntities.Contains(entity.EntityId))
                    return;
                    
                addedEntities.Add(entity.EntityId);
            }
            else if (Entities.Contains(entity.EntityId) && !removedEntities.Contains(entity.EntityId))
                removedEntities.Add(entity.EntityId);
        }

        public override void EntityRemoved(ref Entity entity) => Entities.Remove(entity.EntityId);
    }
}