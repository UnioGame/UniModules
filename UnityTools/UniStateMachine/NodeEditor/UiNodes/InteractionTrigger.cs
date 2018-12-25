using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniStateMachine.NodeEditor.UiNodes
{
    public class InteractionTrigger : MonoBehaviour, IInteractionTrigger
    {
        protected Subject<Object> _subject = new Subject<Object>();

        public string Name => name;

        public IObservable<Object> InteractionObservable => _subject;

        protected virtual void OnDestroy()
        {
            _subject.OnCompleted();
            _subject.Dispose();
        }

        public IDisposable Subscribe(IObserver<Object> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
