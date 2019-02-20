using System;
using System.Collections.Generic;
using UniTools.UniUiSystem;

namespace UniUiSystem
{
    public interface ITriggersContainer : IContainer<IInteractionTrigger>
    {
        IObservable<IInteractionTrigger> TriggersObservable { get; }
        void Initialize();
    }
}