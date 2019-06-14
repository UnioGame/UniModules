namespace UniGreenModules.UniUiSystem.Runtime
{
    using GBG.UI.Runtime;
    using Interfaces;
    using Triggers;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class UiModule : UiView<IValueReceiver>, IUiModule
    {
        #region inspector data
        
        [SerializeField]
        private UiModuleSlotsContainer slots = new UiModuleSlotsContainer();

        [SerializeField]
        private UiTriggersContainer triggers = new UiTriggersContainer();

        #endregion
    
        #region public properties

        public IContainer<IUiModuleSlot> Slots => slots;
        
        public ITriggersContainer Triggers => triggers;
        
        #endregion

        #region public methods

        public void SetParent(RectTransform parent)
        {
            var rectTransform = RectTransform;
            rectTransform.SetParent(parent,false);
        }
        
        public void AddTrigger(IInteractionTrigger trigger)
        {
            triggers.Add(trigger);
        }

        public void AddSlot(IUiModuleSlot slot)
        {
            slots.Add(slot);
        }

        private void OnValidate()
        {

            var slotsItems = GetComponentsInChildren<UiModuleSlot>(true);
            var triggersItems = GetComponentsInChildren<InteractionTrigger>(true);
            
            triggers.Release();
            triggers.AddRange(triggersItems);
            
            slots.Release();
            slots.AddRange(slotsItems);
            
            OnModuleValidate();
        }

#endregion

        protected virtual void OnModuleValidate(){}
    }
}
