namespace UniGreenModules.UniCore.Runtime.Rx.Extensions
{
    using System;
    using DataFlow;

    public static class RxLifetimeExtension 
    {

        public static IDisposable AddTo(this IDisposable disposable, ILifeTime lifeTime)
        {
            if (disposable != null)
                lifeTime.AddDispose(disposable);
            return disposable;
        }
        
    }
}
