namespace UniGreenModules.UniNodes.Runtime.Commands
{
    using System;
    using System.Collections;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime.Interfaces;
    using UniRoutine.Runtime.Extension;

    [Serializable]
    public class PortValueTransferDelayCommand : ILifeTimeCommand
    {
        private readonly float delay;
        private readonly PortValuePreTransferCommand transferCommand;
        
        public PortValueTransferDelayCommand(IPortValue input, IPortValue output, float delay)
        {
            this.delay = delay;
            transferCommand = new PortValuePreTransferCommand(DelayAction,input,input,output);
        }
        
        public void Execute(ILifeTime lifeTime)
        {
            transferCommand.Execute(lifeTime);
        }

        private IEnumerator DelayAction(IContext source,IContextWriter target)
        {
            yield return this.WaitForSeconds(delay);
        }
        
    }
}
