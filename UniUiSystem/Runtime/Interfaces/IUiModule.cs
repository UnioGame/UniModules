namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;

    public interface IUiModule
    {
        IContainer<IUiModuleSlot> Slots { get; }
        ITriggersContainer Triggers { get; }
        void AddTrigger(IInteractionTrigger trigger);
        void AddSlot(IUiModuleSlot slot);
    }
}