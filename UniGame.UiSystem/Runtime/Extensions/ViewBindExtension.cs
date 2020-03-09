using System;
using UniRx;

namespace UniGreenModules.UniGame.UiSystem.Runtime.Extensions
{
    using Abstracts;
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
        
        public static IView Bind<T>(
            this IView view,
            IObservable<T> source, 
            Action<T> target, 
            int frameThrottle = 1)
        {
            source.Bind(target,frameThrottle).
                AddTo(view.LifeTime);
            return view;
        }
        
        public static IView Bind<T>(
            this IView view,
            IObservable<T> source, 
            IReactiveCommand<T> target, 
            int frameThrottle = 1)
        {
            source.Bind(target,frameThrottle).
                AddTo(view.LifeTime);
            return view;
        }

    }
}
