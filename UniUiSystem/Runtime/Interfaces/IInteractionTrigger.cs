using System;

namespace Modules.UniTools.UniUiSystem.Interfaces
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IInteractionTrigger :
        IObservable<IInteractionTrigger>, 
        INamedItem
    {
        
        bool IsActive { get; }
        
        void SetState(bool active);
        
    }
}