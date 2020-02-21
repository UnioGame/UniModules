using System;

namespace Taktika.Addressables.Reactive
{
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UnityEngine.AddressableAssets;
    using Object = UnityEngine.Object;

    public static class AddressablesReactiveExtensions 
    {
        public static IAddressableObservable<TApi> ToObservable<TData,TApi>(this AssetReferenceT<TData> reference) 
            where TData : Object , TApi
            where TApi : class
        {
            var observable = ClassPool.Spawn<AddressableObservable<AssetReferenceT<TData>, TData,TApi>>();
            observable.Initialize(reference);
            return observable;
        }
        
        public static IAddressableObservable<TData> ToObservable<TData>(this AssetReferenceT<TData> reference) 
            where TData : Object
        {
            var observable = ClassPool.Spawn<AddressableObservable<AssetReferenceT<TData>, TData,TData>>();
            observable.Initialize(reference);
            return observable;
        }
        
        
        public static IAddressableObservable<TApi> ToObservable<TApi>(this AssetReference reference) 
            where TApi : class
        {
            var observable = ClassPool.Spawn<AddressableObservable<AssetReference, Object,TApi>>();
            observable.Initialize(reference);
            return observable;
        }
        
        
        public static IAddressableObservable<Object> ToObservable(this AssetReference reference) 
        {
            var observable = ClassPool.Spawn<AddressableObservable<AssetReference, Object,Object>>();
            observable.Initialize(reference);
            return observable;
        }
        
    }
}
