namespace UniGreenModules.UniCore.Runtime.ObjectPool.Runtime
{
    using UniCore.Runtime.Interfaces;

    public interface IPoolItemsContainer<T> : IResetable where T : class
    {
        T Spawn();
        void Despawn(T item);
        void Reset();
    }
}