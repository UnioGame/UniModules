    namespace UniGreenModules.UniUiNodes.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniUiSystem.Runtime.Interfaces;

    public interface IUiModule : IUiView<IValueReceiver>
    {
        
        IContainer<IUiModuleSlot> Slots { get; }
        
        ITriggersContainer Triggers { get; }
        
        void AddTrigger(IInteractionTrigger trigger);
        
        void AddSlot(IUiModuleSlot slot);
    }
}