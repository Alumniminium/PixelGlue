using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.ECS
{
    public class PixelSystem<T> : PixelSystem where T : struct 
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        public override bool MatchesFilter(ref Entity entity) => Pattern<T>.Match(entity);
    }
    public class PixelSystem<T,T2> : PixelSystem where T : struct where T2 : struct
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        public override bool MatchesFilter(ref Entity entity) => Pattern<T,T2>.Match(entity); 
    }
    public class PixelSystem<T,T2,T3> : PixelSystem where T : struct where T2 : struct where T3 : struct
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        public override bool MatchesFilter(ref Entity entity) => Pattern<T,T2,T3>.Match(entity);
    }
    public class PixelSystem<T,T2,T3,T4> : PixelSystem where T : struct where T2 : struct where T3 : struct where T4 : struct 
    {
        public PixelSystem(bool doUpdate, bool doDraw, int threads = 1) : base(doUpdate, doDraw, threads) { }
        public override bool MatchesFilter(ref Entity entity) => Pattern<T,T2,T3,T4>.Match(entity);
    }
    public class GCNeutralList<T>
    {
        private T[] Items;
        private Stack<int> availableIndicies;

        public GCNeutralList(int maxCountItems)
        {
            Items = new T[maxCountItems];
            availableIndicies=new Stack<int>(maxCountItems);
            Clear();
        }

        public ref T this[int index] { get => ref Items[index]; }

        public int Count => (Items.Length - availableIndicies.Count);

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if(availableIndicies.TryPop(out int arrayIdx))
            {
                Items[arrayIdx]=item;
            }
            else
                throw new System.Exception("No more space in this list u fool!");
        }
        public bool Remove(T item)
        {
            for(int i = 0; i < (Items.Length-availableIndicies.Count); i++)
            //for(int i = 0; i< Items.Length; i++)
            {
                if(Items[i].Equals(item))
                {
                    availableIndicies.Push(i);
                    return true;
                }
            }
            return false;
        }
        public bool Contains(T item)
        {
            // can be optimized, we know how many items are inside
            // however, that would require us to defrag the array i think?
            for(int i = 0; i < (Items.Length-availableIndicies.Count); i++)
            //for(int i = 0; i < Items.Length; i++)
            {
                if(Items[i].Equals(item))
                    return true;
            }
            return false;
        }

        public int IndexOf(T item)
        {
            for(int i = 0; i < (Items.Length-availableIndicies.Count); i++)
            //for(int i = 0; i< Items.Length; i++)
            {
                if(Items[i].Equals(item))
                    return i;
            }
            return -1;
        }
        public void Clear()
        {
            foreach(var i in Enumerable.Range(0,Items.Length).Reverse())
                availableIndicies.Push(i);
        }

    }
    public abstract class PixelSystem
    {
        public bool IsActive { get; set; }
        public string Name { get; set; } = "Unnamed System";
        public bool WantsUpdate, WantsDraw;
        
        private int readyThreads;
        private int _counter;
        public GCNeutralList<Entity>[] Entities;
        public Thread[] Threads;
        public SemaphoreSlim Block;
        private float CurrentDeltaTime;

        public PixelSystem(bool doUpdate, bool doDraw,int threads=1)
        {
            WantsUpdate = doUpdate;
            WantsDraw = doDraw;

            Entities = new GCNeutralList<Entity>[threads];
            Threads=new Thread[threads];
            Block = new SemaphoreSlim(0);

            for (int i = 0; i < Threads.Length; i++)
            {
                Entities[i] = new GCNeutralList<Entity>(100_000);
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
                Block.Wait();
                Update(CurrentDeltaTime, Entities[idx]);
            }
        }

        public virtual void Initialize(){ IsActive = true;}

        public void Update(float deltaTime) 
        {
            CurrentDeltaTime= deltaTime;
            readyThreads=0;

            Block.Release(Threads.Length);
            while(readyThreads < Threads.Length)
                Thread.Yield(); // wait for threads to finish
        }
        public virtual void Update(float deltaTime, GCNeutralList<Entity> entities){}
        public virtual void Draw(SpriteBatch spriteBatch){}
        public virtual bool MatchesFilter(ref Entity entityId)=>false;
        private bool ContainsEntity(ref Entity entity)
        {
            for(int i =0;i<Threads.Length;i++)
            {
                if (Entities[i].Contains(entity))
                {
                    return true;
                }
            }
            return false;
        }
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
            var isMatch = MatchesFilter(ref entity);
            var isNew = !ContainsEntity(ref entity);

            if(isMatch && isNew)
                AddEntity(ref entity);
            else if(!isMatch && !isNew)   
                RemoveEntity(ref entity);
        }
    }
}