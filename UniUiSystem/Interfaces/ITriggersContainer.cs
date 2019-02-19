using System;
using System.Collections.Generic;
using UniStateMachine.NodeEditor.UiNodes;

namespace UniTools.UniUiSystem
{
    public interface ITriggersContainer : IContainer<IInteractionTrigger>
    {
        IObservable<IInteractionTrigger> Interactions { get; }
        void Initialize();
    }
}