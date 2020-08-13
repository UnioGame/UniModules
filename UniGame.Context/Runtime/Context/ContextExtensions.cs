namespace UniModules.UniGame.Context.Runtime.Context
{
    using System;
    using Core.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;

    public static class ContextExtensions
    {
        public static IObservable<Unit> ReceiveFirst<T>(this IContext targetContext, IContext sourceContext) where T : class
        {
            return sourceContext.Receive<T>()
                .First()
                .Do(targetContext.Publish)
                .Do(x => GameLog.Log($"{typeof(T).Name} OnServiceLoaded",Color.magenta))
                .AsUnitObservable();
        }
        
        public static IObservable<T> ReceiveFirst<T>(this IContext sourceContext, Action<T> action) where T : class
        {
            return sourceContext.Receive<T>()
                .First()
                .Do(action);
        }

        public static IDisposable LogValue<T>(this IContext context)
        {
            return context.
                Receive<T>().
                Do(x => GameLog.Log($"{typeof(T).Name} CONTEXT Get {x.GetType().Name}", Color.red)).
                Subscribe();
        }
        
        public static IDisposable LogValue<T>(this IContext context,string id)
        {
            return context.
                Receive<T>().
                Do(x => GameLog.Log($"{id} CONTEXT Get {x.GetType().Name}", Color.red)).
                Subscribe();
        }
        
        public static IContext LogValue<T>(this IContext context,string id, ILifeTime lifeTime)
        {
            context.LogValue<T>(id).
                AddTo(lifeTime);
            return context;
        }
        
        public static IObservable<Unit> ReceiveFirst<T>(this IContext targetContext, IObservable<T> sourceContext) where T : class
        {
            return sourceContext.First()
                .Do(targetContext.Publish)
                .Do(x => GameLog.Log($"{typeof(T).Name} OnServiceLoaded",Color.magenta))
                .AsUnitObservable();
        }
    }
}