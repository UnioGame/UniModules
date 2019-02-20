using UnityEngine.EventSystems;

namespace UniUiSystem
{
    public class UiPointerClickToggle : InteractionTrigger, IPointerDownHandler
    {
    
        public void OnPointerDown(PointerEventData eventData)
        {
            SetState(true);
        }
    
    }
}
