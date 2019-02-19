using System.Collections.Generic;
using UniModule.UnityTools.UiViews;
using UnityEngine;

namespace UniTools.UniUiSystem
{
    public class UiModule : UiViewBehaviour
    {
        #region inspector data
        
        [SerializeField]
        private List<UiModuleSlot> _slots;

        [SerializeField]
        private UiTriggersContainer _triggers;

        #endregion

        public void Initialize()
        {
            
        }
        
        #region public properties

        public List<UiModuleSlot> Slots => 
        
        public ITriggersContainer TriggersContainer => _triggers;
        
        #endregion
        
        #region editor only methods

        public void CollectModuleSlots()
        {
            gameObject.GetComponentsInChildren<UiModuleSlot>(true, Slots);
        }

        public void CollectTriggers()
        {
            UiTriggers = GetComponentInChildren<UiTriggersContainer>();
            UiTriggers.CollectTriggers();
        }
        
        #endregion
        
    }
}
