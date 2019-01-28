using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniStateMachine.NodeEditor.UiNodes
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