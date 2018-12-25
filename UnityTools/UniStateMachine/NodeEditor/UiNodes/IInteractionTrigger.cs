using System;
using Object = UnityEngine.Object;

namespace UniStateMachine.NodeEditor.UiNodes
{
    public interface IInteractionTrigger : IObservable<Object>
    {
        string Name { get; }

    }
}