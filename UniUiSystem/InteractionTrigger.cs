using System;
using UniRx;
using UnityEngine;

namespace UniUiSystem
{
    public class InteractionTrigger : MonoBehaviour, IInteractionTrigger
    {
        private string _name;
        
        private Subject<IInteractionTrigger> _subject = new Subject<IInteractionTrigger>();

        public string ItemName => _name;

        public bool IsActive { get; protected set; } = false;

        public IObservable<IInteractionTrigger> InteractionObservable => _subject;

        public void SetName(string itemName)
        {
            _name = name;
        }
        
        public void SetState(bool active)
        {
            IsActive = active;
            _subject.OnNext(this);
        }
        
        protected virtual void OnDestroy()
        {
            _subject.OnCompleted();
            _subject.Dispose();
        }

        public IDisposable Subscribe(IObserver<IInteractionTrigger> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
