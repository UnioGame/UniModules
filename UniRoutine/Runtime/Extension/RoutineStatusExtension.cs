using System;
using System.Collections.Generic;
using UniGreenModules.UniCore.Runtime.Common;
using UniGreenModules.UniCore.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniCore.Runtime.ObjectPool;
using UniTools.UniRoutine.Runtime;

public static class RoutineStatusExtension
{

    public static bool Cancel(this RoutineHandler handler)
    {
        return UniRoutineManager.TryToStopRoutine(handler);
    }
    
    public static IDisposableItem AsDisposable(this RoutineHandler handler)
    {
        var disposable = ClassPool.Spawn<DisposableAction>();
        disposable.Initialize(() => UniRoutineManager.TryToStopRoutine(handler));
        return disposable;
    }

    public static ILifeTime AddTo(this RoutineHandler handler,ILifeTime lifeTime)
    {
        lifeTime.AddCleanUpAction(() => handler.Cancel());
        return lifeTime;
    }
    
    public static IDisposableItem AddTo(this RoutineHandler handler,ICollection<IDisposable> collection)
    {
        var disposable = handler.AsDisposable();
        collection.Add(disposable);
        return disposable;
    }
}
