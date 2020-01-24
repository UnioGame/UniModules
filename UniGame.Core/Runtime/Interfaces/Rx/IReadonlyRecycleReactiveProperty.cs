namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using UniRx;

    public interface IReadonlyRecycleReactiveProperty<TValue> : 
        IReadOnlyReactiveProperty<TValue>,
        IRecycleObservable<TValue>, 
        IContainerValueStatus
    {
    }
}