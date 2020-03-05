using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniGreenModules.UniCore.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
using UnityEngine;

public static class LifetimeExtension
{
    public static LifeTimeDefinition AddTo(this LifeTimeDefinition lifeTimeDefinition, ILifeTime lifeTime)
    {
        lifeTime.AddCleanUpAction(() => lifeTimeDefinition.Terminate());
        return lifeTimeDefinition;
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
