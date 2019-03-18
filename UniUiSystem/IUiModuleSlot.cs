using UniModule.UnityTools.Common;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniUiSystem
{
    public interface IUiModuleSlot
    {
        string SlotName { get; }
        
        RectTransform Transform { get; }

        void ApplySlotData(IContext context,ITypeData container,ILifeTime lifeTime);
    }
}