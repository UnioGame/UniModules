using System;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public interface IContextObservable<TContext>
    {
        IDisposable SubscribeOnContextChanged(Action<TContext> contextCreatedAction);
        IDisposable SubscribeOnContextRemoved(Action<TContext> removeContextAction);
    }
}