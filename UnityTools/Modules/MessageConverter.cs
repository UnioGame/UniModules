using System;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;

namespace Assets.Modules.UnityToolsModule.Tools.UnityTools.Modules
{

    public abstract class MessageConverter
    {

        public abstract IDisposable Register(IMessageBroker receiver, IContext context);

    }
}
