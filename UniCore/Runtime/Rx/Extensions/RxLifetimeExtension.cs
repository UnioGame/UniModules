namespace UniGreenModules.UniCore.Runtime.Rx.Extensions
{
    using System;
    using DataFlow;

    public static class RxLifetimeExtension 
    {

        public static T AddTo<T>(this T disposable, ILifeTime lifeTime)
            where T : class, IDisposable
        {
            if (disposable != null)
                lifeTime.AddDispose(disposable);
            return disposable;
        }
        
    }
}
