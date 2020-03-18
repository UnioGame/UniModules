namespace UniGreenModules.UniCore.Runtime.ObjectPool.Runtime
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Interfaces;

    public class PoolItemsContainer<T> : IPoolItemsContainer<T> where T : class
    {
        private Func<T> factory;
        private Action<T> onDespawn;
        private Action<T> onSpawn;
        private Action<T> onReset;
        
        public Queue<T> Queue;
        public int Count;

        public void Initialize(
            Func<T> factoryFunc = null,
            Action<T> onDespawnAction = null, 
            Action<T> onSpawnAction = null,
            Action<T> onResetAction = null)
        {
            Reset();

            factory = factoryFunc;
            onDespawn = onDespawnAction;
            onSpawn = onSpawnAction;
            onReset = onResetAction;
            
            Queue = ClassPool.Spawn<Queue<T>>();
        }

        public T Spawn()
        {
            var result = Count == 0 ? 
                factory?.Invoke() : 
                Queue.Dequeue();

            Count = Queue.Count;
            if (onSpawn != null && result != null)
                onSpawn(result);
            
            return result;
        }
        
        public void Despawn(T item)
        {
            if (item == null) return;
            if (onDespawn != null)
                onDespawn(item);
            
            Queue.Enqueue(item);
            Count = Queue.Count;
        }
        
        public void Reset()
        {

            if (onReset != null) {
                while (Queue.Count > 0) {
                    var item = Queue.Dequeue();
                    onReset(item);
                }
            }
            
            Count = 0;
            Queue.Clear();
            Queue.DespawnObject(Queue.Clear);
            
            Queue = null;
            factory = null;
            onDespawn = null;
            onSpawn = null;
        }
    }
}
