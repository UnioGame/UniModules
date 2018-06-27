using System;

namespace Assets.Scripts.MessageQueue
{
    public interface IConsumer
    {
        IObservable<TMessage> Bind<TMessage>() where TMessage : Message;
    }
}
