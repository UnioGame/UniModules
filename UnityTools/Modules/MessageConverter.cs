using System;
using UniModule.UnityTools.Interfaces;
using UniRx;

namespace UniModule.UnityTools.Modules
{

    public abstract class MessageConverter
    {

        public abstract IDisposable Register(IMessageBroker receiver, IContext context);

    }
}
