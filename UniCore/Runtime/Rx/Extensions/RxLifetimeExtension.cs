namespace UniRx
{
    using System;
    using UniGreenModules.UniCore.Runtime.DataFlow;

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
