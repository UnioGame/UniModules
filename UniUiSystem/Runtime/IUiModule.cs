using Modules.UniTools.UniUiSystem.Interfaces;
using UniTools.UniUiSystem;

namespace UniUiSystem
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IUiModule
    {
        IContainer<IUiModuleSlot> Slots { get; }
        ITriggersContainer Triggers { get; }
        void AddTrigger(IInteractionTrigger trigger);
        void AddSlot(IUiModuleSlot slot);
    }
}