namespace UniGreenModules.UniCore.Runtime.Extension
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Interfaces.Rx;
    using ObjectPool;
    using Rx;

    public static class RxExtension
    {
        public static void Cancel(this IDisposable disposable)
        {
            disposable?.Dispose();
        }
        
        public static void Cancel<TItem>(this List<TItem> disposables)
            where TItem : IDisposable
        {
            if (disposables == null)
                return;
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i]?.Dispose();
            }
        }

        public static void Cancel(this List<IDisposable> disposables)
        {
            if (disposables == null)
                return;
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i]?.Dispose();
            }
        }

        public static IRecycleObserver<T> CreateRecycleObserver<T>(this object _, 
            Action<T> onNext, 
            Action onComplete = null,
            Action<Exception> onError = null)
        {
            
            var observer = ClassPool.Spawn<RecycleActionObserver<T>>();
            
            observer.Initialize(onNext,onComplete,onError);

            return observer;
            
        }
        
        
    }
}
