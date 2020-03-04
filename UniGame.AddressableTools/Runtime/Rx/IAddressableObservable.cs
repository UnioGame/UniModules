namespace Taktika.Addressables.Reactive
{
    using System;
    using UniRx;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using Object = UnityEngine.Object;

    public interface IAddressableObservable<TData> : 
        IObservable<TData>, 
        IDisposable
    {
        IReadOnlyReactiveProperty<AsyncOperationStatus> Status { get; }

        IReadOnlyReactiveProperty<float> Progress  { get; }

        IReadOnlyReactiveProperty<bool> IsReady  { get; }

        IReadOnlyReactiveProperty<TData> Value { get; }
    }
    
}