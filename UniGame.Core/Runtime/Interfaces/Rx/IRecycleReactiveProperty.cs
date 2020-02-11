namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using ObjectPool.Runtime.Interfaces;
    using UniRx;

    public interface IRecycleReactiveProperty<TValue> : 
        IReactiveProperty<TValue>,
        IReadonlyRecycleReactiveProperty<TValue>,
        IValueContainerStatus,
        IDespawnable
    {
        new TValue Value { get; set; }

    }
}
