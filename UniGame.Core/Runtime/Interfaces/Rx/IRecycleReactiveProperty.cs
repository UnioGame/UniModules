namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using System;
    using ObjectPool.Runtime.Interfaces;
    using UniRx;

    public interface IRecycleReactiveProperty<TValue> : 
        IReactiveProperty<TValue>,
        IReadonlyRecycleReactiveProperty<TValue>,
        IValueContainerStatus,
        ILifeTimeContext,
        IDisposable
#if UNITY_EDITOR
        ,IReadonlyObjectValue
        ,IObjectValue
#endif
        
    {
        new TValue Value { get; set; }
        
    }
}
