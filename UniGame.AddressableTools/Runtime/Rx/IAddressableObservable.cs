namespace Taktika.Addressables.Reactive
{
    using System;
    using Object = UnityEngine.Object;

    public interface IAddressableObservable<TData> : 
        IObservable<TData>, 
        IDisposable
    {
    }
    
}