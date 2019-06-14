namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public interface IUiModuleSlot
    {
        string SlotName { get; }
        
        RectTransform Transform { get; }

        void ApplySlotData(IContext context,ITypeData container,ILifeTime lifeTime);
    }
}