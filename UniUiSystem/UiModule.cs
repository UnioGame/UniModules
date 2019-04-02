using System.Collections.Generic;
using Modules.UniTools.UniUiSystem.Interfaces;
using UnityEngine;
using UniTools.UniUiSystem;

namespace UniUiSystem
{
    public class UiModule : UiViewBehaviour, IUiModule
    {
        #region inspector data
        
        [SerializeField]
        private UiModuleSlotsContainer _slots = new UiModuleSlotsContainer();

        [SerializeField]
        private UiTriggersContainer _triggers = new UiTriggersContainer();

        #endregion
    
        #region public properties

        public IContainer<IUiModuleSlot> Slots => _slots;
        
        public ITriggersContainer Triggers => _triggers;
        
        #endregion

        #region public methods

        public void SetParent(RectTransform parent)
        {
            var rectTransform = RectTransform;
            rectTransform.SetParent(parent,false);
        }
        
        public void AddTrigger(IInteractionTrigger trigger)
        {
            _triggers.Add(trigger);
        }

        public void AddSlot(IUiModuleSlot slot)
        {
            _slots.Add(slot);
        }
        
        #endregion

        protected override void OnInitialize()
        {
            _triggers.Initialize();
            _slots.UpdateCollection();
            base.OnInitialize();
        }
    }
}
