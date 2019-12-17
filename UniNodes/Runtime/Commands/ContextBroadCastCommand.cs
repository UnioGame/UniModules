using System;

namespace UniGreenModules.UniNodes.Runtime.Commands
{
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime.Interfaces;

    [Serializable]
    public class ContextBroadCastCommand<TTarget> : ILifeTimeCommand, IContextWriter
    {
        private readonly Action<TTarget> action;
        private readonly IConnector<IContextWriter> connector;

        public ContextBroadCastCommand(Action<TTarget> action,IConnector<IContextWriter> connector)
        {
            this.action = action;
            this.connector = connector;
        }
        
        public void Execute(ILifeTime lifeTime)
        {
            connector.Connect(this);
            lifeTime.AddCleanUpAction(() => connector.Disconnect(this));
        }

        public void Publish<T>(T message)
        {
            if (message is TTarget data) {
                action(data);
            }
        }

        public bool Remove<TData>() => true;

        public void CleanUp() {}
    }
}
