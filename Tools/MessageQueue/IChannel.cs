using System;
using System.Collections.Generic;

namespace Assets.Scripts.MessageQueue
{
    public interface IChannel : IDisposable {

        bool IsDisposed { get; }

        IObservable<IMessage> Input { get; }

        IObservable<IMessage> Output { get; }

        IObservable<Type> OnAttach { get; }

        IObservable<Type> OnUnsubscribe { get; }

        void Send(IMessage message);

        void SendToInput(IMessage message);

        bool IsRegistered(Type type);

        IEnumerable<Type> GetRegisteredTypes(); 
            
        IDisposable Attach<T>(Action<T> messageAction, Func<T, bool> filter = null) where T : class;

    }

}
