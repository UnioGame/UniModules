using System;
using UnityTools.Runtime.Interfaces;

namespace Modules.UniTools.UniUiSystem.Interfaces
{
    public interface IInteractionTrigger :
        IObservable<IInteractionTrigger>, 
        INamedItem
    {
        
        bool IsActive { get; }
        
        void SetState(bool active);
        
    }
}