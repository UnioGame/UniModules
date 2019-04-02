using System;
using System.Collections.Generic;
using Modules.UniTools.UniUiSystem.Interfaces;
using UniTools.UniUiSystem;

namespace UniUiSystem
{
    public interface ITriggersContainer : IContainer<IInteractionTrigger>
    {
        IObservable<IInteractionTrigger> TriggersObservable { get; }
        void Initialize();
    }
}