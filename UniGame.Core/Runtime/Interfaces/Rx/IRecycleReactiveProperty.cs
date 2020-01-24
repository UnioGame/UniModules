namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using ObjectPool.Runtime.Interfaces;

    public interface IRecycleReactiveProperty<TValue> : 
        IReadonlyRecycleReactiveProperty<TValue>,
        IValueContainerStatus,
        IDespawnable
    {
        new TValue Value { get; set; }
    }
}
