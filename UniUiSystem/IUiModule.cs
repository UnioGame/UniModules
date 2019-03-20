using UniTools.UniUiSystem;

namespace UniUiSystem
{
    public interface IUiModule
    {
        IContainer<IUiModuleSlot> Slots { get; }
        ITriggersContainer Triggers { get; }
        void AddTrigger(IInteractionTrigger trigger);
        void AddSlot(IUiModuleSlot slot);
    }
}