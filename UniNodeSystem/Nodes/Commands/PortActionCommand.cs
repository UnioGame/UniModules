namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using System;
    using Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniRx;

    [Serializable]
    public class PortActionCommand<TTarget> : ILifeTimeCommand
    {
        private readonly IPortValue port;
        private readonly Action<TTarget> action;

        public PortActionCommand(Action<TTarget> action,IPortValue port)
        {
            this.action = action;
            this.port = port;
        }
        
        public void Execute(ILifeTime lifeTime)
        {
            port.Receive<TTarget>().
                Subscribe(action).
                AddTo(lifeTime);
        }
        
    }
}
