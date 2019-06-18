namespace UniGreenModules.UniUiNodes.Runtime
 {
     using Triggers;
     using UnityEngine.EventSystems;

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