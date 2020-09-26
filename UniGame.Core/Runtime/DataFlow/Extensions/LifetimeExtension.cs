using System;
using System.Threading;
using UniGame.Core.Runtime.Common;
using UniGame.Core.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.DataFlow.Interfaces;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
using UniModules.UniGame.Core.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.Core.Runtime.Interfaces;

public static class LifetimeExtension
{
    public static LifeTimeDefinition AddTo(this LifeTimeDefinition lifeTimeDefinition, ILifeTime lifeTime)
    {
        lifeTime.AddCleanUpAction(lifeTimeDefinition.Terminate);
        return lifeTimeDefinition;
    }

    public static LifeTimeDefinition ReleaseWith(this LifeTimeDefinition lifeTimeDefinition, ILifeTime lifeTime)
    {
        lifeTime.AddCleanUpAction(lifeTimeDefinition.Release);
        return lifeTimeDefinition;
    }
    
    public static TLifeTime AddCleanUpAction<TLifeTime>(this TLifeTime context,Action action)
        where TLifeTime : ILifeTimeContext
    {
        context.LifeTime.AddCleanUpAction(action);
        return context;
    }
    
    public static ILifeTimedAction CreateLifeTimedAction<TLifeTime>(
        this ILifeTime lifeTime,
        Action action,
        Action onLifetimeFinished = null)
    {
        var lifetimeAction = ClassPool.Spawn<LifeTimedAction>();
        lifetimeAction.Initialize(lifeTime,action,onLifetimeFinished);
        return lifetimeAction;
    }

    public static IDisposableCommand CreateLifeTimeCommand<TLifeTime>(this Action<ILifeTime> action)
    {
        var lifetimeAction = ClassPool.Spawn<LifeTimeContextCommand>();
        lifetimeAction.Initialize(action);
        return lifetimeAction;
    }
    
    public static TLifeTime AddDisposable<TLifeTime>(this TLifeTime context,IDisposable action)
        where TLifeTime : ILifeTimeContext
    {
        context.LifeTime.AddDispose(action);
        return context;
    }
       
    public static IComposedLifeTime Compose(this ILifeTime source, params  ILifeTime[] lifeTimes)
    {
        var composeAction = ClassPool.Spawn<ComposedLifeTime>();
        return composeAction.
            Bind(source).
            Bind(lifeTimes);
    }

    public static ILifeTime ComposeCleanUp(
        this ILifeTime source,
        ILifeTime additional,
        Action cleanup) => ComposeCleanUp(source, cleanup, additional);
    
    public static ILifeTime ComposeCleanUp(
        this ILifeTime source, 
        Action cleanup,
        params ILifeTime[] additional)
    {
        var composeAction = new LifeTimeCompose();
        
        composeAction.AddCleanUpAction(cleanup);
        composeAction.Add(source);
        composeAction.Add(additional);
        
        return source;
    }

    #region type convertion

    public static CancellationTokenSource AsCancellationSource(this ILifeTime lifeTime)
    {
        var tokenSource = new CancellationTokenSource();
        lifeTime.AddCleanUpAction(tokenSource.Cancel);
        lifeTime.AddDispose(tokenSource);
        return tokenSource;
    } 

    public static CancellationToken AsCancellationToken(this ILifeTime lifeTime)
    {
        return lifeTime.AsCancellationSource().Token;
    } 
    
    #endregion
}
