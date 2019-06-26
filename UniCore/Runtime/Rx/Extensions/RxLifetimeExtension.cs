namespace UniRx
{
    using System;
    using UniGreenModules.UniCore.Runtime.DataFlow;

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
