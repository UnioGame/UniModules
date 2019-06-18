namespace UniGreenModules.UniUiNodes.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.Interfaces;

    public interface IInteractionTrigger :
        IObservable<IInteractionTrigger>, 
        INamedItem
    {
        
        bool IsActive { get; }
        
        void SetState(bool active);
        
    }
}