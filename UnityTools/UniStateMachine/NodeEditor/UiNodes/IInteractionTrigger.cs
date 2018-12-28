using System;
using Object = UnityEngine.Object;

namespace UniStateMachine.NodeEditor.UiNodes
{
    public interface IInteractionTrigger : IObservable<IInteractionTrigger>
    {
        
        string Name { get; }

        bool IsActive { get; }
        
        void SetState(bool active);
    }
}