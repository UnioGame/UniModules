namespace UniGreenModules.UniUiSystem.Runtime.UiNodes
{
    using Triggers;
    using UnityEngine.EventSystems;

    public class UiPointerClickToggle : InteractionTrigger, IPointerDownHandler
    {
    
        public void OnPointerDown(PointerEventData eventData)
        {
            SetState(true);
        }
    
    }
}
