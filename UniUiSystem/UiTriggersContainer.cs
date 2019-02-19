using System;
using System.Collections.Generic;
using UniModule.UnityTools.DataFlow;
using UniRx;
using UniStateMachine.NodeEditor.UiNodes;
using UnityEngine;

namespace UniTools.UniUiSystem
{
    public class UiTriggersContainer : MonoBehaviour
    {
        #region private property
        
        private Subject<IInteractionTrigger> _interactionsSubject = new Subject<IInteractionTrigger>();

        [SerializeField]
        private List<InteractionTrigger> _triggers = new List<InteractionTrigger>();

        #endregion
        
        #region public properties
        
        public List<InteractionTrigger> Triggers => _triggers;
                
        public IObservable<IInteractionTrigger> Interactions => _interactionsSubject;

        #endregion
        
        /// <summary>
        /// collect all child trigger,
        /// this method should be called from inspector only
        /// </summary>
        public virtual void UpdateTriggers()
        {
            
            GetComponentsInChildren<InteractionTrigger>(true, _triggers);

        }
        
        protected void Awake()
        {
            foreach (var interactionTrigger in _triggers)
            {
                interactionTrigger.
                    Subscribe(x => _interactionsSubject.OnNext(interactionTrigger));
            }
        }
    }
}
