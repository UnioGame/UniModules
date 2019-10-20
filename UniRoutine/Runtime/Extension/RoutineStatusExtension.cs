using System;
using System.Collections.Generic;
using UniGreenModules.UniCore.Runtime.Common;
using UniGreenModules.UniCore.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.Interfaces;
using UniGreenModules.UniCore.Runtime.ObjectPool;
using UniTools.UniRoutine.Runtime;

public static class RoutineStatusExtension
{

    public static bool Cancel(this RoutineValue value)
    {
        return UniRoutineManager.TryStopRoutine(value);
    }
    
    public static IDisposableItem ToDisposable(this RoutineValue value)
    {
        var disposable = ClassPool.Spawn<DisposableAction>();
        disposable.Initialize(() => UniRoutineManager.TryStopRoutine(value));
        return disposable;
    }

    public static ILifeTime AddTo(this RoutineValue value,ILifeTime lifeTime)
    {
        lifeTime.AddCleanUpAction(() => value.Cancel());
        return lifeTime;
    }
    
    public static IDisposableItem AddTo(this RoutineValue value,ICollection<IDisposable> collection)
    {
        var disposable = value.ToDisposable();
        collection.Add(disposable);
        return disposable;
    }
}
