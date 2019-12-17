using System;
using System.Collections.Generic;

namespace UniGreenModules.UniRoutine.Runtime.Extension
{
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime;

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
}
