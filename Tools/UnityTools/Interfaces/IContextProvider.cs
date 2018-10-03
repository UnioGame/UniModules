using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Interfaces
{

    public interface IContextProvider<TContext> : IPoolable
    {
        TData Get<TData>(TContext context);
        void RemoveContext(TContext context);
        void Remove<TData>(TContext context);
        void AddValue<TData>(TContext context, TData value);

    }
}
