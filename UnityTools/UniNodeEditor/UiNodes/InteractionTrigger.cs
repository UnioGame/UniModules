using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniStateMachine.NodeEditor.UiNodes
{
    public class InteractionTrigger : MonoBehaviour, IInteractionTrigger
    {
        private Subject<IInteractionTrigger> _subject = new Subject<IInteractionTrigger>();

        public string Name => name;

        public bool IsActive { get; protected set; } = false;

        public IObservable<IInteractionTrigger> InteractionObservable => _subject;

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
