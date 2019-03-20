using System;

namespace UniUiSystem
{
    public interface IInteractionTrigger : IObservable<IInteractionTrigger>, INamedItem
    {
        
        bool IsActive { get; }
        
        void SetState(bool active);
        
    }
}