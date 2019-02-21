using UnityEngine;

namespace UniUiSystem
{
    public interface IUiModuleSlot
    {
        string SlotName { get; }
        
        RectTransform Transform { get; }
        
        
    }
}