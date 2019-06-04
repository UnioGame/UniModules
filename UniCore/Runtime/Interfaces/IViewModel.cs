namespace UniGreenModules.UniCore.Runtime.Views
{
    using DataFlow;
    using ObjectPool.Interfaces;
    using UniRx;

    public interface IViewModel<TModel> : IPoolable
    {
        ILifeTime LifeTime { get; }
        IReadOnlyReactiveProperty<TModel> Model { get; }
        void Initialize(TModel model);
    }
}