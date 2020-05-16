namespace UniGreenModules.UniCore.Runtime.Extension
{
    using System;
    using System.Collections.Generic;
    using Common;
    using ObjectPool.Runtime;

    public static class DisposableExtension
    {
        public static void DisposeItems<TTarget>(this List<TTarget> disposables)
            where  TTarget : IDisposable
        {
            for (int i = 0; i < disposables.Count; i++)
            {
                var item = disposables[i];
                item.Dispose();
            }
            disposables.Clear();
        }

        public static IDisposable AsDisposable<T>(
            this T source, 
            Action<T> cancelationAction)
        {
            var disposable = ClassPool.Spawn<DisposableAction>();
            disposable.Initialize(() => cancelationAction?.Invoke(source));
            return disposable;
        }
        
        public static IDisposable AsDisposable(
            this object source, 
            Action cancelationAction)
        {
            var disposable = ClassPool.Spawn<DisposableAction>();
            disposable.Initialize(() => cancelationAction?.Invoke());
            return disposable;
        }
        
    }
}
