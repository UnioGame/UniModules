
namespace UniGreenModules.UniUiSystem.Runtime
{
    using Interfaces;
    using Models;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Ui module container
    /// </summary>
    public class UiModuleSlot : UIBehaviour, IUiModuleSlot
    {
        public string SlotName => name;
        
        public RectTransform Transform => transform as RectTransform;

        public virtual void Add(IUiModule target)
        {
            target?.RectTransform.SetParent(transform);
        }

    }
    
}
