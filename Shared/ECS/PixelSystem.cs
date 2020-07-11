using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.ECS
{
    public class PixelSystem
    {
        public virtual string Name { get; set; } = "Unnamed System";
        public bool IsActive { get; set; } = true;
        public List<Entity> Entities { get; set; } = new List<Entity>();
        private Thread[] Workers;
        private AutoResetEvent[] Blocks;
        public bool WantsUpdate,WantsDraw;

        public PixelSystem(bool doUpdate,bool doDraw)
        {
            WantsUpdate=doUpdate;
            WantsDraw=doDraw;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnblockThreads()
        {
            for (int i = 0; i < Blocks.Length; i++)
                Blocks[i].Set();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AsyncUpdateLoop(object idx)
        {
            int id = (int)idx;
            while (true)
            {
                Blocks[id].WaitOne();
                AsyncUpdate(id);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AsyncUpdate(int id) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Initialize() { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Update(float deltaTime) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void FixedUpdate(float deltaTime) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Draw(SpriteBatch spriteBatch) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AddEntity(Entity entity) => Global.PostUpdateQueue.Enqueue(() =>
        {
            if (!Entities.Contains(entity))
                Entities.Add(entity);
        });
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void RemoveEntity(Entity entity) => Global.PostUpdateQueue.Enqueue(() => Entities.Remove(entity));
    }
}