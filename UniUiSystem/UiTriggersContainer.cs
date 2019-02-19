using System;
using UniRx;

namespace UniTools.UniUiSystem
{
    public class UiTriggersContainer : 
        AdapterContainer<InteractionTrigger,IInteractionTrigger>, 
        ITriggersContainer
    {
        #region private property
      
        private Subject<IInteractionTrigger> _interactionsSubject = new Subject<IInteractionTrigger>();

        #endregion
        
        #region public properties                

        public IObservable<IInteractionTrigger> Interactions => _interactionsSubject;
        
        #endregion

        public void Initialize()
        {
            UpdateCollection();
        }
        
        /// <summary>
        /// collect all child trigger,
        /// this method should be called from inspector only
        /// </summary>
        public virtual void CollectTriggers()
        {
            
            GetComponentsInChildren(true, _items);
            UpdateCollection();
            
        }
        
        protected void Awake()
        {
            foreach (var interactionTrigger in _items)
            {
                interactionTrigger.Subscribe(x => 
                    _interactionsSubject.OnNext(interactionTrigger));
            }
        }
    }
}
