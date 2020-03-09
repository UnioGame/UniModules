using System;
using UniRx;

namespace UniGreenModules.UniGame.UiSystem.Runtime.Extensions
{
    using Abstracts;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;

    public static class ViewBindExtension
    {
        public static IDisposable Bind<T>(
            this IObservable<T> source, 
            Action<T> target, 
            int frameThrottle = 1)
        {
            return source.
                ThrottleFrame(frameThrottle).
                Subscribe(target);
        }
        
        public static IDisposable Bind<T>(
            this IObservable<T> source, 
            IReactiveCommand<T> target, 
            int frameThrottle = 1)
        {
            return source.
                ThrottleFrame(frameThrottle).
                Where(x => target.CanExecute.Value).
                Subscribe(x => target.Execute(x));
        }
        
        public static IDisposable Bind<T>(
            this IView view,
            IObservable<T> source, 
            Action<T> target, 
            int frameThrottle = 1)
        {
            return source.Bind(target,frameThrottle).
                AddTo(view.LifeTime);
        }
        
        public static IDisposable Bind<T>(
            this IView view,
            IObservable<T> source, 
            IReactiveCommand<T> target, 
            int frameThrottle = 1)
        {
            return source.Bind(target,frameThrottle).
                AddTo(view.LifeTime);
        }

    }
}
