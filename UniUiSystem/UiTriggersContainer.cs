using System;
using Modules.UniTools.UniUiSystem.Interfaces;
using UniRx;
using UniTools.UniUiSystem;

namespace UniUiSystem
{
    using UniGreenModules.UniCore.Runtime.Common;

    [Serializable]
    public class UiTriggersContainer : 
        UniObjectsContainer<InteractionTrigger,IInteractionTrigger>, 
        ITriggersContainer
    {
        #region private property
      
        private Subject<IInteractionTrigger> _interactionsSubject;

        #endregion
        
        #region public properties                

        public IObservable<IInteractionTrigger> TriggersObservable => _interactionsSubject;
        
        #endregion

        public override void Release()
        {
            _interactionsSubject?.Dispose();
            base.Release();
        }

        public void Initialize()
        {
            UpdateCollection();
            
            _interactionsSubject = CreateInteractionObservable();
        }
        
        protected Subject<IInteractionTrigger> CreateInteractionObservable()
        {
            var observable = new Subject<IInteractionTrigger>();
            
            foreach (var interactionTrigger in _items)
            {
                interactionTrigger.Subscribe(x => 
                    _interactionsSubject.OnNext(interactionTrigger));
            }

            return observable;
        }
    }
}
