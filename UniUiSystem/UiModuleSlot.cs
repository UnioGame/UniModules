using UnityEngine;
using UnityEngine.EventSystems;

namespace UniUiSystem
{
    /// <summary>
    /// Ui module container
    /// </summary>
    public class UiModuleSlot : UIBehaviour, IUiModuleSlot
    {

        public string SlotName => name;
        
        public RectTransform Transform => transform as RectTransform;

        
    }
    
}
