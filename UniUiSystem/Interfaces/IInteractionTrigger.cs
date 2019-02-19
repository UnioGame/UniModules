using System;

namespace UniTools.UniUiSystem
{
    public interface IInteractionTrigger : IObservable<IInteractionTrigger>
    {
        
        string Name { get; }

        bool IsActive { get; }
        
        void SetState(bool active);
    }
}