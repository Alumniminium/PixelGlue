using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.ECS
{
    public class PixelSystem<T> : PixelSystem where T : struct 
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        internal override bool MatchesFilter(ref Entity entity) => Pattern<T>.Match(entity);

        public override void Update(float deltaTime, List<Entity> entities)
        {
        }
    }
    public class PixelSystem<T,T2> : PixelSystem where T : struct where T2 : struct
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        internal override bool MatchesFilter(ref Entity entity) => Pattern<T,T2>.Match(entity); 
        public override void Update(float deltaTime, List<Entity> entities)
        {
        }
    }
    public class PixelSystem<T,T2,T3> : PixelSystem where T : struct where T2 : struct where T3 : struct
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        internal override bool MatchesFilter(ref Entity entity) => Pattern<T,T2,T3>.Match(entity);
         public override void Update(float deltaTime, List<Entity> entities)
        {
        }
    }
    public class PixelSystem<T,T2,T3,T4> : PixelSystem where T : struct where T2 : struct where T3 : struct where T4 : struct 
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        internal override bool MatchesFilter(ref Entity entity) => Pattern<T,T2,T3,T4>.Match(entity);
        public override void Update(float deltaTime, List<Entity> entities)
        {
        }
    }
    public abstract class PixelSystem
    {
        public bool IsActive { get; set; }
        public string Name { get; set; } = "Unnamed System";
        public bool WantsUpdate, WantsDraw;
        
        private int readyThreads;
        private int _counter;
        public List<Entity>[] Entities;
        public Thread[] Threads;
        public AutoResetEvent Block;
        private float CurrentDeltaTime;

        public PixelSystem(bool doUpdate, bool doDraw,int threads=1)
        {
            WantsUpdate = doUpdate;
            WantsDraw = doDraw;

            Entities = new List<Entity>[threads];
            Threads=new Thread[threads];
            Block = new AutoResetEvent(false);

            for (int i = 0; i < Threads.Length; i++)
            {
                Entities[i] = new List<Entity>();
                Threads[i] = new Thread(WaitLoop);
                Threads[i].Name = Name + " Thread #" + i;
                Threads[i].IsBackground=true;
                Threads[i].Start(i);
            }
        }

        private void WaitLoop(object ido)
        {
            var idx = (int)ido;
            while(true)
            {
                Interlocked.Increment(ref readyThreads);
                Block.WaitOne();
                Update(CurrentDeltaTime, Entities[idx]);
            }
        }

        public virtual void Initialize(){ IsActive = true;}

        public void Update(float deltaTime) 
        {
            CurrentDeltaTime= deltaTime;
            readyThreads=0;

            Block.Set();
            while(readyThreads < Threads.Length)
                Thread.Sleep(0); // wait for threads to finish
        }
        public abstract void Update(float deltaTime, List<Entity> entities);

        public virtual void Draw(SpriteBatch spriteBatch){}

        internal virtual bool MatchesFilter(ref Entity entityId)=>false;
        private void AddEntity(ref Entity entity)
        {
            Entities[_counter++].Add(entity);
            
            if(_counter==Threads.Length)
                _counter=0;
        }
        private void RemoveEntity(ref Entity entity)
        {
            foreach(var list in Entities)
                list.Remove(entity);
        }

        internal void EntityChanged(ref Entity entity)
        {
                if(MatchesFilter(ref entity))
                    AddEntity(ref entity);
                else   
                    RemoveEntity(ref entity);
        }
    }
}