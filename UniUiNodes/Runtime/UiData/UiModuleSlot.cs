
namespace UniGreenModules.UniUiNodes.Runtime.UiData
{
    using System;
    using Interfaces;
    using UniNodeSystem.Runtime.Connections;
    using UniNodeSystem.Runtime.Interfaces;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Ui module container
    /// </summary>
    public class UiModuleSlot : 
        UIBehaviour, 
        IUiModuleSlot,
        IDisposable
    {
        private TypeDataBrodcaster value = new TypeDataBrodcaster();

        public string SlotName => name;

        public ITypeDataBrodcaster Value => value;
     
        public void Dispose()
        {
            value.Release();
        }

        public virtual void Insert(RectTransform targetTransfom)
        {
            targetTransfom.SetParent(transform,false);
        }
    }
    
}
