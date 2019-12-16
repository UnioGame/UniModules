namespace UniGreenModules.UniCore.Runtime.Rx.Extensions
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Rx;
    using ObjectPool.Runtime;
    using Rx;

    public static class RxExtension
    {
        public static IDisposable Cancel(this IDisposable disposable, bool clearValue = true)
        {
            disposable?.Dispose();
            return clearValue ? null : disposable;
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
            
            disposables.Clear();
        }

        public static void Cancel(this List<IDisposable> disposables)
        {
            if (disposables == null)
                return;
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i]?.Dispose();
            }
            
            disposables.Clear();
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
