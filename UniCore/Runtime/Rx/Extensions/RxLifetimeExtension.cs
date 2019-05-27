namespace UniGreenModules.UniCore.Runtime.Rx.Extensions
{
    using System;
    using DataFlow;

    public static class RxLifetimeExtension 
    {

        public static IDisposable AddTo(this IDisposable disposable, ILifeTime lifeTime)
        {
            lifeTime.AddDispose(disposable);
            return disposable;
        }
        
    }
}
