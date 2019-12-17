namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using System;
    using Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;

    [Serializable]
    public class PortDataBridgeActionCommand<TTarget> : ILifeTimeCommand
    {
        private readonly IContext inputPort;
        private readonly IContext outputPort;
        private readonly Action<TTarget,IContext,IContext> action;

        public PortDataBridgeActionCommand(Action<TTarget,IContext,IContext> action,IContext input,IContext output)
        {
            this.action = action;
            this.inputPort = input;
            this.outputPort = output;
        }
        
        public void Execute(ILifeTime lifeTime)
        {
            inputPort.Receive<TTarget>().
                Do(x => action(x,inputPort,outputPort)).
                Subscribe().
                AddTo(lifeTime);
        }
        
    }
}
