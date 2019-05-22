using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniUiSystem
{
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public interface IUiModuleSlot
    {
        string SlotName { get; }
        
        RectTransform Transform { get; }

        void ApplySlotData(IContext context,ITypeData container,ILifeTime lifeTime);
    }
}