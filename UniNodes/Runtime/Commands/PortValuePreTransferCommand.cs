namespace UniGreenModules.UniNodes.Runtime.Commands
{
    using System;
    using System.Collections;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime.Interfaces;
    using UniRoutine.Runtime;
    using UniRoutine.Runtime.Extension;

    [Serializable]
    public class PortValuePreTransferCommand : ILifeTimeCommand,IContextWriter
    {
        private readonly Func<IContext,IContextWriter,IEnumerator> action;
        private readonly IConnector<IContextWriter> connector;
        private readonly IContext sourceContext;
        private readonly IContextWriter target;

        private RoutineHandler handler;

        public PortValuePreTransferCommand(
            Func<IContext,IContextWriter,IEnumerator> action,
            IConnector<IContextWriter> connector,
            IContext sourceContext,
            IContextWriter target)
        {
            this.action = action;
            this.connector = connector;
            this.sourceContext = sourceContext;
            this.target = target;
        }
        
        public void Execute(ILifeTime lifeTime)
        {
            connector.Connect(this);
            lifeTime.AddCleanUpAction(() => connector.Disconnect(this));
            lifeTime.AddCleanUpAction(() => handler.Cancel());
        }

        public void Publish<T>(T message)
        {
            handler = OnPublish(message).Execute();
        }

        public bool Remove<TData>() => true;

        public void CleanUp() {}

        private IEnumerator OnPublish<T>(T message)
        {
            yield return action(sourceContext, target);
            target.Publish(message);
        }
    }
}
