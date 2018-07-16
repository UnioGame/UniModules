using System;

namespace Assets.Scripts.MessageQueue
{
    public interface IChannel : IInputChannel {

        IObserver<IMessage> Input { get; }

        IObservable<IMessage> Output { get; }
        
        IObservable<T> Attach<T>() where T : class;

    }

}
