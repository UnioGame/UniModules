using UnityEngine.EventSystems;

namespace UniUiSystem
 {
     public class UiPointerClickTrigger : InteractionTrigger, IPointerDownHandler, IPointerUpHandler
     {
         public void OnPointerDown(PointerEventData eventData)
         {
             SetState(true);
         }

         public void OnPointerUp(PointerEventData eventData)
         {
             SetState(false);
         }
         
     }
 }