namespace UniGreenModules.UniUiSystem.Runtime
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
