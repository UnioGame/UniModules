namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using System;
    using Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;

    public class NodeDataActionCommand : ILifeTimeCommand, IContextWriter
    {
        private readonly IPortValue port;
        private readonly Action onAddData;
        private readonly Action onRemove;

        public NodeDataActionCommand(
            IPortValue port,
            Action onAddData,
            Action onRemove = null)
        {
            this.port = port;
            this.onAddData = onAddData;
            this.onRemove = onRemove;
        }
        
        public void Execute(ILifeTime lifeTime)
        {
            port.Connect(this);
            lifeTime.AddCleanUpAction(() => port.Disconnect(this));
        }

        public bool Remove<TData>()
        {
            onRemove?.Invoke();
            return true;
        }

        public void Add<TData>(TData data)
        {
            onAddData?.Invoke();
        }
        
        public void CleanUp()
        {
        }
    }
}
