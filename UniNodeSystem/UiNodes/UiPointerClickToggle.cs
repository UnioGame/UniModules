using UniStateMachine.NodeEditor.UiNodes;
using UnityEngine.EventSystems;

namespace UniModule.UnityTools.UniVisualNodeSystem.UiNodes
{
    public class UiPointerClickToggle : InteractionTrigger, IPointerDownHandler
    {
    
        public void OnPointerDown(PointerEventData eventData)
        {
            SetState(true);
        }
    
    }
}
