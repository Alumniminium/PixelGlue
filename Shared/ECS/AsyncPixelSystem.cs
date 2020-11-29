using System;
using System.Collections.Generic;
using System.Threading;

namespace Shared.ECS
{
    public class WorkerThread
    {
        public int Id;
        public volatile bool Ready;
        private Thread Worker;
        public AutoResetEvent Block;
        public List<int> Entities,ToBeAdded;
        private Action<WorkerThread> Target;
        public DateTime StartTime,StopTime;

        public WorkerThread(Action<WorkerThread> targetMethod, ThreadPriority priority)
        {
            Target = targetMethod;
            Block = new AutoResetEvent(false);
            Entities = new List<int>();
            ToBeAdded = new List<int>();
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

            lock(ToBeAdded)
                foreach(var id in ToBeAdded)
                    Entities.Add(id);

            lock(ToBeAdded)
                ToBeAdded.Clear();
                
                Target.Invoke(this);

                StopTime=DateTime.UtcNow;
            }
        }
        public void AddEntity(int entityId) 
        {
            lock(ToBeAdded)
             ToBeAdded.Add(entityId);
        }
    }
    public class AsyncPixelSystem : PixelSystem
    {
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
        public override void Update(float deltaTime)
        {
            int threadIndex=0;
            foreach (var entity in Entities)
            {
                var wt = WorkerThreads[threadIndex];
                wt.AddEntity(entity);
                    
                threadIndex++;

                if (threadIndex == WorkerThreads.Length)
                    threadIndex = 0;
            }
            foreach (var wt in WorkerThreads)
            {
                wt.StartTime = DateTime.UtcNow;
                wt.Block.Set();
            }
            
            foreach (var wt in WorkerThreads)
            {
                while (!wt.Ready)
                    Thread.Yield();
                wt.StopTime=DateTime.UtcNow;
            }
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

        public override void EntityRemoved(int entityId) => Entities.Remove(entityId);
    }
}