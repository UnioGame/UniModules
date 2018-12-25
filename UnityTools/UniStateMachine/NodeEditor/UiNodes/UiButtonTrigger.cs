using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniStateMachine.NodeEditor.UiNodes
 {
     public class UiButtonTrigger : InteractionTrigger
     {
         [SerializeField]
         private Button _button;        

         private void Awake()
         {
             _button = _button ? _button : GetComponent<Button>();
             if (!_button)
             {
                 Debug.LogErrorFormat("UiButtonTrigger {0} NULL  target button ",this);
             }
             _button.onClick.AsObservable().Subscribe(x => _subject.OnNext(this));
         }
     }
 }