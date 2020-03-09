using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniGreenModules.UniCore.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UnityEngine;

public static class LifetimeExtension
{
    public static LifeTimeDefinition AddTo(this LifeTimeDefinition lifeTimeDefinition, ILifeTime lifeTime)
    {
        lifeTime.AddCleanUpAction(lifeTimeDefinition.Terminate);
        return lifeTimeDefinition;
    }

    public static TLifeTime AddCleanUpAction<TLifeTime>(this TLifeTime context,Action action)
        where TLifeTime : ILifeTimeContext
    {
        context.LifeTime.AddCleanUpAction(action);
        return context;
    }
    
    public static TLifeTime AddDisposable<TLifeTime>(this TLifeTime context,IDisposable action)
        where TLifeTime : ILifeTimeContext
    {
        context.LifeTime.AddDispose(action);
        return context;
    }
                    
    #region type convertion

    public static CancellationTokenSource AsCancellationSource(this ILifeTime lifeTime)
    {
        var tokenSource = new CancellationTokenSource();
        lifeTime.AddDispose(tokenSource);
        return tokenSource;
    } 

    #endregion
}
