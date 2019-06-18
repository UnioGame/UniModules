    namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    using GBG.UI.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public interface IUiModule : IUiView<IValueReceiver>
    {
        
        IContainer<IUiModuleSlot> Slots { get; }
        
        ITriggersContainer Triggers { get; }
        
        void AddTrigger(IInteractionTrigger trigger);
        
        void AddSlot(IUiModuleSlot slot);
    }
}