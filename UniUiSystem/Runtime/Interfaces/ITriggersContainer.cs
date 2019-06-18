namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    using System;
    using UniCore.Runtime.Interfaces;

    public interface ITriggersContainer : IContainer<IInteractionTrigger>
    {
        IObservable<IInteractionTrigger> TriggersObservable { get; }
        
    }
}