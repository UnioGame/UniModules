namespace UniGreenModules.UniUiSystem.Runtime
{
    using Interfaces;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class UiModule : UiViewBehaviour, IUiModule
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
        
        #endregion

        protected override void OnInitialize()
        {
            triggers.Initialize();
            slots.UpdateCollection();
            base.OnInitialize();
        }
    }
}
